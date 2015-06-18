using UnityEngine;
using System.Collections;

public class LevelStateData {

	public string LevelName { get; set; }
	public LevelState LevelState { get; set; }

	public LevelStateData(){
		LevelName = "";
		LevelState = LevelState.Hidden;
	}

	public LevelStateData(string levelName, LevelState levelState){
		LevelName = levelName;
		LevelState = levelState;
	}

}
