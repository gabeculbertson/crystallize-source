using UnityEngine;
using System.Collections;

public class MultiplayerSecondLevelTutorialScript : LevelScript {

    enum CommunicationState {
        None,
        OutOfRange,
        AwaitingPickup,
        AwaitingDrop,
        AwaitingConfirm,
        AwaitingApproachOther,
        AwaitingSay,
        AwaitingOtherSay,
        Complete
    }

    public Transform questClient;
    public DialogueActor actor;
    public GameObject downArrow;
    public int wordID;

    bool engagedWithActor;

    GameObject speechBubbleInstance;

    // Use this for initialization
    IEnumerator Start() {
        ObjectiveManager.main.SetObjective(this, false);

        downArrow.SetActive(false);

        // Tell the player to start the quest
        SetMessage("Approach the the client. (He is the one with the exclamation point above his head.)");

        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.Environment.OnActorApproached -= HandleActorApproached;

        SetMessage("Accept the quest.");

        yield return null;

        // Wait for the player to start the quest
        CrystallizeEventManager.UI.OnUIInteraction += HandleQuestStateChanged;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.UI.OnUIInteraction -= HandleQuestStateChanged;


        SetMessage("Wait for your partner...");
        while (true) {
            var quest = GameData.Instance.QuestData.GetQuestInfoFromWorldID(questClient.GetWorldID());
            var questInstance = PlayerData.Instance.QuestData.GetQuestInstance(quest.QuestID);
            //Debug.Log(questInstance);
            if (questInstance != null) {
                //Debug.Log(questInstance.QuestID + "; " + questInstance.GetObjectiveState(0));
                if (questInstance.GetObjectiveState(0).IsComplete) {
                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
        //SetMessage("");

        // Get the players to learn a word
        CrystallizeEventManager.Environment.OnActorApproached += HandlePassiveActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted += HandlePassiveActorDeparted;
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleSpeechBubbleOpen;

        //downArrow.SetActive(true);
        //SetMessage("To learn words, approach pairs to overhear conversations");

        var lastState = CommunicationState.None;
        while (true) {
            var newState = GetLearnState();
            if (newState == CommunicationState.AwaitingApproachOther) {
                break;
            }

            if (newState != lastState) {
                yield return StartCoroutine(SetLearnState(newState));
                //yield return null;
                SetLearnState(newState);
                lastState = newState;
            }

            yield return null;
        }

        CrystallizeEventManager.Environment.OnActorApproached -= HandlePassiveActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted -= HandlePassiveActorDeparted;
        CrystallizeEventManager.UI.OnSpeechBubbleOpen -= HandleSpeechBubbleOpen;

        TutorialCanvas.main.ClearAllIndicators();
        ClearMessages();

        lastState = CommunicationState.None;
        while (true) {
            var newState = GetSayState();
            if (newState == CommunicationState.Complete) {
                break;
            }

            if (newState != lastState) {
                SetSayState(newState);
                lastState = newState;
            }

            yield return null;
        }

        TutorialCanvas.main.ClearAllIndicators();
        ClearMessages();

        ObjectiveManager.main.SetObjective(this, true);
        //SetMessage("Go upstairs to continue");
    }

    //TODO: make this more robust
    void HandleQuestStateChanged(object sender, System.EventArgs e) {
        if (e is QuestConfirmedEventArgs) {
            Continue();
        }
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        var c = sender as Component;
        if (!c) {
            Debug.Log("Not component! " + sender);
        }

        if (c.transform == questClient) {
            Continue();
        }
    }

    void HandlePassiveActorApproached(object sender, System.EventArgs e) {
        if (sender is DialogueGroup) {
            engagedWithActor = true;
        }
    }

    void HandlePassiveActorDeparted(object sender, System.EventArgs e) {
        if (sender is DialogueGroup) {
            engagedWithActor = false;
        }
    }

    void HandleSpeechBubbleOpen(object sender, PhraseEventArgs e) {
        if (e.Phrase == null) {
            return;
        }

        if (e.Phrase.PhraseElements[0].WordID == wordID) {
            speechBubbleInstance = sender as GameObject;
        }
    }

    CommunicationState GetLearnState() {
        if (ObjectiveManager.main.IsWordFound(wordID)) {
            return CommunicationState.AwaitingApproachOther;
        }

        if (UISystem.main.PhraseDragHandler.IsDragging) {
            return CommunicationState.AwaitingDrop;
        }

        if (engagedWithActor) {
            return CommunicationState.AwaitingPickup;
        }

        return CommunicationState.OutOfRange;
    }

    IEnumerator SetLearnState(CommunicationState state) {
        downArrow.SetActive(false);
        TutorialCanvas.main.ClearAllIndicators();
        ClearMessages();
        switch (state) {
            case CommunicationState.OutOfRange:
                downArrow.SetActive(true);
                SetMessage("To learn words, approach pairs to overhear conversations.");
                break;

            case CommunicationState.AwaitingPickup:
                while (!speechBubbleInstance) {
                    yield return null;
                }
                SetMessage("Drag or left-click on words from speech bubbles to learn them.");
                TutorialCanvas.main.CreateUIDragBox(speechBubbleInstance.GetComponent<RectTransform>(), "drag or left-click");
                break;

            case CommunicationState.AwaitingDrop:
                TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.InventoryUI.GetEntry(new PhraseSequenceElement(wordID, 0)), "...to here.");
                break;
        }

        yield return null;
    }

    CommunicationState GetSayState() {
        var gid = questClient.GetWorldID();
        var qid = GameData.Instance.QuestData.GetQuestInfoFromWorldID(gid).QuestID;
        var qpd = PlayerData.Instance.QuestData.GetQuestInstance(qid);
        if (qpd.State == ObjectiveState.Complete) {
            return CommunicationState.Complete;
        }

        if (qpd.GetObjectiveState(1).IsComplete && !qpd.GetObjectiveState(2).IsComplete) {
            return CommunicationState.AwaitingOtherSay;
        }

        var fi = TutorialCanvas.main.GetRegisteredGameObject("FreeInput").GetComponent<DragDropFreeInputUI>();
        var desiredPhrase = new PhraseSequence();
        desiredPhrase.Add(new PhraseSequenceElement(wordID, 0));
        if (fi.Phrase.FulfillsTemplate(desiredPhrase)) {
            return CommunicationState.AwaitingConfirm;
        }

        if (InteractionManager.IsInteractingWithOtherPlayer() && UISystem.main.PhraseDragHandler.IsDragging) {
            return CommunicationState.AwaitingDrop;
        }

        if (InteractionManager.IsInteractingWithOtherPlayer() && !UISystem.main.PhraseDragHandler.IsDragging) {
            return CommunicationState.AwaitingPickup;
        }

        return CommunicationState.AwaitingApproachOther;
    }

    void SetSayState(CommunicationState state) {
        downArrow.SetActive(false);
        TutorialCanvas.main.ClearAllIndicators();
        ClearMessages();
        switch (state) {
            case CommunicationState.AwaitingApproachOther:
                SetMessage("Approach your partner.");
                break;

            case CommunicationState.AwaitingPickup:
                SetMessage("Drag or left-click words from the inventory to say them.");
                TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.InventoryUI.GetEntry(new PhraseSequenceElement(wordID, 0)), "drag or left-click");
                break;

            case CommunicationState.AwaitingDrop:
                TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.GetRegisteredGameObject("FreeInput").GetComponent<RectTransform>(), "...to here.");
                break;

            case CommunicationState.AwaitingConfirm:
                TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.GetRegisteredGameObject("FreeInputConfirm").GetComponent<RectTransform>(), "Click here!");
                break;

            case CommunicationState.AwaitingOtherSay:
                SetMessage("Now wait for your partner...");
                break;
        }
    }

}
