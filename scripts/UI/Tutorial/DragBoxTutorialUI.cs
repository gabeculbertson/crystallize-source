using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragBoxTutorialUI : UIMonoBehaviour {

    public Text infoText;
    public RectTransform target;

    public void Initialize(RectTransform target, string text) {
        infoText.text = text;
        this.target = target;
    }
	
	// Update is called once per frame
	void Update () {
        if (target) {
            rectTransform.pivot = target.pivot;
            rectTransform.position = target.position;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, target.rect.height);
        }  
	}
}
