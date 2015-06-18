using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SpeechBubbleWordUI : MonoBehaviour, IPointerDownHandler, IPointerClickHandler {

	PhraseSequenceElement word;

    public PhraseSequenceElement Word {
        get {
            return word;
        }
    }

	public void Initialize(PhraseSegmentData word){
		this.word = word.GetPhraseElement();
		GetComponent<Image> ().color = GUIPallet.main.GetColorForWordCategory (word.Category);
        GetComponentInChildren<Outline>().effectColor = GUIPallet.main.GetColorForWordCategory(word.Category);
	}

    public void Initialize(PhraseSequenceElement word) {
        this.word = word;
        GetComponentInChildren<Text>().text = word.GetPlayerText();
        var c = GUIPallet.main.GetColorForWordCategory(word.GetPhraseCategory());
        GetComponent<Image>().color = c;
        GetComponentInChildren<Outline>().effectColor = c;
    }

	public void OnPointerDown (PointerEventData eventData)
	{
        if (eventData.button == PointerEventData.InputButton.Left) {
            UISystem.main.PhraseDragHandler.BeginDrag(word, eventData.position);
        }
	}

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            CrystallizeEventManager.UI.RaiseWordClicked(this, new WordClickedEventArgs(word, "Inventory"));
        } else if(eventData.button == PointerEventData.InputButton.Right) {
            CrystallizeEventManager.UI.RaiseWordClicked(this, new WordClickedEventArgs(word, "Dictionary"));
        }
    }
}
