using UnityEngine;
using System.Collections;
using Crystallize;

public class OpenDialogueEvent : MonoBehaviour {

	public GameObject actorObject;
	public GameObject eventObject;

	DialogueActor actor;

	void Awake(){
		eventObject.SetActive (false);
	}
	
	// Use this for initialization
	void Start () {
		if (!actorObject) {
			actor = gameObject.GetComponent<DialogueActor> ();
		} else {
			actor = actorObject.GetComponent<DialogueActor>();
		}

		CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
	}

	void HandleActorApproached (object sender, System.EventArgs e)
	{
		if ((DialogueActor)sender == actor) {
			eventObject.SetActive (true);
			CrystallizeEventManager.Environment.OnActorApproached -= HandleActorApproached;
		}
	}

}
