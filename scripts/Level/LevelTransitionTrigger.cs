using UnityEngine;
using System.Collections;

public class LevelTransitionTrigger : MonoBehaviour {

	public GameObject promptPrefab;
	public int targetLevelID = -1;
	public string levelString = "";
	public string targetLevel = "";

	GameObject promptInstance;

	void OnTriggerEnter (Collider other) {
		if (!other.attachedRigidbody) {
			return;
		}

		if (other.attachedRigidbody.tag != "Player") {
			return;
		}

		if(!promptInstance){
			var instance = Instantiate(promptPrefab) as GameObject;
			instance.GetComponent<LevelTransitionPromptUI>().Initiallize(targetLevel, levelString);
			promptInstance = instance;
		}
	}

	void OnTriggerExit (Collider other) {
		if (!other.attachedRigidbody) {
			return;
		}
		
		if (other.attachedRigidbody.tag != "Player") {
			return;
		}

		if(promptInstance){
			Destroy (promptInstance);
			promptInstance = null;
		}
	}

}
