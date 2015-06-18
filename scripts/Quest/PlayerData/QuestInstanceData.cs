using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuestInstanceData {

	public int QuestID { get; set; }
	public ObjectiveState State { get; set; }
	public List<QuestInstanceObjectiveData> ObjectiveStates { get; set; }

	public QuestInstanceData(){
		QuestID = -1;
		State = ObjectiveState.Hidden;
		ObjectiveStates = new List<QuestInstanceObjectiveData> ();
	}

	public QuestInstanceData (int questID, ObjectiveState state){
		QuestID = questID;
		State = state;
		ObjectiveStates = new List<QuestInstanceObjectiveData>(QuestInfo.GetQuestInfo(questID).Objectives.Count);
	}

	public QuestInstanceObjectiveData GetObjectiveState(int index){
		if (index < 0 || index >= ObjectiveStates.Count) {
			return new QuestInstanceObjectiveData();
		}

		return ObjectiveStates[index];
	}

	public void SetObjectiveState(int index, bool value){
		while (ObjectiveStates.Count <= index) {
			ObjectiveStates.Add(new QuestInstanceObjectiveData());
		}
        if (value) {

            ObjectiveStates[index].CurrentCount = ObjectiveStates[index].TotalCount;
        } else {
            ObjectiveStates[index].CurrentCount = 0;
        }
	}

	public void SetObjectiveState(int index, int total, int value){
		while (ObjectiveStates.Count <= index) {
			ObjectiveStates.Add(new QuestInstanceObjectiveData(total));
		}
		ObjectiveStates [index].TotalCount = total;
		ObjectiveStates [index].CurrentCount = value;
	}

	public int GetCurrentObjective(){
		if (ObjectiveStates.Count == 0) {
			return 0;
		}

		int count = GameData.Instance.QuestData.Quests.GetItem (QuestID).GetObjectives().Count;
		for(int i = 0; i < count; i++){
			if(!GetObjectiveState(i).IsComplete){
				return i;
			}
		}

		return -1;
	}

	public QuestInfoGameData GetQuestGameData(){
		return GameData.Instance.QuestData.Quests.GetItem (QuestID);
	}

}
