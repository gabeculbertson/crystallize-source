using UnityEngine;
using System.Collections;

public class CutsceneLevel : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Application.LoadLevel(LevelSettings.main.nextLevel);
		}
	}
}
