using UnityEngine;
using System.Collections;
using Crystallize;

public class DialogueAnimationSet : MonoBehaviour {

	public GameObject actorObject;
	public string animationName;
	//public DialogueEventType eventType = DialogueEventType.Open;

	DialogueActor actor;

	// Use this for initialization
	void Start () {
		if (!actorObject) {
			actor = gameObject.GetComponent<DialogueActor>();
		} else {
			actor = actorObject.GetComponent<DialogueActor>();
		}

		CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
	}

	void HandleActorApproached (object sender, System.EventArgs e)
	{
		if ((DialogueActor)sender == actor) {
			CrystallizeEventManager.Environment.OnActorApproached -= HandleActorApproached;
			GetComponentInChildren<Animator> ().CrossFade (animationName, 0.2f);
		}
	}

}
