using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class EmptySlotUI : MonoBehaviour, IPhraseDropHandler, IItemDropAcceptor, IPhraseDropEvent, IPointerEnterHandler, IPointerExitHandler {

    public bool controlColor = true;

	public event EventHandler<PhraseEventArgs> OnPhraseDropped;
    public event EventHandler<ItemDragEventArgs> OnItemDropped;

	void Start(){
        if (controlColor) {
            GetComponent<Image>().color = GUIPallet.Instance.darkGray;
        }
	}

	public void AcceptDrop (IWordContainer phraseObject)
	{
		if (OnPhraseDropped != null) {
			OnPhraseDropped(this, new PhraseEventArgs(phraseObject));
		}
	}

    public void AcceptDrop(int itemID, GameObject obj) {
        if (OnItemDropped != null) {
            OnItemDropped(this, new ItemDragEventArgs(itemID));
        }
    }

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (UISystem.main.PhraseDragHandler.IsDragging) {
            if (controlColor) {
                GetComponent<Image>().color = Color.yellow;
            }
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
        if (UISystem.main.PhraseDragHandler.IsDragging) {
            if (controlColor) {
                GetComponent<Image>().color = GUIPallet.Instance.darkGray;
            }
        }
	}

}
