using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class PlayerController : MonoBehaviour {

	public static PlayerController main { get; set; }

	public static void LockMovement(object lockObject){
		if (main) {
			if(!main.locks.Contains(lockObject)){
				main.locks.Add (lockObject);
			}
		}
	}

	public static void UnlockMovement(object lockObject){
		if (main) {
			if(main.locks.Contains(lockObject)){
				main.locks.Remove (lockObject);
			}
		}
	}

	public float speed = 5f;
	public Transform target = null;

	HashSet<object> locks = new HashSet<object>();

	public bool MovementLocked {
		get {
			return locks.Count > 0;
		}
	}

	void Awake (){
		main = this;
	}

	// Use this for initialization
	void Start () {
		if (!target) {
			var p = PlayerManager.main.PlayerGameObject;

			if(!p){
				return;
			}

			target = p.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (MovementLocked) {
			return;
		}

		if (!target) {
			return;
		}

		target.GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (Input.GetMouseButton (0)) {
            if (!UISystem.MouseOverUI()) {
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				var hit = new RaycastHit();
				if(Physics.Raycast(ray, out hit)){
					var dir = hit.point - target.transform.position;
					dir.y = 0;
					target.GetComponent<Rigidbody>().velocity = dir.normalized * speed;
				}
			} 
		} else {
			var camForward = Camera.main.transform.forward;
			camForward.y = 0;
			var rot = Quaternion.LookRotation (camForward, Vector3.up);

			var xForce = Input.GetAxis ("Horizontal");
			var yForce = Input.GetAxis ("Vertical");
			var inputDirection = new Vector3 (xForce, 0, yForce);

			target.GetComponent<Rigidbody>().velocity = rot * inputDirection * speed;
		}
	}
	
}
