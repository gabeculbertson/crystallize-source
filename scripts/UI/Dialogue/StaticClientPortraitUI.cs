using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;	
using Crystallize;

public class StaticClientPortraitUI : MonoBehaviour {
	
	//public GameObject lockEffect;
	public Image portraitImage;
	public GameObject anonImage;
	public GameObject questionEffect;
	public GameObject exclaimationEffect;
	public GameObject countEffect;
	public GameObject checkEffect;
	public GameObject lockEffect;

	ConversationClient client;
	List<PhraseSegmentData> phraseData;

	GameObject[] Effects { 
		get {
			return new GameObject[]{ anonImage, exclaimationEffect, questionEffect, countEffect, checkEffect, lockEffect};
		}
	}

	public ConversationClient Client {
		get {
			return client;
		}
	}

	public void Initialize(InteractiveDialogActor actor){
		this.client = actor.GetComponent<ConversationClient>();
		phraseData = client.GetObjectiveWords ();
		portraitImage.sprite = client.clientData.socialData.portrait;
		client.OnStateChanged += HandleOnStateChanged;
	}

	// Use this for initialization
	void Start () {
		RefreshState ();
	}

	void Update(){

	}

	void OnDisable(){
		client.OnStateChanged -= HandleOnStateChanged;
	}

	void RefreshState(){
		foreach (var eff in Effects) {
			eff.SetActive(false);
		}

		switch (client.State) {
		case ConversationClientState.Locked:
			lockEffect.SetActive(true);
			anonImage.SetActive(true);
			var level = client.GetComponent<InteractiveDialogActor>().minimumLevel;
			lockEffect.GetComponentInChildren<Text>().text = level.ToString();
			break;

		case ConversationClientState.SeekingClient:
			anonImage.SetActive(true);
			questionEffect.SetActive (true);
			break;

		case ConversationClientState.SeekingWords:
			countEffect.SetActive(true);
			
			int completed = 0;
			foreach (var word in phraseData) {
				if(PlayerManager.main.playerData.WordStorage.ContainsFoundWord(word)){
					completed++;
				}
			}
			countEffect.GetComponentInChildren<Text>().text =  string.Format ("{0}/{1}", completed, phraseData.Count);
			break;

		case ConversationClientState.Available:
			exclaimationEffect.SetActive (true);
			break;

		case ConversationClientState.Completed:
			checkEffect.SetActive (true);
			break;
		}
	}

	void HandleOnStateChanged (object sender, System.EventArgs e)
	{
		RefreshState ();
	}

}
