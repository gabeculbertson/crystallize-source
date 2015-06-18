using UnityEngine;
using System.Collections;

public class QuestConfirmedEventArgs : System.EventArgs {

	public int QuestID { get; set; }

	public QuestConfirmedEventArgs(int questID){
		QuestID = questID;
	}

}
