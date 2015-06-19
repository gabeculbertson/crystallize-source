using UnityEngine;
using System.Collections;

public class WorldCanvas : MonoBehaviour {

	public static WorldCanvas main { get; set; }

	// Use this for initialization
	void Awake () {
		main = this;
	}

}
