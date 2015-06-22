using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThoughtBubbleDialogueAvailableUI : MonoBehaviour {

    public Image icon;
    public Image cross;
    public Text text;

    public void SetState(bool active) {
        if (active) {
            icon.color = Color.white;
            text.color = Color.black;
            cross.gameObject.SetActive(false);
        } else {
            icon.color = GUIPallet.Instance.lightGray;
            text.color = GUIPallet.Instance.inactiveColor;
            cross.gameObject.SetActive(true);
        }
    }

}
