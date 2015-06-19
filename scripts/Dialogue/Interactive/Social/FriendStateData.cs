using UnityEngine;
using System.Collections;

public class FriendStateData {

	public string ID { get; set; }
	public int FriendLevel { get; set; }

	public FriendStateData(){
		ID = "";
		FriendLevel = 0;
	}

	public FriendStateData(string id, int level){
		ID = id;
		FriendLevel = level;
	}

}
