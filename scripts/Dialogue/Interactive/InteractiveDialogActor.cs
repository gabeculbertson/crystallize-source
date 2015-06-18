using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Crystallize {
	public class InteractiveDialogActor : MonoBehaviour, IDialogActor {

		public FixedPlayerDialog playerDialog;
		public bool isFemale = false;
		public int minimumLevel = 0;

		public event EventHandler<PhraseEventArgs> OnOpenDialog;
		public event EventHandler<PhraseEventArgs> OnReact;
		public event EventHandler<PhraseEventArgs> OnPhraseSuccess;
		public event EventHandler<PhraseEventArgs> OnDialogueSuccess;
		public event EventHandler<PhraseEventArgs> OnReject;
		public event EventHandler<PhraseEventArgs> OnExitDialog;
		public event EventHandler OnEnterTrigger;

		public bool IsComplete { get; set; }
		public bool HasBeenVisited { get; set; }

		public bool IsOpen { get; private set; }

		ConversationClient client;
		int turn = 0;

		IEnumerator setSequence;

		public bool AllObjectivesComplete { 
			get {
				foreach(var word in client.GetObjectiveWords()){//neededWords){
					if(!PlayerManager.main.playerData.WordStorage.ContainsFoundWord(word)){
						return false;
					}
				}
				return true;
			}
		}

		public PointerType SpeechBubblePointerType {
			get {
				return PointerType.Normal;
			}
		}

		void Start(){
			client = GetComponent<ConversationClient> ();
			if (client) {
				playerDialog = client.clientData.dialog;
			}
		}

		public void TriggerEntered(){
			if (OnEnterTrigger != null) {
				OnEnterTrigger(this, EventArgs.Empty);
			}
		}

		public void BeginDialog(){
			if (PlayerManager.main.playerData.InventoryState.Level < minimumLevel) {
				return;
			}

			if (!HasBeenVisited) {
				HasBeenVisited = true;
				CrystallizeEventManager.UI.RaiseOnProgressEvent(this, System.EventArgs.Empty);
			}
			IsOpen = true;

			if (IsComplete) {
				SetPhrase (playerDialog.successPhrase);
			} else {
				turn = 0;
				if(playerDialog){
					SetPhrase (playerDialog.dialogPhrases [0]);
				} 
			}
		}

		public void SetPhrase(PhraseSegmentData phrase){
			if (playerDialog) {
				if (turn + 1 < playerDialog.dialogPhrases.Count) {
					PlayerManager.main.TargetPhrase = playerDialog.dialogPhrases [turn + 1];
				}
			}

			StartCoroutine(PlayAudio(phrase));
			if (OnOpenDialog != null) {
				OnOpenDialog (this, new PhraseEventArgs(phrase));
			}
		}

		public void CloseDialogue(){
			IsOpen = false;
			if (OnExitDialog != null) {
				OnExitDialog (this, PhraseEventArgs.Empty);
			} 
		}

		IEnumerator PlayAudio(PhraseSegmentData phrase){
			if (phrase == null) {
				yield break;
			}

			AudioClip clip = phrase.MaleAudioClip;
			if (isFemale && phrase.FemaleAudioClip) {
				clip = phrase.FemaleAudioClip;
			}

			if (!clip) {
				yield break;
			}

			if (!GetComponent<AudioSource>()) {
				gameObject.AddComponent<AudioSource>();
			}
			GetComponent<AudioSource>().clip = clip;
			GetComponent<AudioSource>().Play ();
		}

		public virtual void ReactToPhrase(PhraseSegmentData phrase){
			if (setSequence != null) {
				StopCoroutine(setSequence);
			}

			if (phrase.Text == playerDialog.dialogPhrases [turn + 1].Text) {
				turn += 2;
				//var score = PhraseEvaluator.GetPhraseLevel(phrase);

				if (OnReact != null) {
					OnReact (this, PhraseEventArgs.Empty);
				}

				//Debug.Log(turn + "; " + playerDialog.dialogPhrases.Count);

				if (turn < playerDialog.dialogPhrases.Count) {
					var reaction = playerDialog.dialogPhrases [turn];
					FacePlayer ();
					SetPhrase (reaction);

					if(OnPhraseSuccess != null){
						OnPhraseSuccess(this, new PhraseEventArgs(reaction));
					}
				} else {
					SetPhrase (playerDialog.successPhrase);
					IsComplete = true;

					if (OnDialogueSuccess != null) {
						OnDialogueSuccess (this, new PhraseEventArgs (playerDialog.successPhrase));
					}
				}
			} else {
				if (OnReject != null) {
					OnReject (this, new PhraseEventArgs (playerDialog.successPhrase));
					SetPhrase(null);
					setSequence = WaitAndSetPhrase(2f, playerDialog.unsurePhrase);
					StartCoroutine(setSequence);
				} else {
					SetPhrase(playerDialog.unsurePhrase);
				}
				AudioManager.main.PlayDialogueFailure();
			}
		}

		IEnumerator WaitAndSetPhrase(float seconds, PhraseSegmentData phrase){
			yield return new WaitForSeconds (seconds);
			SetPhrase(playerDialog.unsurePhrase);
		}

		void FacePlayer(){
            var player = PlayerManager.main.PlayerGameObject;
			var dir = player.transform.position - transform.position;
			dir.y = 0;
			transform.forward = dir;
		}

	}
}