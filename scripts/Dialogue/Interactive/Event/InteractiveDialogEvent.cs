using UnityEngine;
using System.Collections;

public class InteractiveDialogEvent : MonoBehaviour {

	public GameObject eventObject;

	void Awake(){
		eventObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		GetComponent<InteractiveDialogActor>().OnDialogueSuccess += HandleOnUnlock;
	}

	void HandleOnUnlock (object sender, PhraseEventArgs e)
	{
		eventObject.SetActive (true);
	}
	

}
