using UnityEngine;
using System;
using System.Collections;
using Crystallize;

namespace Crystallize.Animation {
	public class AnimationPhaseScript : MonoBehaviour {

		public void EnqueueLateUpdate(Action action){
			StartCoroutine (RunInLateUpdate (action));
		}

		IEnumerator RunInLateUpdate(Action action){
			yield return new WaitForEndOfFrame ();
			action ();
		}

	}
}