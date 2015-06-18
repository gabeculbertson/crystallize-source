using UnityEngine;
using System;
using System.Collections;

namespace Crystallize {
    public class PassiveDialogActor : MonoBehaviour, IDialogActor, ITriggerEventHandler {

        //public PassiveDialogData passiveDialogData;
        public bool isFemale = false;
        public float delay = 0;
        public PhraseSegmentData phrase;
        public bool isSolo = false;
        public PointerType pointerType;

        public event EventHandler<PhraseEventArgs> OnOpenDialog;
        public event EventHandler<PhraseEventArgs> OnExitDialog;
        public event EventHandler<PhraseEventArgs> OnDialogueSuccess;

        IEnumerator coroutine;

        public PointerType SpeechBubblePointerType {
            get {
                return pointerType;
            }
        }

        public void PlayPhrase() {
            SetPhrase(phrase);
        }

        public void SetPhrase(PhraseSegmentData phrase) {
            if (phrase == null) {
                if (OnExitDialog != null) {
                    OnExitDialog(this, PhraseEventArgs.Empty);
                }
                if (coroutine != null) {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
            } else {
                coroutine = WaitAndSetBubble();
                StartCoroutine(coroutine);
            }
        }

        IEnumerator WaitAndSetBubble() {
            yield return new WaitForSeconds(delay);

            if (OnOpenDialog != null) {
                OnOpenDialog(this, new PhraseEventArgs(phrase));
            }

            PlayAudio(phrase);

            coroutine = null;
        }

        void PlayAudio(PhraseSegmentData phrase) {
            AudioClip clip = phrase.MaleAudioClip;
            if (isFemale && phrase.FemaleAudioClip) {
                clip = phrase.FemaleAudioClip;
            }

            if (!clip) {
                return;
            }

            if (!GetComponent<AudioSource>()) {
                gameObject.AddComponent<AudioSource>();
            }
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().time = 0;
            GetComponent<AudioSource>().Play();
        }

        public void HandleTriggerEntered(object sender, TriggerEventArgs args) {
            PlayPhrase();
            //Debug.Log ("Trigger entered.");
        }

        public void HandleTriggerExited(object sender, TriggerEventArgs args) {
            SetPhrase(null);
            //Debug.Log ("Trigger exited.");
        }
    }
}