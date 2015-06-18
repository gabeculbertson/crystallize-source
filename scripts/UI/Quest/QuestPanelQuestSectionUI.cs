using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class QuestPanelQuestSectionUI : UIMonoBehaviour, IPointerClickHandler {


	public event EventHandler OnClicked;

	public QuestInfoGameData quest;
	public GameObject checkmarkInstance;

	public void Initialize(QuestInfoGameData quest, bool complete){
		GetComponentInChildren<Text> ().text = quest.Title;
		this.quest = quest;
		checkmarkInstance.SetActive (complete);
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (OnClicked != null) {
			OnClicked(this, EventArgs.Empty);
		}
	}
	
}
