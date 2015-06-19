using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkResources : MonoBehaviour {

    public List<GameObject> prefabs = new List<GameObject>();

	// Use this for initialization
	void Start () {
        foreach (var prefab in prefabs) {
            Instantiate(prefab);
        }
	}

}
