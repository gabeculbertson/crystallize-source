using UnityEngine;
using System.Collections;

public class SlotClickTutorialScript : LevelScript {

    public const int ID = 2;

    RectTransform slot;
    
    public void Initialize(RectTransform slot) {
        this.slot = slot;
    }

	// Use this for initialization
	IEnumerator Start () {
        PlayerData.Instance.Tutorial.SetTutorialViewed(ID);

        SetMessage("Some words have been moved out of your quick inventory. Click a word slot to see a list of all your words.");

        if (slot) {
            TutorialCanvas.main.CreateUIDragBox(slot, "Click here!");
        }

        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.UI.OnUIRequested -= HandleUIRequested;

        TutorialCanvas.main.ClearAllIndicators();

        SetMessage("Click a word to choose it.");

        Destroy(gameObject);
	}

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is WordSelectionUIRequestEventArgs) {
            Continue(sender, e);
        }
    }
}
