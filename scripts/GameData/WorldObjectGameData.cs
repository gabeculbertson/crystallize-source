using UnityEngine;
using System.Collections;

public class WorldObjectGameData {

	public int ID { get; set; }

	public WorldObjectGameData(){
		ID = -1;
	}

	public WorldObjectGameData(int id){
		ID = id;
	}

}
