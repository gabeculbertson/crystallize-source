using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorTrigger : MonoBehaviour {

	HashSet<Collider> containedColliders = new HashSet<Collider>();

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if (enabled) {
			containedColliders.Add(other);
			Open();
		}
	}

	void OnTriggerExit(Collider other){
		if (enabled) {
			containedColliders.Remove(other);
			if(containedColliders.Count == 0){
				Close();
			}
		}
	}

	public void Open(){
		foreach (var door in GetComponentsInChildren<SwingingDoor>()) {
			door.isOpen = true;
		}
	}

	public void Close(){
		foreach (var door in GetComponentsInChildren<SwingingDoor>()) {
			door.isOpen = false;
		}
	}

}
