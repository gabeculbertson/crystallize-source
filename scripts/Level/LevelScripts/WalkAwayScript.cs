using UnityEngine;
using System.Collections;

public class WalkAwayScript : MonoBehaviour {

	//public Transform agent;
	public Transform target;
	public bool destroyOnArrive = false;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (target);
		if (transform.GetComponent<Rigidbody>()) {
			transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
			transform.GetComponent<Rigidbody>().isKinematic = false;
			transform.GetComponent<Rigidbody>().MovePosition(transform.position);
		} else {
			transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
		}

		if (Vector3.Distance (transform.position, target.position) < 0.1f) {
			enabled = false;
			if(destroyOnArrive){
				Destroy(gameObject);
			}
		}
	}
}
