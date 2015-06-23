using UnityEngine;
using System.Collections;

public class DebugTimeProcess : MonoBehaviour {
	// Use this for initialization
	void Start () {
		// construct a job reference
		var myJob = new JobRef (10);

		var p = GameTimeProcess.GetTestInstance ();
		p.MorningSessionCallback (null, myJob);
	}

}
