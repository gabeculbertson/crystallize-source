using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTradePanelUI : UIMonoBehaviour {

    public InputField playerMoneyInput;
    public Text otherMoneyText;
    public Image playerButtonImage;
    public Image otherButtonImage;
    public GameObject playerArrowImage;
    public GameObject otherArrowImage;
    public ItemInventoryUI mainInventory;
    public ItemInventoryUI playerInventory;
    public ItemInventoryUI otherInventory;

    Color buttonColor;

    int playerMoney = 0;
    int otherMoney = 0;
    bool playerReady = false;
    bool otherReady = false;

    bool hold = false;
    bool closing = false;

    TradeState playerState;
    TradeState otherState;

	// Use this for initialization
	void Start () {
        if (LevelSettings.main) {
            if (!LevelSettings.main.allowItemInventoryUI) {
                Destroy(gameObject);
                return;
            }
        }

        buttonColor = playerButtonImage.color;

        CrystallizeEventManager.UI.OnItemDragStarted += HandleItemDragStarted;
        CrystallizeEventManager.UI.OnTradeStateChanged += HandleTradeStateChanged;
        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;

        closing = true;
	}

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is TradeUIRequestEventArgs) {
            Open();
            hold = true;
        }
    }

    void Open() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
            if (closing) {
                SetPlayerReady(false);
                otherReady = false;
                closing = false;
            }
        }
    }

    public void Close() {
        SetEmpty();
        //CrystallizeEventManager.main.RaiseTradeStateChanged(this, new TradeStateEventArgs(GetPlayerTradeState()));
    }

    void HandleItemDragStarted(object sender, ItemDragEventArgs e) {
        Open();
    }

    void HandleTradeStateChanged(object sender, TradeStateEventArgs e) {
        if (sender == this) {
            return;
        }

        SetOtherState(e.State);
    }

    void Update() {
        UpdateConfirmationState();

        var s = GetPlayerTradeState();
        if (!s.IsContentSame(playerState) && !closing) {
            playerState = s;
            SetPlayerReady(false);
            otherReady = false;
            hold = true;
            CrystallizeEventManager.UI.RaiseTradeStateChanged(this, new TradeStateEventArgs(GetPlayerTradeState()));
        }

        if (hold || ItemDragHandler.main.IsDragging) {
            this.FadeIn();
        } else {
            this.FadeOut();
            if (canvasGroup.alpha == 0) {
                gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            ExecuteTrade();
        }
    }

    void UpdateConfirmationState() {
        playerArrowImage.SetActive(playerReady);
        otherArrowImage.SetActive(otherReady);

        if (playerReady) {
            playerButtonImage.color = buttonColor;
        } else {
            playerButtonImage.color = Color.white;
        }

        if (otherReady) {
            otherButtonImage.color = buttonColor;
        } else {
            otherButtonImage.color = Color.gray;
        }
    }

    public void UpdatePlayerMoney() {
        int m = 0;
        if (int.TryParse(playerMoneyInput.text, out m)) {
            m = Mathf.Clamp(m, 0, PlayerManager.main.playerData.Money);
            playerMoneyInput.text = m.ToString();
        }
        playerMoney = m;
    }

    public void SetEmpty() {
        playerMoneyInput.text = "";
        playerMoney = 0;
        otherMoney = 0;
        playerInventory.SetEmpty();
        otherInventory.SetEmpty();
        hold = false;
        closing = true;
    }

    void SetPlayerReady(bool ready) {
        if (playerReady != ready) {
            playerReady = ready;
            CrystallizeEventManager.UI.RaiseTradeStateChanged(this, new TradeStateEventArgs(new TradeState(playerReady)));
        }
    }

    public void TogglePlayerReady() {
        SetPlayerReady(!playerReady);

        if (playerReady && otherReady) {
            ExecuteTrade();
        }
    }

    TradeState GetPlayerTradeState() {
        return new TradeState(playerMoney, playerInventory.GetItems());
    }

    public void SetOtherState(TradeState state) {
        // there are 2 types of messages: trade content messages and confirmation messages
        if (state.IsReady) {
            otherReady = state.IsReady;

            if (playerReady && otherReady) {
                ExecuteTrade();
            }
        } else if (!state.IsContentSame(otherState)) {
            otherInventory.SetItems(state.Items);
            UpdateOtherMoney(state.Money);
            otherReady = false;
            SetPlayerReady(false);
            otherState = state;
        } 
    }

    public void UpdateOtherMoney(int amount) {
        if (amount != otherMoney) {
            otherMoney = amount;
            otherMoneyText.text = otherMoney + " ¥";
        }
    }

    void ExecuteTrade() {
        PlayerManager.main.playerData.Money -= playerMoney;
        PlayerManager.main.playerData.Money += otherMoney;

        playerInventory.SetEmpty();
        foreach (var item in otherInventory.GetItems()) {
            if (item != 0) {
                mainInventory.AddItem(item);
            }
        }
        CrystallizeEventManager.PlayerState.RaiseMoneyChanged(this, System.EventArgs.Empty);

        Close();
        EffectManager.main.PlayMessage("Trade successful!", Color.cyan);
    }

}
