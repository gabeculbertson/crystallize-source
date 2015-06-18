using UnityEngine;
using System.Collections;

public class PersonAnimationController : MonoBehaviour {

	const string RunFlag = "Running";
	const float SpeedThreshold = 0.1f;

	Animator animator;

	Vector3 lastPosition;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		var thisPosition = transform.position;

		//Debug.Log (gameObject + "; " + thisPosition + "; " + lastPosition + "; " + Vector3.Distance(thisPosition, lastPosition));

		if (Vector3.Distance (thisPosition, lastPosition) > SpeedThreshold * Time.deltaTime) {
			animator.SetBool (RunFlag, true);
		} else {
			animator.SetBool (RunFlag, false);
		}
		lastPosition = thisPosition;
	}
}
