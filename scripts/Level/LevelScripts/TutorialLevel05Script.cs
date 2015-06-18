using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crystallize;

public class TutorialLevel05Script : LevelScript {

	public TriggerEventObject trigger;

	// Use this for initialization
	IEnumerator Start () {
		while (!LevelSystemConstructor.main) {
			yield return null;
		}

		yield return null;

		SetMessage ("Complete all 3 conversations!");

        //foreach (var entry in TutorialCanvas.main.ClientUI.Clients) {
        //    TutorialCanvas.main.CreateUIUpArrow((Vector2)entry.position + entry.rect.center - Vector2.up * 40f);
        //}

        //PlayerGameObject.rigidbody.WakeUp ();
        //PlayerController.UnlockMovement (this);

		trigger.OnTriggerExitEvent += HandleOnTriggerExitEvent;
	}

	void HandleOnTriggerExitEvent (object sender, TriggerEventArgs e)
	{
		TutorialCanvas.main.ClearAllIndicators ();
        //ClearMessages();
	}



}
