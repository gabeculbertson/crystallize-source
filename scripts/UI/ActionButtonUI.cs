using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionButtonUI : MonoBehaviour {

    void Start() {
        if (LevelSettings.main) {
            if (!LevelSettings.main.allowItemInventoryUI) {
                Destroy(gameObject);
                return;
            }
        }
    }

    public void OpenTradePanel() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new TradeUIRequestEventArgs());
    }

    public void CloseSpeechBubble() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new SpeechPanelUIRequestEventArgs(false));
    }

}
