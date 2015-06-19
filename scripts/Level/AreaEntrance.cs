using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AreaEntrance : MonoBehaviour {

    enum AreaState {
        TooExpensive,
        Available,
        Unlocked
    }

    public Text costText;
    public Image arrowImage;
    public int destinationAreaID = -1;
    public bool enforcePayment = true;

    AreaGameData area;
    GameObject menuParent;

	// Use this for initialization
	IEnumerator Start () {
        while (!PlayerManager.main) {
            yield return null;
        }

        yield return null;

        area = GameData.Instance.NavigationData.Areas.GetItem(destinationAreaID);
        var trigger = gameObject.GetComponentInChildren<TriggerEventObject>();
        if (trigger) {
            trigger.OnTriggerEnterEvent += HandleTriggerEnterEvent;
            trigger.OnTriggerExitEvent += HandleTriggerExitEvent;
        }

        CrystallizeEventManager.PlayerState.OnMoneyChanged += HandleStateChanged;
        CrystallizeEventManager.PlayerState.OnAreaUnlocked += HandleStateChanged;
		CrystallizeEventManager.PlayerState.OnFlagChanged += HandleOnFlagChanged;

        UpdateState();
	}

	void HandleOnFlagChanged (object sender, TextEventArgs e)
	{
		if (e.Text == FlagPlayerData.IsMultiplayer) {
			UpdateState ();
		}
	}

    void HandleStateChanged(object sender, System.EventArgs e) {
        UpdateState();
    }

    void UpdateState() {
		if (PlayerData.Instance.Flags.GetFlag (FlagPlayerData.IsMultiplayer)) {
            if (!GameSettings.GetFlag(GameSystemFlags.LockQuestInterdependence) || enforcePayment) {
                gameObject.SetActive(false);
                return;
            }
		} else {
			gameObject.SetActive(true);
		}

        if (enforcePayment) {
            costText.text = area.Cost + " yen";
        } else {
            costText.text = "";
        }

        if (enforcePayment) {
            switch (GetState()) {
                case AreaState.TooExpensive:
                    costText.color = Color.red;
                    arrowImage.color = Color.red;
                    break;

                case AreaState.Available:
                    costText.color = Color.yellow;
                    arrowImage.color = Color.yellow;
                    break;

                case AreaState.Unlocked:
                    costText.text = "";
                    arrowImage.color = Color.green;
                    break;
            }
        } else {
            if (ObjectiveManager.main.IsComplete) {
                arrowImage.color = Color.green;
            } else {
                arrowImage.color = Color.red;
            }
        }
    }

    AreaState GetState() {
        if (PlayerManager.main.playerData.LevelData.GetAreaUnlocked(destinationAreaID)) {
            return AreaState.Unlocked;
        }

        if (PlayerManager.main.playerData.Money >= area.Cost) {
            return AreaState.Available;
        } else {
            return AreaState.TooExpensive;
        }
    }

    void HandleTriggerEnterEvent(object sender, TriggerEventArgs e) {
        if (e.Collider.IsPlayer()) {

            var area = GameData.Instance.NavigationData.Areas.GetItem(destinationAreaID);
            if (enforcePayment) {
                switch (GetState()) {
                    case AreaState.TooExpensive:
                        CrystallizeEventManager.UI.RaiseTutorialEvent(this, new MoneyLockTutorialEventArgs());
                        EffectManager.main.PlayMessage("Not enough money!", Color.red);
                        break;

                    case AreaState.Available:
                        CrystallizeEventManager.UI.RaiseUIRequest(this, new AreaUnlockConfirmationUIRequestEventArgs(GetMenuParent(), area));
                        break;

                    case AreaState.Unlocked:
                        CrystallizeEventManager.UI.RaiseUIRequest(this, new AreaTravelConfirmationUIRequestEventArgs(GetMenuParent(), area));
                        break;
                }
            } else {
                if (ObjectiveManager.main.IsComplete) {
                    CrystallizeEventManager.UI.RaiseUIRequest(this, new AreaTravelConfirmationUIRequestEventArgs(GetMenuParent(), area));
                } else {
                    EffectManager.main.PlayMessage("Finish the quests to continue!", Color.red);
                }
            }

        }
    }

    void HandleTriggerExitEvent(object sender, TriggerEventArgs e) {
        if (e.Collider.IsPlayer()) {
            if (menuParent) {
                Destroy(menuParent);
            }
        }
    }

    GameObject GetMenuParent() {
        if (!menuParent) {
            menuParent = new GameObject("MenuParent");
            menuParent.transform.SetParent(FieldCanvas.main.transform);
        }
        return menuParent;
    }
	
}
