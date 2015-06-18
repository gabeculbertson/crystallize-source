using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestData {

	public List<QuestInstanceData> QuestInstances { get; set; }
	public int ActiveQuest { get; set; }

	public QuestData(){
		QuestInstances = new List<QuestInstanceData>();
	}

	public QuestInstanceData GetQuestInstance(int questID){
		return (from q in QuestInstances where q.QuestID == questID select q).FirstOrDefault ();
	}

	public QuestInstanceData GetOrCreateQuestInstance(int questID){
		var q = GetQuestInstance (questID);
		if (q == null) {
			Debug.Log("Quest being created." + questID);
			q = new QuestInstanceData(questID, ObjectiveState.Hidden);
			QuestInstances.Add(q);
		}
		return q;
	}
	
	public void SetQuestState(int questID, ObjectiveState questState){
		var quest = GetQuestInstance (questID);
        var oldState = ObjectiveState.Hidden;
		if (quest == null) {
			quest = new QuestInstanceData (questID, questState);
			QuestInstances.Add (quest);
		} else {
            oldState = quest.State;
			quest.State = questState;
		}

        if (oldState != ObjectiveState.Active && questState == ObjectiveState.Active) {
            quest.GetQuestGameData().BeginQuest();
        }
	}

}
