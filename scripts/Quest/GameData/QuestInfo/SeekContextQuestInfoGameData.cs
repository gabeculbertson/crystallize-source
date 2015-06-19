using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeekContextQuestInfoGameData : QuestInfoGameData {

	public string ContextLabel { get; set; }
	public int TotalCount { get; set; }

	public SeekContextQuestInfoGameData() : base(){	}

	public SeekContextQuestInfoGameData (int questID, int clientID) : base(questID, clientID){
		ContextLabel = "NULL";
		Description = "Fetch the item.";
	}

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var o = new List<QuestObjectiveInfoGameData>();
        o.Add(new QuestObjectiveInfoGameData("Seek the information", TotalCount));
        o.Add(new QuestObjectiveInfoGameData("Return to the client."));
        o[1].LocationType = ObjectiveLocationType.Client;

        return o;
    }

	public override void ProcessMessage (System.EventArgs args)
	{
		if (args is ContextDataExpressedEventArgs) {
			var targs = (ContextDataExpressedEventArgs)args;
			var qi = PlayerManager.main.playerData.QuestData.GetQuestInstance (QuestID);
			if (targs.ContextItemLabel == ContextLabel) {
				if (!qi.GetObjectiveState (0).IsComplete) {
					Debug.Log ("Count for " + ContextLabel + " increased.");
					qi.SetObjectiveState (0, TotalCount, qi.GetObjectiveState (0).CurrentCount + 1);
					EffectManager.main.PlayMessage (ContextLabel + " learned!", Color.cyan);
				}
			}
			CrystallizeEventManager.PlayerState.RaiseQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.main.PlayerID, qi));
		} else if (args is PersonApproachedEventArgs) {
			var targs = (PersonApproachedEventArgs)args;
			if(targs.WorldID != WorldID){
				return;
			}

			var qi = PlayerManager.main.playerData.QuestData.GetQuestInstance (QuestID);
			if(!qi.GetObjectiveState(0).IsComplete){
				return;
			}

			qi.SetObjectiveState(1, true);
            CompleteQuest();
		}
	}

}
