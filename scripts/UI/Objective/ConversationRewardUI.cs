using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConversationRewardUI : MonoBehaviour {

	public PhraseSegmentData phrase;
	public Text translationText;
	public Text wordText;

	// Use this for initialization
	void Start () {
		if (phrase) {
			wordText.text = phrase.ConvertedText;
			translationText.text = phrase.Translation;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
