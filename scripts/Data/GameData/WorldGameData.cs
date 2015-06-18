using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGameData  {

	public int CurrentWorldObjectID { get; set; }
	public List<int> WorldObjects { get; set; }
	public int AreaID { get; set; }
	public Vector3 Position { get; set; }

	public WorldGameData(){
		CurrentWorldObjectID = 1000000;
		WorldObjects = new List<int> ();
	}

	 public int GetNewID(){
		var objID = CurrentWorldObjectID;
		CurrentWorldObjectID += 10;
		return objID;
	}

}
