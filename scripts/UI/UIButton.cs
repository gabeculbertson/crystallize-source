using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIButton : MonoBehaviour, IPointerClickHandler {

    public event EventHandler OnClicked;

    public void OnPointerClick(PointerEventData eventData) {
        if (OnClicked != null) {
            OnClicked(this, EventArgs.Empty);
        }
    }
}
