using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class ItemButtonUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image itemIconImage;

    public int ItemID { get; set; }

    public event EventHandler OnDragStarted;

	// Use this for initialization
	void Start () {
	
	}

    public void Initialize(int itemID) {
        this.ItemID = itemID;
        var res = ScriptableObjectDictionaries.main.itemResourceDictionary.GetOrCreateItemResources(itemID);
        itemIconImage.sprite = res.icon;
    }

    public void OnDrag(PointerEventData eventData) {
        
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (OnDragStarted != null) {
            OnDragStarted(this, System.EventArgs.Empty);
        }

        CrystallizeEventManager.UI.RaiseItemDragStarted(this, new ItemDragEventArgs(ItemID));
    }

    public void OnEndDrag(PointerEventData eventData) {
        CrystallizeEventManager.UI.RaiseItemDropped(this, new ItemDragEventArgs(ItemID));
    }

    public void OnPointerEnter(PointerEventData eventData) {
        CrystallizeEventManager.UI.RaiseHoverOverItem(this, new ItemHoverEventArgs(true, "New item", GetTextPosition()));
    }

    public void OnPointerExit(PointerEventData eventData) {
        CrystallizeEventManager.UI.RaiseHoverOverItem(this, new ItemHoverEventArgs(false, "New item", GetTextPosition()));
    }

    Vector2 GetTextPosition() {
        var v = (Vector2)transform.position;
        v += Vector2.up * 16f;
        return v;
    }

}
