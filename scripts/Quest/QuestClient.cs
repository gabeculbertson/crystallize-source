using UnityEngine;
using System.Collections;
using Crystallize;

public class QuestClient : MonoBehaviour {

	public int questID = 0;

	// Use this for initialization
	void Start () {
		if (PlayerManager.main) {
			LoadQuestData();
		}
        CrystallizeEventManager.main.OnLoad += HandleOnLoad;

		if (GetComponent<ConversationClient> ()) {
			GetComponent<ConversationClient> ().OnStateChanged += HandleOnStateChanged;
		}
	}

	void HandleOnLoad (object sender, System.EventArgs e)
	{
		LoadQuestData ();
	}

	void LoadQuestData(){
		var a = GetComponent<InteractiveDialogActor> ();
		if (a) {
			var questInstance = PlayerManager.main.playerData.QuestData.GetQuestInstance (questID);
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
				
				CrystallizeEventManager.UI.RaiseOnProgressEvent (this, System.EventArgs.Empty);
			}
			return;
		}

		if (!PlayerManager.main) {
			Debug.LogError("Player manager not present.");
			return;
		}
	}

	void HandleOnStateChanged (object sender, System.EventArgs e)
	{
		Debug.Log ("Quest state changed.");
		var s = GetComponent<ConversationClient> ().State;
		switch (s) {
		case ConversationClientState.Locked:
			PlayerManager.main.playerData.QuestData.SetQuestState(questID, ObjectiveState.Hidden);
			break;

		case ConversationClientState.SeekingClient:
		case ConversationClientState.SeekingWords:
			PlayerManager.main.playerData.QuestData.SetQuestState(questID, ObjectiveState.Available);
			break;

		case ConversationClientState.Available:
			PlayerManager.main.playerData.QuestData.SetQuestState(questID, ObjectiveState.Available);
			PlayerManager.main.playerData.QuestData.GetQuestInstance(questID).SetObjectiveState(0, true);
			break;

		case ConversationClientState.Completed:
			PlayerManager.main.playerData.QuestData.SetQuestState(questID, ObjectiveState.Complete);
			PlayerManager.main.playerData.QuestData.GetQuestInstance(questID).SetObjectiveState(1, true);
			break;
		}
	}

	public void CompleteQuest(){
		var info = QuestInfo.GetQuestInfo (questID);
		var state = PlayerManager.main.playerData.QuestData.GetOrCreateQuestInstance (questID);
		for (int i = 0; i < info.Objectives.Count; i++) {
			state.SetObjectiveState(i, true);
		}
		state.State = ObjectiveState.Complete;
	}

}
