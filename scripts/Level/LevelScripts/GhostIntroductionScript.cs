using UnityEngine;
using System.Collections;

public class GhostIntroductionScript : MonoBehaviour {

	public Transform cameraTarget1;
	public Transform cameraTarget2;
	public Transform cameraTarget3;
	public Transform player;
	public Transform ghost;
	public Transform ghostTarget;
	public RectTransform blackScreen;
	public Light[] lights;
	public PhraseSegmentData phrase;

	// Use this for initialization
	IEnumerator Start () {
		Camera.main.GetComponent<OmniscientCamera> ().enabled = false;
		SetCameraTarget (cameraTarget1);

		StartCoroutine (FadeScreenIn ());

		var walk = player.GetComponent<WalkAwayScript> ();
		while (walk.enabled) {
			yield return null;
		}

		player.GetComponent<PlayerDialogActor> ().SetPhrase (phrase);

		yield return StartCoroutine(ZoomCamera (cameraTarget1, cameraTarget2));

		ghost.GetComponent<WalkAwayScript> ().enabled = true;
		ghost.GetComponentInChildren<Animator> ().CrossFade ("Stand", 10f);

		yield return new WaitForSeconds (3f);

		yield return StartCoroutine (Flicker (5));

		blackScreen.GetComponent<CanvasGroup> ().alpha = 1f;
		blackScreen.gameObject.SetActive (true);

		SetCameraTarget (cameraTarget3);
		ghost.GetComponent<WalkAwayScript> ().enabled = false;
		ghost.position = ghostTarget.position;
		ghost.rotation = ghostTarget.rotation;
		ghost.GetComponentInChildren<Animator> ().Play ("CastMagic");

		yield return new WaitForSeconds (0.25f);

		blackScreen.gameObject.SetActive (false);
		player.GetComponentInChildren<Animator> ().Play ("Collapse");

		yield return new WaitForSeconds (1f);

		blackScreen.gameObject.SetActive (true);
		yield return StartCoroutine (FadeScreenOut (5f));
		//blackScreen.gameObject.SetActive (false);

		Application.LoadLevel (LevelSettings.main.nextLevel);
	}

	IEnumerator Flicker(int times){
		for (int i = 0; i < times; i++) {
			foreach (var light in lights) {
				light.enabled = false;
			}
			
			yield return new WaitForSeconds (Random.Range(0.05f, 0.3f));
			
			foreach (var light in lights) {
				light.enabled = true;
			}

			yield return new WaitForSeconds (Random.Range(0.25f, 0.45f));
		}
	}

	IEnumerator FadeScreenIn(){
		for (float t = 0; t < 1f; t += Time.deltaTime) {
			blackScreen.GetComponent<CanvasGroup>().alpha = 1f - t;
			
			yield return null;
		}
	}

	IEnumerator FadeScreenOut(float time){
		blackScreen.GetComponent<CanvasGroup> ().alpha = 0;
		for (float t = 0; t < 1f; t += Time.deltaTime / time) {
			blackScreen.GetComponent<CanvasGroup>().alpha = t;
			
			yield return null;
		}
	}

	IEnumerator ZoomCamera(Transform t1, Transform t2){
		for (float t = 0; t < 1f; t += Time.deltaTime) {
			LerpCameraTarget(t, t1, t2);

			yield return null;
		}
		SetCameraTarget (t2);
	}

	void SetCameraTarget(Transform target){
		Camera.main.transform.position = target.position;
		Camera.main.transform.rotation = target.rotation;
	}

	void LerpCameraTarget(float t, Transform t1, Transform t2){
		Camera.main.transform.position = Vector3.Lerp (t1.position, t2.position, t);
		Camera.main.transform.rotation = Quaternion.Lerp(t1.rotation, t2.rotation, t);
	}

}
