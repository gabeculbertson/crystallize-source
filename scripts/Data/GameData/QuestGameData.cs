using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestGameData {

	int currentClientID;

	public SerializableDictionary<int, QuestClientGameData> Clients { get; set; }
	public UniqueKeySerializableDictionary<QuestInfoGameData> Quests { get; set; }

	public QuestGameData(){
		currentClientID = -1;
		Clients = new SerializableDictionary<int, QuestClientGameData> ();
		Quests = new UniqueKeySerializableDictionary<QuestInfoGameData> ();
//		Quests.UpdateItem (new QuestInfoGameData (-100, -100));
//		Quests.UpdateItem (new FetchQuestInfoGameData (-101, -100));
	}

	public int GetNextClientID(){
		IncrementClientID ();
		return currentClientID;
	}

	public void IncrementClientID(){
		currentClientID++;
		while (Clients.GetItem(currentClientID) != null){
			currentClientID++;
		}
	}

	public QuestClientGameData GetClientFromName(string name){
		return (from c in Clients.Items where c.Name.Trim().ToLower() == name.Trim().ToLower() select c).FirstOrDefault ();
	}

	public QuestInfoGameData GetQuestInfoFromWorldID(int worldID){
		return (from q in Quests.Items where q.WorldID == worldID select q).FirstOrDefault ();
	}

	public QuestInfoGameData AddQuest(QuestInfoGameData questInfo){
		questInfo.QuestID = Quests.GetNextKey ();
		Quests.AddItem (questInfo);
		return questInfo;
	}

}
