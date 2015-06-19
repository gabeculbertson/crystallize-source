using UnityEngine;
using System.Collections;
using Util;

namespace Crystallize.Visualization {
	public class TextBubbleController : MonoBehaviour {

		public GameObject actorObject;

		bool isPlayer = false;
		IDialogActor actor;
		PhraseSegmentData phraseData;
		GameObject speechBubbleInstance;
		PointerType pointerType;

		void Start () {
			actor = actorObject.GetInterface<IDialogActor> ();
			if (transform.parent) {
				if(transform.parent.gameObject.tag == "Player"){
					isPlayer = true;
				}
			}

			pointerType = actor.SpeechBubblePointerType;
			actor.OnOpenDialog += HandleOnBeginDialog;
			actor.OnExitDialog += HandleOnExitDialog;
		}

		void HandleOnBeginDialog (object sender, PhraseEventArgs e)
		{
			if (speechBubbleInstance) {
				Destroy (speechBubbleInstance);
			}

			phraseData = e.PhraseData;
			if (phraseData != null){
				speechBubbleInstance = SpeechPanelUI.main.GetSpeechBubble (transform, phraseData, pointerType);
				if(isPlayer){
					speechBubbleInstance.GetComponent<SpeechBubbleUI>().SetTranslation(e.Translation);
				}
			}
		}

		void HandleOnExitDialog (object sender, PhraseEventArgs e)
		{
			phraseData = null;
			if (speechBubbleInstance) {
				Destroy(speechBubbleInstance);
			}
		}

	}
}