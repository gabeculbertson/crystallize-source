using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Crystallize {
	public class PlayerDialogActor : MonoBehaviour, IDialogActor, ITriggerConsumer {

		public static PlayerDialogActor main { get; set; }

		public event EventHandler<PhraseEventArgs> OnOpenDialog;
		public event EventHandler<PhraseEventArgs> OnExitDialog;
		public event EventHandler<PhraseEventArgs> OnDialogueSuccess;
		public event EventHandler OnEnterTrigger;
		public event EventHandler OnExitTrigger;

		public float currentTrust = 0;
		public float totalTrust = 1f;

		public ConversationClient currentClient;

		HashSet<GameObject> engagedTriggers = new HashSet<GameObject>();

		public PointerType SpeechBubblePointerType {
			get {
				return PointerType.Normal;
			}
		}

		void Awake(){
			main = this;
		}

		void Update(){
			if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
				currentTrust += 0.2f;
			}
		}

		public void SetPhrase(PhraseSegmentData phrase){
			if (phrase == null) {
				if (OnExitDialog != null) {
					OnExitDialog (this, PhraseEventArgs.Empty);
				} 
			} else {
				//StartCoroutine(PlayAudio(phrase));
				if (OnOpenDialog != null) {
					OnOpenDialog (this, new PhraseEventArgs(phrase));
				} 
			}
		}

		public void SetPhrase(PhraseSegmentData phrase, string translation){
			if (phrase == null) {
				if (OnExitDialog != null) {
					OnExitDialog (this, PhraseEventArgs.Empty);
				} 
			} else {
				//StartCoroutine(PlayAudio(phrase));
				if (OnOpenDialog != null) {
					OnOpenDialog (this, new PhraseEventArgs(phrase, translation));
				} 
			}
		}

		/*IEnumerator PlayAudio(PhraseSegmentData phrase){
			if (!phrase.AudioClip) {
				yield break;
			}

			if (!audio) {
				gameObject.AddComponent<AudioSource>();
			}
			audio.clip = phrase.AudioClip;
			audio.Play ();
		}*/

		public void TriggerEntered (GameObject trigger)
		{
			//int count = engagedTriggers.Count;
			if (!engagedTriggers.Contains (trigger)) {
				engagedTriggers.Add (trigger);
			}
			CheckEngagedTriggerState ();
		}

		public void TriggerExited (GameObject trigger)
		{
			if (engagedTriggers.Contains (trigger)) {
				engagedTriggers.Remove (trigger);
			}
			CheckEngagedTriggerState ();
		}

		void CheckEngagedTriggerState(){
			//Debug.Log("Checking trigger state.");
			if (engagedTriggers.Count > 0) {
				if(OnEnterTrigger != null){
					OnEnterTrigger(this, EventArgs.Empty);
				}
			} else {
				OnExitDialog(this, null);
				if(OnExitTrigger != null){
					OnExitTrigger(this, EventArgs.Empty);
				}
			}
		}

	}
}