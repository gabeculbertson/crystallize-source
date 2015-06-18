using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestInfo {

	static QuestInfo nullInfo;
	static Dictionary<int, QuestInfo> questInfoDictionary = new Dictionary<int, QuestInfo> ();

	static QuestInfo(){
		nullInfo = new QuestInfo (-1, "Null", "Quest not found.", new List<QuestObjectiveInfo>());

		AddBasicConversationQuest (0, "Order a water", "You're feeling a bit thirsty. Figure out how to order some water.");
		AddBasicConversationQuest (1, "Order some sushi", "You really want to try some authentic sushi. Figure out how to order some.");
		AddBasicConversationQuest (2, "What are you eating?", "Someone's food looks really good. Figure out how to ask what it is.");
		AddBasicConversationQuest (3, "What are you drinking?", "Someone's drink looks really good. Figure out how to ask what it is.");
		AddFetchQuest (4, "I'm thirsty...", "Someone really wants a drink.", "Akiyoshi");
	}

	static void AddBasicConversationQuest(int id, string name, string description, params string[] objectives){
		var objs = new List<QuestObjectiveInfo> ();
		objs.Add (new QuestObjectiveInfo ("Find the necessary words"));
		objs.Add (new QuestObjectiveInfo ("Complete the conversation", ObjectiveLocationType.Self));
		foreach (var obj in objectives) {
			objs.Add(new QuestObjectiveInfo(obj));
		}
		questInfoDictionary.Add (id, new QuestInfo (id, name, description, objs)); 
	}

	static void AddFetchQuest(int id, string name, string description, string targetClient){
		var objs = new List<QuestObjectiveInfo> ();
		objs.Add (new QuestObjectiveInfo ("Find the item!", targetClient));
		objs.Add (new QuestObjectiveInfo ("Return to the client", ObjectiveLocationType.Self));
		questInfoDictionary.Add (id, new QuestInfo (id, name, description, objs)); 
	}

	public static QuestInfo GetQuestInfo(int id){
		if (questInfoDictionary.ContainsKey (id)) {
			return questInfoDictionary [id];
		} else {
			return nullInfo;
		}
	}

	public static IEnumerable<QuestInfo> GetAllQuestInfo(){
		return questInfoDictionary.Values;
	}

	public int ID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public List<QuestObjectiveInfo> Objectives { get; set; }

	public QuestInfo(int id, string name, string description, List<QuestObjectiveInfo> objectives){
		ID = id;
		Name = name;
		Description = description;
		Objectives = objectives;
	}

}
