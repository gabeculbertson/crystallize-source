using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum UIMode
{
    Exploring,
    Speaking,
    FixedPhraseInput
}

public class UISystem : MonoBehaviour {

	public static UISystem main { get; set; }

    public static bool MouseOverUI() {
        if (main.PhraseDragHandler.IsDragging) {
            return true;
        }

        return UIUtil.MouseOverUI();
    }

    UIMode lastMode = UIMode.Exploring;

	public IPhraseDragHandler PhraseDragHandler { get; set; }
	public UIMode Mode { get; set; }

	[SerializeField]
	GameObject phraseDragHandlerObject;

    HashSet<object> centerPanels = new HashSet<object>();

    public IEnumerable<object> CenterPanels {
        get {
            return centerPanels;
        }
    }

    public event EventHandler<UIInputEventArgs> OnInputEvent;

    KeyCode[] UIKeys = { KeyCode.Space, KeyCode.Return };

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {
		PhraseDragHandler = phraseDragHandlerObject.GetInterface<IPhraseDragHandler> ();

        CrystallizeEventManager.UI.OnUIModeRequested += HandleUIModeRequested;
	}

    void HandleUIModeRequested(object sender, UIModeChangedEventArgs e)
    {
        if (e.Mode != Mode)
        {
            Mode = e.Mode;
            CrystallizeEventManager.UI.RaiseUIModeChanged(this, new UIModeChangedEventArgs(Mode));
        }
    }

    void Update() {
        foreach (var uiKey in UIKeys) {
            if (Input.GetKeyDown(uiKey)) {
                if (OnInputEvent != null) {
                    OnInputEvent(this, new UIInputEventArgs(uiKey));
                }
            }
        }

        if (Mode != lastMode) {
            lastMode = Mode;
            CrystallizeEventManager.UI.RaiseUpdateUI(this, EventArgs.Empty);
        }
    }

    public void AddCenterPanel(object panel) {
        Debug.Log("Adding " + panel);
        if (!centerPanels.Contains(panel)) {
            centerPanels.Add(panel);
        }
    }

    public void RemoveCenterPanel(object panel) {
        Debug.Log("Removing " + panel);
        if (centerPanels.Contains(panel)) {
            centerPanels.Remove(panel);
        }
    }

    public bool ContainsCenterPanel() {
        return centerPanels.Count > 0;
    }

}
