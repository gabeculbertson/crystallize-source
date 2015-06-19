using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuestInstanceObjectiveData {

	public bool IsComplete { 
		get{
			return CurrentCount >= TotalCount;
		}
	}

	public int CurrentCount { get; set; }
	public int TotalCount { get; set; }

	public QuestInstanceObjectiveData(){
		CurrentCount = 0;
		TotalCount = 1;
	}

	public QuestInstanceObjectiveData(int totalCount) : this(){
		TotalCount = totalCount;
	}

}
