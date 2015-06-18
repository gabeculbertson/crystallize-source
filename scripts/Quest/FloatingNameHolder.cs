using UnityEngine;
using System.Collections;
using Crystallize;

public class FloatingNameHolder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FloatingNameUI.main.SetName (this, "???");
	}

}
