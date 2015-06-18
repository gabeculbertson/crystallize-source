using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragDropFreeInputHistoryPanelUI : MonoBehaviour {

    public DragDropFreeInputUI inputUI;
    public GameObject buttonPrefab;

    List<GameObject> buttonInstances = new List<GameObject>();

	// Use this for initialization
	void Start () {
	    
	}
	
	void UpdateButtons () {
	    foreach(var b in buttonInstances){
            Destroy(b);
        }
        buttonInstances.Clear();
	}
}
