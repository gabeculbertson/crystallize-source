using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpeechBubbleDropAreaUI : MonoBehaviour, IPhraseDropHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

	static PlayerSpeechBubbleDropAreaUI _tempSource;

    public PhraseSequenceElement word;

	Color originalColor;
    GameObject menuParent;

	public event EventHandler<PhraseEventArgs> OnWordDropped;
    public event EventHandler OnStateChanged;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
		originalColor = GetComponent<Image> ().color;
		RefreshVisualState ();
	}

	void OnDisable(){
		PlayerController.UnlockMovement (this);
	}

    //void Update(){
    //    if (word != null) {
    //        if (lockDrop){
    //            GetComponent<Image> ().color = Color.clear;
    //        } else if (rejected && !lockDrop) {
    //            var c = GetComponent<Image> ().color;
    //            c.a = 0.5f + Mathf.PingPong (Time.time, 0.5f);
    //            GetComponent<Image> ().color = c;
    //        } else {
    //            var c = GetComponent<Image> ().color;
    //            c.a = 1f;
    //            GetComponent<Image> ().color = c;
    //        }
    //    }
    //}

	void RefreshVisualState(){
		GetComponent<LayoutElement>().preferredWidth = 160f;
		if (word != null) {
			GetComponentInChildren<Text> ().text = word.GetPlayerText();

			GetComponentInChildren<Text> ().font = GUIPallet.Instance.defaultFont;
			GetComponentInChildren<Text> ().color = Color.black;
			if(word.GetPhraseCategory() == PhraseCategory.Punctuation){
				GetComponent<Image> ().color = Color.clear;
			} else {
				GetComponent<Image> ().color = GUIPallet.Instance.GetColorForWordCategory(word.GetPhraseCategory());
			}
		} else {
            if (PlayerData.Instance.Flags.GetFlag(FlagPlayerData.ClickWordSlotMessage)) {
                GetComponentInChildren<Text>().text = "click for choices";
            } else {
                GetComponentInChildren<Text>().text = "";
            }
			GetComponentInChildren<Text> ().color = Color.gray;
			GetComponent<Image> ().color = originalColor;
		}
	}

	public void AcceptDrop (IWordContainer phraseObject)
	{
        DestroyMenuParent();

		if (_tempSource) {
			if(word != null){
				_tempSource.AcceptPhrase(word);
			}

			_tempSource = null;
		}

		AcceptPhrase (phraseObject.Word);
        RaiseStateChanged();

        if (phraseObject.gameObject) {
            Destroy(phraseObject.gameObject);
        }
		AudioManager.main.PlayWordSuccess ();
	}

	public void AcceptPhrase(PhraseSequenceElement word){
		this.word = word;

		RefreshVisualState ();

		if (OnWordDropped != null) {
			OnWordDropped(this, new PhraseEventArgs(word));
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (word != null) {
			word = null;
            RaiseStateChanged();
			RefreshVisualState ();
        } else {
            CrystallizeEventManager.UI.RaiseUIRequest(this, new WordSelectionUIRequestEventArgs(GetMenuParent(), this));
        }        
	}

    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (word != null) {
			var go = UISystem.main.PhraseDragHandler.BeginDrag (word, eventData.position);
			go.GetComponent<InventoryEntryUI> ().type = InventoryType.Conversation;
			word = null;
			RefreshVisualState();
		}
	}

    void RaiseStateChanged() {
        if (OnStateChanged != null) {
            OnStateChanged(this, System.EventArgs.Empty);
        }
    }

    GameObject GetMenuParent() {
        if (!menuParent) {
            menuParent = new GameObject("menu parent");
            menuParent.transform.SetParent(FieldCanvas.main.transform);
        }
        return menuParent;
    }

    void DestroyMenuParent() {
        if (menuParent) {
            Destroy(menuParent);
        }
    }

}
