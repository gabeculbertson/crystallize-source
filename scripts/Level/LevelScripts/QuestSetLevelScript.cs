using UnityEngine;
using System.Collections;

public class QuestSetLevelScript : LevelScript {

	public bool multiplayerOnly = true;
	public Transform[] clients = new Transform[0];

	// Use this for initialization
	void Start () {
        PlayerData.Instance.Flags.SetFlag(FlagPlayerData.DictionaryUnlocked, true);
        PlayerData.Instance.Flags.SetFlag(FlagPlayerData.ClickWordSlotMessage, true);
		HandleFlagChanged (null, null);
		CrystallizeEventManager.PlayerState.OnFlagChanged += HandleFlagChanged;
	}

	void HandleFlagChanged (object sender, TextEventArgs e)
	{
		if (multiplayerOnly) {
			if(PlayerData.Instance.Flags.GetFlag(FlagPlayerData.IsMultiplayer)){
				ObjectiveManager.main.SetObjective (this, false);
				CrystallizeEventManager.PlayerState.OnQuestStateChanged -= HandleQuestStateChanged;
				CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;
			}
		}
	}

	void HandleQuestStateChanged (object sender, QuestStateChangedEventArgs e)
	{
		if (AllQuestsComplete ()) {
			ObjectiveManager.main.SetObjective(this, true);
		}
	}

	bool AllQuestsComplete(){
		foreach (var client in clients) {
			var qi = GetQuestInstance(client);
			if(qi == null){
				continue;
			}

			if(qi.State != ObjectiveState.Complete){
				return false;
			}
		}
		return true;
	}

}
