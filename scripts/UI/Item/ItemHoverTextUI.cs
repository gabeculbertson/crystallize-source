using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemHoverTextUI : UIMonoBehaviour {

    bool hovering = false;
    string text;

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.UI.OnHoverOverItem += OnHoverOverItem;
	}

    void OnHoverOverItem(object sender, ItemHoverEventArgs e) {
        if (e.IsHovering) {
            if (!gameObject.activeSelf) {
                gameObject.SetActive(true);
            }
            hovering = true;
            transform.position = e.Position;
            GetComponentInChildren<Text>().text = e.Text;
        } else {
            hovering = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (hovering) {
            this.FadeIn();
        } else {
            this.FadeOut();
            if (canvasGroup.alpha == 0) {
                gameObject.SetActive(false);
            }
        }
	}

}
