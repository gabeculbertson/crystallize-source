using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActiveQuestObjectiveUI : MonoBehaviour {

	public GameObject checkInstance;
	public Text descriptionText;

	QuestInstanceData questState;
	int id;

	public void Initialize(QuestInstanceData questState, int id){
		this.questState = questState;
		this.id = id;
		//Debug.Log (QuestInfo.GetQuestInfo (questState.QuestID).Objectives.Count);
		descriptionText.text = GameData.Instance.QuestData.Quests.GetItem (questState.QuestID).GetObjectives()[id].Description;
			//QuestInfo.GetQuestInfo (questState.QuestID).Objectives [id].Description;
	}
	
	// Update is called once per frame
	void Update () {
		var qo = questState.GetObjectiveState(id);
		if (qo.TotalCount > 1) {
			descriptionText.text = GameData.Instance.QuestData.Quests.GetItem (questState.QuestID).GetObjectives() [id].Description +
				string.Format (" ({0}/{1})", qo.CurrentCount, qo.TotalCount);
		}
		checkInstance.SetActive(questState.GetObjectiveState(id).IsComplete);
	}
}
