using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WordSlotUI : UIMonoBehaviour, IPhraseDropHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

	//public PhraseSegmentData word;
    public PhraseSequenceElement word;

	public event EventHandler<PhraseEventArgs> OnWordDropped;
    public event EventHandler OnWordCleared;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
		RefreshVisualState ();
	}

	void RefreshVisualState(){
		GetComponent<LayoutElement>().preferredWidth = 160f;
		if (word != null) {
			GetComponentInChildren<Text> ().text = word.GetPlayerText();
			GetComponentInChildren<Text> ().font = GUIPallet.Instance.defaultFont;
			GetComponentInChildren<Text> ().color = Color.black;
			GetComponent<Image> ().color = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
		} else {
			GetComponentInChildren<Text> ().text = "";
			GetComponentInChildren<Text> ().color = Color.black;
			GetComponent<Image> ().color = Color.black;
		}
	}

	public void AcceptDrop (IWordContainer phraseObject)
	{
        if (OnWordDropped != null) {
            OnWordDropped(this, new PhraseEventArgs(phraseObject));
        }
	}

    public void ClearWord() {
        word = null;
        RefreshVisualState();

        if (OnWordCleared != null) {
            OnWordCleared(this, System.EventArgs.Empty);
        }
    }

    public void SetWord(PhraseSequenceElement word) {
        this.word = word;
        RefreshVisualState();
    }

	public void OnPointerClick (PointerEventData eventData)
	{
		if (word != null) {
            ClearWord();
		}
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if (word != null) {
			var go = UISystem.main.PhraseDragHandler.BeginDrag (word, eventData.position);
			go.GetComponent<InventoryEntryUI> ().type = InventoryType.Conversation;
            ClearWord();
		}
	}

	public void OnEndDrag (PointerEventData eventData){}
	public void OnDrag (PointerEventData eventData){}

}
