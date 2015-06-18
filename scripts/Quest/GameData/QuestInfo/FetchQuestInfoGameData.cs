using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FetchQuestInfoGameData : QuestInfoGameData {

	public string RequiredItem { get; set; }

	public FetchQuestInfoGameData() : base(){	}

	public FetchQuestInfoGameData (int questID, int clientID) : base(questID, clientID){
		RequiredItem = "NULL";
		Description = "Fetch the item.";
	}

	public override List<QuestObjectiveInfoGameData> GetDefaultObjectives(){
		var o = new List<QuestObjectiveInfoGameData>();
		o.Add (new QuestObjectiveInfoGameData("Find the item"));
		o.Add (new QuestObjectiveInfoGameData("Return to the client."));

		return o;
	}

	public override void ProcessMessage (System.EventArgs args)
	{

	}

}
