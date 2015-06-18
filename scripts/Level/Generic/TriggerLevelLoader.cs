using UnityEngine;
using System.Collections;

public class TriggerLevelLoader : MonoBehaviour {

	public string levelName = "";

	void OnTriggerEnter () {
		if (MenuSwapper.main) {
			MenuSwapper.main.SwapToSecondary();
		} else {
			Application.LoadLevel (levelName);
		}
	}

}
