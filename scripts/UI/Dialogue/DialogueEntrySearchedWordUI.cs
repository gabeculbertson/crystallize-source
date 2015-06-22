using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueEntrySearchedWordUI : UIMonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	PhraseSegmentData phrase;
	PlayerSpeechBubbleDropAreaUI dropSection;

	// Use this for initialization
	void Start () {
		canvasGroup.alpha = 0.5f;
	}
	
	public void Initialize(PlayerSpeechBubbleDropAreaUI dropSection, PhraseSegmentData phrase){
		this.dropSection = dropSection;
		this.phrase = phrase;

		GetComponent<Image> ().color = GUIPallet.Instance.GetColorForWordCategory (phrase.Category);
		GetComponentInChildren<Text> ().text = phrase.ConvertedText;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		Debug.Log ("clicked.");
		dropSection.AcceptPhrase (phrase.GetPhraseElement());
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		canvasGroup.alpha = 1f;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		canvasGroup.alpha = 0.5f;
	}
	
}
