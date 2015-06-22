using UnityEngine;
using System.Collections;
using Crystallize;

public class QuestClient : MonoBehaviour {

    public int questID = 0;

    // Use this for initialization
    void Start() {
        if (PlayerManager.Instance) {
            LoadQuestData();
        }
        CrystallizeEventManager.main.OnLoad += HandleOnLoad;

        if (GetComponent<ConversationClient>()) {
            GetComponent<ConversationClient>().OnStateChanged += HandleOnStateChanged;
        }
    }

    void HandleOnLoad(object sender, System.EventArgs e) {
        LoadQuestData();
    }

    void LoadQuestData() {
        var a = GetComponent<InteractiveDialogActor>();
        if (a) {
            var questInstance = PlayerData.Instance.QuestData.GetQuestInstance(questID);
            if (questInstance != null) {
                switch (questInstance.State) {
                    case ObjectiveState.Active:
                    case ObjectiveState.Available:
                        a.HasBeenVisited = true;
                        break;

                    case ObjectiveState.Complete:
                        a.IsComplete = true;
                        a.HasBeenVisited = true;
                        break;
                }

                CrystallizeEventManager.UI.RaiseOnProgressEvent(this, System.EventArgs.Empty);
            }
            return;
        }

        if (!PlayerManager.Instance) {
            Debug.LogError("Player manager not present.");
            return;
        }
    }

    void HandleOnStateChanged(object sender, System.EventArgs e) {
        Debug.Log("Quest state changed.");
        var s = GetComponent<ConversationClient>().State;
        switch (s) {
            case ConversationClientState.Locked:
                PlayerData.Instance.QuestData.SetQuestState(questID, ObjectiveState.Hidden);
                break;

            case ConversationClientState.SeekingClient:
            case ConversationClientState.SeekingWords:
                PlayerData.Instance.QuestData.SetQuestState(questID, ObjectiveState.Available);
                break;

            case ConversationClientState.Available:
                PlayerData.Instance.QuestData.SetQuestState(questID, ObjectiveState.Available);
                PlayerData.Instance.QuestData.GetQuestInstance(questID).SetObjectiveState(0, true);
                break;

            case ConversationClientState.Completed:
                PlayerData.Instance.QuestData.SetQuestState(questID, ObjectiveState.Complete);
                PlayerData.Instance.QuestData.GetQuestInstance(questID).SetObjectiveState(1, true);
                break;
        }
    }

    public void CompleteQuest() {
        var info = QuestInfo.GetQuestInfo(questID);
        var state = PlayerData.Instance.QuestData.GetOrCreateQuestInstance(questID);
        for (int i = 0; i < info.Objectives.Count; i++) {
            state.SetObjectiveState(i, true);
        }
        state.State = ObjectiveState.Complete;
    }

}
