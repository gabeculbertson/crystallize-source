using UnityEngine;
using System.Collections;

public class Multiplayer03TutorialScript : LevelScript {

    enum TutorialState {
        Disengaged,
        AwaitingDrag,
        AwaitingDrop,
        Complete
    }

    public int wordID;

    public Transform questClient;
    GameObject speechBubbleInstance;

	// Use this for initialization
	IEnumerator Start () {
		ObjectiveManager.main.SetObjective (this, false);
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;

        SetMessage("Complete the quest.");
        /*SetMessage("Wait for your partner...");
        while (true) {
            var quest = GameData.Instance.QuestData.GetQuestInfoFromWorldID(questClient.GetWorldID());
            var questInstance = PlayerManager.main.playerData.QuestData.GetQuestInstance(quest.QuestID);
            if (questInstance != null) {
                if (questInstance.GetObjectiveState(0).IsComplete) {
                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }*/

        CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleSpeechBubbleOpen;
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += main_OnSpeechBubbleOpen;

        // Wait for the player to start the quest
        CrystallizeEventManager.UI.OnUIInteraction += HandleQuestStateChanged;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.UI.OnUIInteraction -= HandleQuestStateChanged;

        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.Environment.OnActorApproached -= HandleActorApproached;

        yield return new WaitForSeconds(0.5f);

        TutorialState lastState = TutorialState.Disengaged;

        SetMessage("Drag words to the dictionary to look them up.");
        while (true) {
            var state = GetState();
            if (state == TutorialState.Complete) {
                break;
            }

            if (state != lastState) {
                SetState(state);
                lastState = state;
            }

            yield return null;
        }
        TutorialCanvas.main.ClearAllIndicators();
        ClearMessages();

        yield return null;

        CrystallizeEventManager.UI.OnSpeechBubbleOpen -= HandleSpeechBubbleOpen;

        SetMessage("Complete the quest.");
	}

    void main_OnSpeechBubbleOpen(object sender, PhraseEventArgs e) {
        
    }

    void HandleQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
        var gid = questClient.GetWorldID();
        var qid = GameData.Instance.QuestData.GetQuestInfoFromWorldID(gid).QuestID;
        var qpd = PlayerData.Instance.QuestData.GetQuestInstance(qid);
        if (qpd.State == ObjectiveState.Complete) {
            ObjectiveManager.main.SetObjective(this, true);
        }
    }

    void HandleQuestStateChanged(object sender, System.EventArgs e) {
        if (e is QuestConfirmedEventArgs) {
            Continue();
        }
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        if (sender is DialogueGroup) {
            Continue();
        }
    }

    void HandleSpeechBubbleOpen(object sender, PhraseEventArgs e) {
        if (e.Phrase == null) {
            return;
        }

        Debug.Log(sender + "; ");
        var sbi = sender as GameObject;
        foreach (var w in sbi.GetComponent<SpeechBubbleUI>().phrase.PhraseElements) {
            if (w.WordID == wordID) {
                speechBubbleInstance = sender as GameObject;
            }
        }
    }

    TutorialState GetState() {
        if (!DialogueSystemManager.main.InteractionTarget) {
            return TutorialState.Disengaged;
        }

        var dict = TutorialCanvas.main.GetRegisteredGameObject("QuickDictionary").GetComponent<QuickDictionaryUI>();
        if (dict.Word != null) {
            return TutorialState.Complete;
        }

        if (!UISystem.main.PhraseDragHandler.IsDragging) {
            return TutorialState.AwaitingDrag;
        }

        if (UISystem.main.PhraseDragHandler.IsDragging) {
            return TutorialState.AwaitingDrop;
        }

        return TutorialState.Disengaged;
    }

    void SetState(TutorialState state) {
        TutorialCanvas.main.ClearAllIndicators();
        var dict = TutorialCanvas.main.GetRegisteredGameObject("QuickDictionary").GetComponent<QuickDictionaryUI>();
        switch (state) {
            case TutorialState.Disengaged:
                SetMessage("Approach the girls.");
                break;

            case TutorialState.AwaitingDrag:
                TutorialCanvas.main.CreateUIDragBox(speechBubbleInstance.GetComponent<SpeechBubbleUI>().GetWord(wordID), "Drag or right-click");
                break;

            case TutorialState.AwaitingDrop:
                TutorialCanvas.main.CreateUIDragBox(dict.GetComponent<RectTransform>(), "...to here");
                break;
        }
    }
	
}
