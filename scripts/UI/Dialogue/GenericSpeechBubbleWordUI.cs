using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class GenericSpeechBubbleWordUI : MonoBehaviour, IPointerClickHandler {

	PhraseSequenceElement word;

    public PhraseSequenceElement Word {
        get {
            return word;
        }
    }

    public event EventHandler OnClicked;

	public void Initialize(PhraseSegmentData word){
		this.word = word.GetPhraseElement();
		GetComponent<Image> ().color = GUIPallet.Instance.GetColorForWordCategory (word.Category);
        GetComponentInChildren<Outline>().effectColor = GUIPallet.Instance.GetColorForWordCategory(word.Category);
	}

    public void Initialize(PhraseSequenceElement word) {
        this.word = word;
        GetComponentInChildren<Text>().text = word.GetPlayerText();
        var c = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
        c = Color.Lerp(Color.white, c, 0.5f);
        GetComponent<Image>().color = c;
        //GetComponentInChildren<Outline>().effectColor = c;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (OnClicked != null)
        {
            OnClicked(this, EventArgs.Empty);
        }
    }
}
