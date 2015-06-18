using UnityEngine;
using System.Collections;

public class QuestConfirmationUIRequestEventArgs : UIRequestEventArgs {

	public QuestInfoGameData Quest { get; set; }

	public QuestConfirmationUIRequestEventArgs(GameObject parent, QuestInfoGameData quest) : base(parent){
		Quest = quest;
	}

}
