using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WordInventoryHotbarUI : WordInventoryUI, IObjectiveUI, IInventoryUI {

    // Scrollbar stuff
    public Scrollbar scrollbar;
    public int currentOffset = 0;
    bool isUpdatingScrollBar = false;

    // TODO: move this out
    GameObject menuParent;

    public IEnumerable<RectTransform> Entries {
        get {
            return (from e in resourcePool select e.GetComponent<RectTransform>());
        }
    }

    protected override int StartIndex {
        get {
            return currentOffset * RowCount;
        }
    }

    protected override int EndIndex {
        get {
            return StartIndex + SlotCount;
        }
    }

    protected override int SlotCount {
        get {
            return 10;
        }
    }

    //TODO: get rid of this
    public float Height { get; set; }

    protected override bool BeforeStart() {
        TutorialCanvas.main.InventoryUI = this;
        TutorialCanvas.main.ObjectiveUI = this;

        if (scrollbar) {
            UpdateScrollbar();
            scrollbar.onValueChanged.AddListener(SetScrollPosition);
        }

        menuParent = new GameObject("MenuParent");
        menuParent.transform.SetParent(MainCanvas.main.transform);

        CrystallizeEventManager.UI.OnUIModeChanged += HandleUIModeChanged;
        //UpdateMode(UIMode.Exploring);

        return true;
    }

    void HandleUIModeChanged(object sender, UIModeChangedEventArgs e)
    {
        UpdateMode(e.Mode);
    }

    void UpdateMode(UIMode mode)
    {
        if (mode == UIMode.Exploring)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected override void BeforeInitialize() {
        while (words.Count < SlotCount) {
            words.Add(null);
        }
    }

    protected override void BeforeRefreshInventory() {
        UpdateScrollbar(false);
    }

    protected override void BeforeWordReceived(PhraseSequenceElement word) {
        if (IndexOf(word) >= 0) {
            if (IsObjective(word)) {
                AudioManager.main.PlayWordSuccess();
                //var slot = IndexOf(e.Word);
                //words[slot] = e.Word;
                PlayerData.Instance.WordStorage.AddFoundWord(word.WordID);
                UpdatePlayerData();
            } else {
                AudioManager.main.PlayWordFailure();
                EffectManager.main.PlayMessage("You already have that word!");
                DataLogger.LogTimestampedData("AlreadyFound", word.WordID.ToString());
            }
        } else {
            AudioManager.main.PlayWordSuccess();
            var slot = GetFirstEmptySlot();
            if (slot >= words.Count) {
                words.Add(word);
            } else {
                words[slot] = word;
            }
            PlayerData.Instance.WordStorage.AddFoundWord(word.WordID);
            UpdatePlayerData();
            RefreshInventory();
        }
    }

    public RectTransform GetEntry(PhraseSequenceElement word) {
        for (int i = 0; i < words.Count; i++) {
            if (words[i] != null) {
                if (words[i].WordID == word.WordID) {
                    return resourcePool[i].GetComponent<RectTransform>();
                }
            }
        }
        return null;
    }

    public RectTransform GetObjective(PhraseSequenceElement word) {
        return GetEntry(word);
    }

    public int GetMaxScroll() {
        var filledCount = PlayerData.Instance.WordStorage.GetInventoryCount(); //GetFilledCount();
        //Debug.Log("Inventory count: " + filledCount + "; " + ((filledCount / RowCount)));
        return Mathf.Max(((filledCount - 1) / RowCount), 0);
    }

    public void ScrollToTop() {
        currentOffset = 0;
        RefreshInventory();
    }

    public void ScrollUp() {
        currentOffset = Mathf.Max(currentOffset - 1, 0);
        
        if (!scrollbar) {
            RefreshInventory();
        } 
        UpdateScrollbar();
    }

    public void ScrollDown() {
        currentOffset = Mathf.Min(currentOffset + 1, GetMaxScroll());
        
        if (!scrollbar) {
            RefreshInventory();
        } 
        UpdateScrollbar();
    }

    public void ScrollToBottom() {
        currentOffset = GetMaxScroll();
        RefreshInventory();
    }

    void SetScrollPosition(float val) {
        //Debug.Log("Set called");
        currentOffset = Mathf.RoundToInt(val * (scrollbar.numberOfSteps - 1));
        if (!isUpdatingScrollBar) {
            RefreshInventory();
        }
    }

    void UpdateScrollbar(bool refreshInventory = true) {
        if (scrollbar) {
            var maxScroll = GetMaxScroll();
            scrollbar.numberOfSteps = maxScroll + 1;
            //Debug.Log("Scroll steps: " + scrollbar.numberOfSteps);
            if (scrollbar.numberOfSteps > 1) {
                scrollbar.size = 0.1f;//1f / maxScroll;
            } else {
                scrollbar.size = 1f;
            }


            if (!refreshInventory) {
                isUpdatingScrollBar = true;
            }

            if (maxScroll > 0) {
                scrollbar.value = (float)currentOffset / maxScroll;
            } else {
                scrollbar.value = 0;
            }

            if (!refreshInventory) {
                isUpdatingScrollBar = false;
            }
        }
    }

    // TODO: these should not be here
    public void OpenFullInventory() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new FullInventoryUIRequestEventArgs(menuParent));
    }

    public void OpenEmoticonPanel() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new EmoticonPanelUIRequestEventArgs());
    }

    public void OpenMainMenu() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new MainMenuUIRequestEventArgs());
    }

    public void OpenAreas() {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new AreaMenuUIRequestEventArgs(menuParent));
    }

}
