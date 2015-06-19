using UnityEngine;
using System.Collections;

public class PlayerSpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var spawns = GameObject.FindGameObjectsWithTag("PlayerOrigin");
        if (spawns.Length == 1) {
            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.left;
        } else {
            Destroy(gameObject);
        }
	}
	
}
