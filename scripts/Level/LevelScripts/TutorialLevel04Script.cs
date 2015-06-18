using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crystallize;

public class TutorialLevel04Script : LevelScript {

	// Use this for initialization
	IEnumerator Start () {
		while (!LevelSystemConstructor.main) {
			yield return null;
		}

		ReviewManager.main.AddSimulatedTime (0.1f);
	}

}
