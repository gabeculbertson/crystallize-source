using UnityEngine;
using System.Collections;

public class FullInventoryButtonUI : MonoBehaviour, IFullInventoryUI {

	void Start () {
        TutorialCanvas.main.FullInventoryButton = this;
	}

    public RectTransform GetRectTransform() {
        return GetComponent<RectTransform>();
    }

}
