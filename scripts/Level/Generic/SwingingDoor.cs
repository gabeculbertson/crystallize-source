using UnityEngine;
using System.Collections;

public class SwingingDoor : MonoBehaviour {

	public float openRotation = 115f;
	public bool isOpen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isOpen) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, openRotation, 0), 300f * Time.deltaTime);
		} else {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.identity, 300f * Time.deltaTime);
		}
	}
}
