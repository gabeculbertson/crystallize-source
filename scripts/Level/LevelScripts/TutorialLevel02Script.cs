using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crystallize;

public class TutorialLevel02Script : LevelScript {

	public GameObject wasdTutorialPanel;
	public TriggerEventObject movementTrigger;
   
	// Use this for initialization
	IEnumerator Start () {
		while (!LevelSystemConstructor.main) {
			yield return null;
		}

		//PlayerDialogActor.main.OnEnterTrigger += HandleOnEnterTrigger;
		//PlayerController.LockMovement (this);
		ObjectiveManager.main.SetObjective (this, false);

		// wait for everything to initialize
		yield return null;

		SetMessage ("Move with WASD.");
		var wasdPanelInstance = Instantiate (wasdTutorialPanel) as GameObject;
		wasdPanelInstance.transform.SetParent (MainCanvas.main.transform);
		wasdPanelInstance.transform.position = new Vector2 (Screen.width * 0.5f, 300f);
		PlayerGameObject.GetComponent<Rigidbody>().WakeUp ();
		PlayerController.UnlockMovement (this);
		
		movementTrigger.OnTriggerExitEvent += Continue;
		yield return StartCoroutine (WaitForEvent ());
		movementTrigger.OnTriggerExitEvent -= Continue;
		
		Destroy (wasdPanelInstance);
		SetMessage ("Complete the conversation to continue.");

		CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleOnSpeechBubbleOpen;

		ObjectiveManager.main.SetObjective (this, true);
	}

	void HandleOnSpeechBubbleOpen (object sender, PhraseEventArgs e)
	{
        ClearMessages();
	}



}
