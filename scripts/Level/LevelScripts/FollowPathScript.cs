using UnityEngine;
using System.Collections;

public class FollowPathScript : MonoBehaviour {

	public Transform agent;
	public Transform[] targets = new Transform[0];
	public bool destroyOnArrive = false;
	public float speed = 5f;
	public Animator animator;

	int currentTarget = 0;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (animator) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Run")) {
				animator.Play ("Run");
				animator.SetBool ("Running", true);
			}
		}

		agent.LookAt (targets[currentTarget]);
		if (agent.GetComponent<Rigidbody>()) {
			agent.position = Vector3.MoveTowards (transform.position, targets[currentTarget].position, speed * Time.deltaTime);
			agent.GetComponent<Rigidbody>().isKinematic = false;
			agent.GetComponent<Rigidbody>().MovePosition(transform.position);
			//Debug.Log(transform.rigidbody.position);
		} else {
			transform.position = Vector3.MoveTowards (transform.position, targets[currentTarget].position, speed * Time.deltaTime);
		}

		if (Vector3.Distance (transform.position, targets[currentTarget].position) < 0.1f) {
			currentTarget++;

			if(currentTarget >= targets.Length){
				if (animator) {
					Debug.Log("Enabling run.");
					animator.SetBool ("Running", false);
				}

				agent.transform.position = targets[targets.Length - 1].position;
				agent.up = Vector3.up;

				enabled = false;
				if(agent.GetComponent<Rigidbody>()){
					agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
					agent.GetComponent<Rigidbody>().isKinematic = true;
				}

				if(destroyOnArrive){
					Destroy(agent.gameObject);
				}
			}
		}
	}
}
