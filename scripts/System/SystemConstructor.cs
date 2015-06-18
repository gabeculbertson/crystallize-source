using UnityEngine;
using System.Collections;

public class SystemConstructor : MonoBehaviour {

    public GameObject mainCanvasPrefab;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}


	
}
