using UnityEngine;
using System.Collections;

public class BranchedDialogueHolder : ISerializableDictionaryItem<int> {

	public int Key {
		get {
			return WorldObjectID;
		}
	}
	
	public int WorldObjectID { get; set; }
	public int BranchedDialogueID { get; set; }

	public BranchedDialogueHolder(){
		WorldObjectID = -1;
		BranchedDialogueID = -1;
	}

	public BranchedDialogueHolder(int worldID, int branchID){
		WorldObjectID = worldID;
		BranchedDialogueID = branchID;
	}

}
