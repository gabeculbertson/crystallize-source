using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocialActor : MonoBehaviour {

	//public GameObject networkCardPrefab;
	//public GameObject friendRequestButtonsPrefab;
	//public GameObject setEffect;
	public SocialData socialData;

	// Use this for initialization
	void Start () {
		GetComponent<InteractiveDialogActor>().OnOpenDialog += HandleOnOpenDialog;
		GetComponent<InteractiveDialogActor>().OnExitDialog += HandleOnExitDialog;
		GetComponent<InteractiveDialogActor>().OnDialogueSuccess += HandleOnUnlock;
	}

	void HandleOnOpenDialog (object sender, PhraseEventArgs e)
	{
		if (DialogEnergyUI.main) {
			DialogEnergyUI.main.Open(GetComponent<InteractiveDialogActor>());
		}
	}

	void HandleOnExitDialog (object sender, PhraseEventArgs e)
	{
		if (DialogEnergyUI.main) {
			DialogEnergyUI.main.Close();
		}
	}

	void HandleOnUnlock (object sender, PhraseEventArgs e)
	{
		FriendsPanelUI.main.CreateFriendRequest (socialData);
	}
	
}
