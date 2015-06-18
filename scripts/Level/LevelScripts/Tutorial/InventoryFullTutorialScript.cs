using UnityEngine;
using System.Collections;

public class InventoryFullTutorialScript : LevelScript {

    public const int ID = 1;

    IEnumerator Start() {
        PlayerManager.main.playerData.Tutorial.SetTutorialViewed(ID);

        SetMessage("Inventory getting full!");

        var rt = TutorialCanvas.main.FullInventoryButton.GetRectTransform();
        //Debug.Log(rt);
        TutorialCanvas.main.CreateUIDragBox(rt, "Click here!");

        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.UI.OnUIRequested -= HandleUIRequested;

        TutorialCanvas.main.ClearAllIndicators();

        SetMessage("Move words so that words you want to access most frequently are in the top 2 rows.");

        Destroy(gameObject);
    }

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is FullInventoryUIRequestEventArgs) {
            Continue(sender, e);
        }
    }

}
