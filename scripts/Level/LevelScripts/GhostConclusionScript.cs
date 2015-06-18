using UnityEngine;
using System.Collections;

public class GhostConclusionScript : MonoBehaviour {

	public Transform ghost;
	public Transform player;
	public Transform cow;
	public Transform ghostAnger;
	public Transform blackScreen;
	public PhraseSegmentData phrase;
	//public string nextLevel;

	bool lookAtCow = true;

	// Use this for initialization
	IEnumerator Start () {
		StartCoroutine(FadeScreenIn ());

		ghostAnger.gameObject.SetActive (false);
		ghost.position -= Vector3.up * 4f;
		var ghostPos = ghost.position;

		yield return new WaitForSeconds (5f);

		lookAtCow = false;
		var lookTarget = ghost.position;
		lookTarget.y = 0;
		player.LookAt (lookTarget);
		player.GetComponent<PlayerDialogActor> ().SetPhrase (phrase);

		for (float h = 0; h < 4f; h += Time.deltaTime) {
			ghost.transform.position = ghostPos + Vector3.up * h;

			yield return null;
		}

		ghostAnger.gameObject.SetActive (true);

		yield return new WaitForSeconds (1f);

		yield return StartCoroutine(FadeScreenOut (3f));

		Application.LoadLevel (LevelSettings.main.nextLevel);
	}

	IEnumerator FadeScreenIn(){
		blackScreen.gameObject.SetActive (true);
		for (float t = 0; t < 1f; t += Time.deltaTime) {
			blackScreen.GetComponent<CanvasGroup>().alpha = 1f - t;
			
			yield return null;
		}
	}
	
	IEnumerator FadeScreenOut(float time){
		blackScreen.gameObject.SetActive (true);
		blackScreen.GetComponent<CanvasGroup> ().alpha = 0;
		for (float t = 0; t < 1f; t += Time.deltaTime / time) {
			blackScreen.GetComponent<CanvasGroup>().alpha = t;
			
			yield return null;
		}
	}

	void Update(){
		if (lookAtCow) {
			player.LookAt(cow.position);
		}
	}

}
