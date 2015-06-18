using UnityEngine;
using System.Collections;
using Util;

namespace Crystallize.Visualization {
	public class SpeechBubbleController : MonoBehaviour {

		public GameObject textSourceObject;

		GameObject speechBubbleInstance;

		void Start () {
			var source = textSourceObject.GetInterface<ISpeechTextSource> ();

			source.OnSpeechTextChanged += HandleOnSpeechTextChanged;
		}

		void HandleOnSpeechTextChanged (object sender, PhraseEventArgs e)
		{
			if (speechBubbleInstance) {
				Destroy (speechBubbleInstance);
			}

			if (e.Phrase != null){
				speechBubbleInstance = SpeechPanelUI.main.GetSpeechBubble (transform, e.Phrase, PointerType.Normal, false, false);
			}
		}

	}
}