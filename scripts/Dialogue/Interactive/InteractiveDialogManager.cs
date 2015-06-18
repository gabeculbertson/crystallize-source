using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class InteractiveDialogManager : MonoBehaviour {

	const float SpeakingDistanceThreshold = 4f;

	public static InteractiveDialogManager main { get; set; }

	public GameObject dialogTriggerPrefab;

	Queue<GameObject> dialogTriggerInstances = new Queue<GameObject>();

	public event EventHandler OnTriggerEntered;
	public event EventHandler OnDialogSuccess;
	//public event EventHandler OnPhraseRejected;

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {
		//OnDialogOpened += CrystallizeEventManager.main.RaiseInteractiveDialogueOpened;
		//OnDialogExited += CrystallizeEventManager.main.RaiseInteractiveDialogueClosed;

		CreateDialogTriggers ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateDialogTriggers(){
		while (dialogTriggerInstances.Count > 0) {
			Destroy(dialogTriggerInstances.Dequeue());
		}

		foreach (var actor in GetComponentsInChildren<InteractiveDialogActor>()) {
			actor.OnOpenDialog += HandleOnOpenDialog;
			actor.OnEnterTrigger += HandleEnterTrigger;
			actor.OnExitDialog += HandleOnExitDialog;
			actor.OnDialogueSuccess += HandleOnUnlock;
			//actor.OnLevelTooHigh += HandleOnLevelTooHigh;
			actor.OnReject += HandleOnReject;
			var instance = Instantiate(dialogTriggerPrefab, actor.transform.position, Quaternion.identity) as GameObject;
			instance.GetComponent<InteractiveDialogTrigger> ().Initialize (actor);
			instance.transform.SetParent(actor.transform);
			dialogTriggerInstances.Enqueue(instance);
		}
	}

	void HandleOnOpenDialog (object sender, PhraseEventArgs e)
	{
		CrystallizeEventManager.UI.RaiseInteractiveDialogueOpened (sender, e);
	}

	void HandleOnReject (object sender, PhraseEventArgs e)
	{
		//CrystallizeEventManager.main.RaiseOnPhraseRejected (sender, e);
	}

	void HandleOnUnlock (object sender, PhraseEventArgs e)
	{
		if (OnDialogSuccess != null) {
			OnDialogSuccess(sender, EventArgs.Empty);
		}
	}

	void HandleEnterTrigger (object sender, EventArgs e)
	{
//		var a = sender as InteractiveDialogActor;
//		var missingWords = a.GetComponent<ConversationClient> ().GetObjectiveWords ();
//		foreach (var word in missingWords) {
//			ObjectiveManager.main.AddObjectiveWord(gameObject, word);
//		}
		//Debug.Log ("Opening dialog.");
		if (OnTriggerEntered != null) {
			OnTriggerEntered(sender, EventArgs.Empty);
		}

//		if (a.GetComponent<ConversationClient>().State == ConversationClientState.SeekingWords) {
//			foreach(var word in missingWords){
//				if(!ObjectiveManager.main.IsWordFound(word)){
//					var entry = TutorialCanvas.main.GetWordObjective(word);
//					TutorialCanvas.main.CreateUIDownArrow((Vector2)entry.position 
//					                                  + entry.rect.center
//					                                  + Vector2.up * 10f);
//				}
//			}
//			MainCanvas.main.OpenNotificationPanel ("Find the words!");
//		}
	}

	void HandleOnExitDialog (object sender, PhraseEventArgs e)
	{
		TutorialCanvas.main.ClearAllIndicators ();
		MainCanvas.main.CloseNotificationPanel ();

		CrystallizeEventManager.UI.RaiseInteractiveDialogueClosed (sender, e);
	}

	public void PlayerPhraseEntered(PhraseSegmentData phraseData){
        var player = PlayerManager.main.PlayerGameObject;
		foreach (var actor in GetComponentsInChildren<InteractiveDialogActor>()) {
			if(Vector3.Distance(player.transform.position, actor.transform.position) < SpeakingDistanceThreshold){
				actor.ReactToPhrase(phraseData);
				break;
			}
		}
	}

}
