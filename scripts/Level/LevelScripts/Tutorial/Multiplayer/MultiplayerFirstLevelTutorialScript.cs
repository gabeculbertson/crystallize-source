using UnityEngine;
using System.Collections;

public class MultiplayerFirstLevelTutorialScript : LevelScript {

    enum CommunicationState {
        None,
        AwaitPartnerAccept,
        OutOfRange,
        EmoticonPanelClosed,
        EmoticonPanelOpen,
        WaitingForOther,
        Complete
    }

    public Transform questClient;
    public GameObject movementTutorialPrefab;
    public Transform movementTrigger;

    GameObject movementTutorialInstance;

    bool partnerJoined = false;

    // Use this for initialization
    IEnumerator Start() {
        ObjectiveManager.main.SetObjective(this, false);
        StartCoroutine(WaitAndActivateQuest());
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;

        yield return null;

        SetMessage("Wait for your partner to join the game.");
        while (!partnerJoined) {
            yield return null;
        }

        // Teach the player to move
        SetMessage("Move with WASD.");
        movementTutorialInstance = Instantiate<GameObject>(movementTutorialPrefab);
        TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.GetRegisteredGameObject("HelpBox").GetComponent<RectTransform>(), "Help is shown here");

        movementTrigger.GetComponent<TriggerEventObject>().OnTriggerExitEvent += HandleTriggerExit;

        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.Environment.OnActorApproached -= HandleActorApproached;

        SetMessage("Accept the quest.");

        yield return null;

        // Wait for the player to start the quest
        CrystallizeEventManager.UI.OnUIInteraction += HandleQuestStateChanged;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.UI.OnUIInteraction -= HandleQuestStateChanged;

        // Get player into range and have them click emoticon
        var lastState = CommunicationState.None;
        while (true) {
            var newState = GetState();
            if (newState == CommunicationState.Complete) {
                break;
            }

            if (newState != lastState) {
                SetState(newState);
                lastState = newState;
            }

            yield return null;
        }

        TutorialCanvas.main.ClearAllIndicators();
    }

    IEnumerator WaitAndActivateQuest() {
        partnerJoined = false;
        questClient.gameObject.SetActive(false);
        while (PlayerManager.main.PlayerCount < 2) {
            yield return null;
        }
        questClient.gameObject.SetActive(true);
        partnerJoined = true;
    }

    void HandleTriggerExit(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            Continue();
            Destroy(movementTutorialInstance);
            movementTrigger.GetComponent<TriggerEventObject>().OnTriggerExitEvent -= HandleTriggerExit;
            SetMessage("Approach the teacher.");
        }
    }

    //TODO: make this more robust
    void HandleQuestStateChanged(object sender, System.EventArgs e) {
        if (e is QuestConfirmedEventArgs) {
            Continue();
        }

        if (GetQuestInstance(questClient).State == ObjectiveState.Complete) {
            ObjectiveManager.main.SetObjective(this, true);
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

    CommunicationState GetState() {
        var gid = questClient.GetWorldID();
        var qid = GameData.Instance.QuestData.GetQuestInfoFromWorldID(gid).QuestID;
        var qpd = PlayerManager.main.playerData.QuestData.GetQuestInstance(qid);
        if (qpd.State == ObjectiveState.Complete) {
            return CommunicationState.Complete;
        }

        if (qpd.GetObjectiveState(1).IsComplete) {
            return CommunicationState.WaitingForOther;
        }

        if (!qpd.GetObjectiveState(0).IsComplete) {
            return CommunicationState.AwaitPartnerAccept;
        }

        if (!InteractionManager.IsInteractingWithOtherPlayer()) {
            return CommunicationState.OutOfRange;
        }

        var ep = TutorialCanvas.main.GetRegisteredGameObject("EmoticonPanel");
        if (!ep.activeSelf) {
            return CommunicationState.EmoticonPanelClosed;
        }

        return CommunicationState.EmoticonPanelOpen;
    }

    void SetState(CommunicationState state) {
        TutorialCanvas.main.ClearAllIndicators();
        ClearMessages();
        switch (state) {
            case CommunicationState.AwaitPartnerAccept:
                SetMessage("Wait for your partner to accept the quest");
                break;

            case CommunicationState.OutOfRange:
                SetMessage("Move closer to your partner (the one with the blue circle)\n"
                           + "Arrows will appear when you are close enough.");
                break;

            case CommunicationState.EmoticonPanelClosed:
                SetMessage("Click the emoticon button.");
                var eb = TutorialCanvas.main.GetRegisteredGameObject("EmoticonButton").GetComponent<RectTransform>();
                TutorialCanvas.main.CreateUIDragBox(eb, "Click here!");
                break;

            case CommunicationState.EmoticonPanelOpen:
                SetMessage("Click the emote button.");
                var wb = TutorialCanvas.main.GetRegisteredGameObject("WaveButton").GetComponent<RectTransform>();
                TutorialCanvas.main.CreateUIDragBox(wb, "Click here!");
                break;

            case CommunicationState.WaitingForOther:
                SetMessage("Now wait for your partner to wave back...");
                break;
        }
    }

}
