using UnityEngine;
using System.Collections;

public class QuestObjectiveInfo {

	public string Description { get; set; }
	public ObjectiveLocationType LocationType { get; set; }
	public string ClientName { get; set; }

	public QuestObjectiveInfo (string description){
		Description = description;
		LocationType = ObjectiveLocationType.None;
	}

	public QuestObjectiveInfo (string description, ObjectiveLocationType locationType){
		Description = description;
		LocationType = locationType;
	}

	public QuestObjectiveInfo (string description, string clientName){
		Description = description;
		LocationType = ObjectiveLocationType.Client;
		ClientName = clientName;
	}

}
