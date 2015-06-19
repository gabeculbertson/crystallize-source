using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnlockConfirmationUI : MonoBehaviour {

    public Text confirmationText;

    AreaGameData area;

    public void Initialize(AreaGameData area) {
        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        this.area = area;
        confirmationText.text = string.Format("Unlock <color=yellow>{0}</color> for {1} yen?", area.AreaName, area.Cost);
    }

    public void Confirm() {
        if (PlayerManager.main.playerData.Money >= area.Cost) {
            PlayerManager.main.playerData.LevelData.SetAreaUnlocked(area.AreaID, true);
            PlayerManager.main.playerData.Money -= area.Cost;
            EffectManager.main.PlayMessage("Area unlocked!", Color.green);

            CrystallizeEventManager.PlayerState.RaiseMoneyChanged(this, System.EventArgs.Empty);
            CrystallizeEventManager.PlayerState.RaiseAreaUnlocked(this, System.EventArgs.Empty);

            CrystallizeEventManager.UI.RaiseUIRequest(this, new AreaTravelConfirmationUIRequestEventArgs(transform.parent.gameObject, area));
        } else {
            EffectManager.main.PlayMessage("Not enough money!", Color.red);
        }
        
        Close();
    }

    public void Close() {
        Destroy(gameObject);
    }

}
