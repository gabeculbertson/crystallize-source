using UnityEngine;
using System.Collections;

public class QuestObjectiveInfoGameData {

	public string Description { get; set; }
	public ObjectiveLocationType LocationType { get; set; }
	public int TotalCount { get; set; }

	public QuestObjectiveInfoGameData(){
		Description = "EMPTY";
		LocationType = ObjectiveLocationType.None;
		TotalCount = 1;
	}

	public QuestObjectiveInfoGameData(string desc) : this(){
		Description = desc;
	}

	public QuestObjectiveInfoGameData(string desc, int totalCount) : this(desc){
		TotalCount = totalCount;
	}

}
