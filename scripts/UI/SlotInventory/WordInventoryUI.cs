using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordInventoryUI : MonoBehaviour {

    public GameObject slotPrefab;
    public GameObject objectiveSlotPrefab;
    public GameObject fillerSlotPrefab;
    public GameObject tentativeSlotPrefab;

    public RectTransform slotParent;

    protected Dictionary<GameObject, int> instanceIndicies = new Dictionary<GameObject, int>();
    protected List<GameObject> resourcePool = new List<GameObject>();
    protected bool initialized = false;
    protected List<PhraseSequenceElement> words = new List<PhraseSequenceElement>();
    protected int tentativeIndex = -1;

    protected virtual int RowCount {
        get {
            return 5;
        }
    }

    protected virtual int SlotCount {
        get {
            return words.Count;
        }
    }

    protected virtual int StartIndex {
        get {
            return 0;
        }
    }

    protected virtual int EndIndex {
        get {
            return 10;
        }
    }

	// Use this for initialization
	protected void Start () {
        if (!BeforeStart()) {
            return;
        }

        CrystallizeEventManager.UI.OnUpdateUI += HandleUpdateUI;
        CrystallizeEventManager.UI.OnWordClicked += HandleWordClicked;
        CrystallizeEventManager.main.OnLoad += HandleOnLoad;

        if (!initialized) {
            Initialize();
        }
	}

    protected void OnDestroy() {
        BeforeOnDestroy();

        CrystallizeEventManager.UI.OnUpdateUI -= HandleUpdateUI;
        CrystallizeEventManager.UI.OnWordClicked -= HandleWordClicked;
        CrystallizeEventManager.main.OnLoad -= HandleOnLoad;
    }

    protected virtual bool BeforeStart() { return true; }
    protected virtual bool BeforeOnDestroy() { return true; }
    protected virtual void BeforeInitialize() { }
    protected virtual void BeforeRefreshInventory() { }
    protected virtual void BeforeWordReceived(PhraseSequenceElement word) { }

    protected void Initialize() {
        words.Clear();

        foreach (var e in PlayerData.Instance.WordStorage.InventoryElements) {
            if (e == null) {
                words.Add(null);
            } else {
                words.Add(e);
            }
        }

        BeforeInitialize();

        RefreshInventory();

        initialized = true;
    }

    void HandleOnLoad(object sender, System.EventArgs e) {
        Initialize();
    }

    void HandleOnPhraseDropped(object sender, PhraseEventArgs e) {
        if (sender is Component) {
            var comp = (Component)sender;
            var index = instanceIndicies[comp.gameObject];

            int cleared = ClearWord(e.Word);

            if (comp is ExplicitInventorySlotUI && cleared >= 0) {
                var slot = (ExplicitInventorySlotUI)comp;
                words[cleared] = slot.Word;
            }

            while (words.Count <= index) {
                words.Add(null);
            }
            words[index] = e.Word;
            PlayerData.Instance.WordStorage.AddFoundWord(e.Word.WordID);

            UpdatePlayerData();
            RefreshInventory();
            CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);

            if (e.PhraseContainer != null) {
                Destroy(e.PhraseContainer.gameObject);
            }

            // TODO: find a better way to do this?
            /*if (LevelSettings.main.RequireTranslation) {
                //Debug.Log("Dropped: " + cleared);
                if(isNew){
                    var entry = GetEntry(e.Word);
                    CrystallizeEventManager.main.RaiseUIRequest(this, new WordTranslationUIRequestEventArgs(TutorialCanvas.main.gameObject, e.Word, entry, 
                        () => SetWordFound(e.Word), () => RemoveWord(e.Word)));
                }
            }*/
        }
    }

    void HandleUpdateUI(object sender, System.EventArgs e) {
        // TODO: this is a super hack
        if (sender is ObjectiveManager) {
            UpdatePlayerData();
        }

        if (sender != this) {
            Initialize();
        }
    }

    void SetWordFound(PhraseSequenceElement word) {
        PlayerData.Instance.WordStorage.AddFoundWord(word.WordID);
        tentativeIndex = -1;

        RefreshInventory();
        CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);
    }

    protected void RefreshInventory() {
        BeforeRefreshInventory();

        foreach (var r in resourcePool) {
            Destroy(r);
        }
        resourcePool.Clear();
        instanceIndicies.Clear();

        foreach (var w in PlayerData.Instance.WordStorage.ObjectiveWords) {
            //Debug.Log(w);
            if (!ContainsWord(w)) {
                AddWord(new PhraseSequenceElement(w, 0));
            }
        }

        var start = StartIndex;
        var end = EndIndex;
        for (int i = start; i < end; i++) {
            //Debug.Log(start + "; " + end + "; " + i);
            if (i >= words.Count) {
                InsertSlot(i, fillerSlotPrefab, slotParent);
            } else if (i == tentativeIndex) {
                var go = InsertSlot(i, tentativeSlotPrefab, slotParent);
                go.GetComponent<TentativeInventorySlotUI>().Intialize(words[i]);
            } else if (words[i] == null) {
                InsertSlot(i, fillerSlotPrefab, slotParent);
            } else {
                if (PlayerData.Instance.WordStorage.ContainsObjectiveWord(words[i])) {
                    var instance = InsertSlot(i, objectiveSlotPrefab, slotParent);
                    instance.GetComponent<ObjectiveSlotColoring>().Initialize(words[i]);
                    // TODO: we need another class for these types of slots to reject wrong words
                    //instance.GetComponentInChildren<Text>().text = words[i].GetTranslation();
                    //instance.GetComponentInChildren<Text>().color = GUIPallet.main.darkGray;
                } else {
                    var instance = InsertSlot(i, slotPrefab, slotParent);
                    instance.GetComponent<ExplicitInventorySlotUI>().SetWord(words[i]);
                }
            }
        }
    }

    protected void HandleWordClicked(object sender, WordClickedEventArgs e) {
        if (e.Destination == "Inventory") {
            BeforeWordReceived(e.Word);
            RefreshInventory();
        }
    }

    protected bool ContainsWord(int wordID) {
        for (int i = 0; i < SlotCount; i++) {
            if (words[i] != null) {
                if (words[i].WordID == wordID) {
                    return true;
                }
            }
        }
        return false;
    }

    protected void AddWord(PhraseSequenceElement word) {
        for (int i = 0; i < SlotCount; i++) {
            if (words[i] == null) {
                words[i] = word;
                return;
            }
        }
    }

    protected void UpdatePlayerData() {
        PlayerData.Instance.WordStorage.InventoryElements.Clear();
        foreach (var word in words) {
            PlayerData.Instance.WordStorage.InventoryElements.Add(word);
        }

        //TODO: move this somewhere else
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new GameEventArgs(GameEventType.InventoryUpdated));
    }

    GameObject InsertSlot(int index) {
        return InsertSlot(index, slotPrefab, slotParent);
    }

    GameObject InsertSlot(int index, GameObject prefab, RectTransform parent) {
        var instance = GetInstance(prefab);
        instance.transform.SetParent(parent);
        instanceIndicies[instance] = index;
        var dropEvent = instance.GetInterface<IPhraseDropEvent>();
        if (dropEvent != null) {
            dropEvent.OnPhraseDropped += HandleOnPhraseDropped;
        }

        return instance;
    }

    GameObject GetInstance(GameObject prefab) {
        var instance = Instantiate(prefab) as GameObject;
        resourcePool.Add(instance);
        return instance;
    }

    int ClearWord(PhraseSequenceElement word) {
        var i = IndexOf(word);
        if (i >= 0) {
            words[i] = null;
        }
        return i;
    }

    void RemoveWord(PhraseSequenceElement word) {
        tentativeIndex = -1;
        var index = ClearWord(word);
        if (index >= 0) {
            RefreshInventory();
            CrystallizeEventManager.UI.RaiseUpdateUI(this, System.EventArgs.Empty);
        }
    }

    int GetFilledCount() {
        int count = 0;
        for (int i = 0; i < SlotCount; i++) {
            if (words[i] != null) {
                if (!IsObjective(words[i])) {
                    count++;
                }
            }
        }
        return count;
    }

    protected int GetFirstEmptySlot() {
        int i = words.Count;
        while (i > 0) {
            i--;
            if (words[i] == null) {
                continue;
            }

            if (IsObjective(words[i])) {
                continue;
            }

            break;
        }

        int end = i;
        for (int j = 0; j < end; j++) {
            if (words[j] == null) {
                return j;
            }
        }

        return i + 1;
    }

    protected bool IsObjective(PhraseSequenceElement word) {
        return PlayerData.Instance.WordStorage.ContainsObjectiveWord(word);
    }

    protected int IndexOf(PhraseSequenceElement word) {
        if (word == null) {
            return -1;
        }

        for (int i = 0; i < words.Count; i++) {
            if (words[i] == null) {
                continue;
            }

            if (word.GetText() == words[i].GetText()) {
                return i;
            }
        }
        return -1;
    }

}
