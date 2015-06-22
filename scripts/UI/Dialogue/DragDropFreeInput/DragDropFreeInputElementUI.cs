using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class DragDropFreeInputElementUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public event EventHandler OnClicked;
    public event EventHandler OnDragStarted;

    PhraseSequenceElement phraseElement;

    public int Index { get; set;}

	public void Initialize (int index, PhraseSequenceElement phraseElement) {
        this.phraseElement = phraseElement;
        this.Index = index;
        GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
        GetComponent<Image>().color = GUIPallet.Instance.GetColorForWordCategory(phraseElement.GetPhraseCategory());
        GetComponentInChildren<Text>().text = phraseElement.GetPlayerText();
        GetComponentInChildren<Text>().font = GUIPallet.Instance.defaultFont;
        GetComponentInChildren<Text>().color = Color.black;
	}


    public void OnPointerClick(PointerEventData eventData) {
        if (OnClicked != null) {
            OnClicked(this, EventArgs.Empty);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        
    }

    public void OnBeginDrag(PointerEventData eventData) {
        UISystem.main.PhraseDragHandler.BeginDrag(phraseElement, eventData.position);

        if (OnDragStarted != null) {
            OnDragStarted(this, EventArgs.Empty);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        
    }
}
