using UnityEngine;
using System.Collections;

public class QuestEventArgs : System.EventArgs {

	public int QuestID { get; set; }


	public QuestEventArgs (int questID){
		QuestID = questID;
	}

}
