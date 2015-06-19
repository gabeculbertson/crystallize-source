using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordSelectionPanelUI : MonoBehaviour, ISelectionSequence<PhraseSequenceElement> {

    const string ResourcePath = "UI/WordSelectionPanel";

    public static WordSelectionPanelUI GetInstance() {
        var instance = Instantiate<GameObject>(Resources.Load<GameObject>(ResourcePath));
        var panel = instance.GetComponent<WordSelectionPanelUI>();
        panel.Initialize();
        panel.transform.SetParent(MainCanvas.main.transform, true);
        return panel;
    }

    public Transform wordParent;
    public GameObject wordButtonPrefab;

    IPhraseDropHandler source;
    Dictionary<UIButton, PhraseSequenceElement> buttonWords = new Dictionary<UIButton, PhraseSequenceElement>();

    public event EventHandler OnCancel;
    public event EventHandler OnExit;
    public event SequenceCompleteCallback<PhraseSequenceElement> OnSelection;

    public bool IsOpen {
        get {
            return gameObject;
        }
    }

    public void Initialize() {
        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        foreach (var word in PlayerManager.main.playerData.WordStorage.InventoryElements) {
            if (word == null) {
                continue;
            }

            var instance = Instantiate(wordButtonPrefab) as GameObject;
            instance.transform.SetParent(wordParent);
            instance.GetComponentInChildren<Image>().color = GUIPallet.main.GetColorForWordCategory(word.GetPhraseCategory());
            instance.GetComponentInChildren<Text>().text = word.GetPlayerText();
            instance.GetComponent<UIButton>().OnClicked += HandleClicked;
            buttonWords[instance.GetComponent<UIButton>()] = word;
        }

        TutorialCanvas.main.RegisterGameObject("WordSelector", gameObject);

        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
    }

	public void Initialize (IPhraseDropHandler wordContainer) {
        source = wordContainer;

        Initialize();
	}

    void HandleActorDeparted(object sender, System.EventArgs e) {
        Close();
    }

    void HandleClicked(object sender, System.EventArgs e) {
        if (!(sender is UIButton)) {
            return;
        }

        if (!buttonWords.ContainsKey((UIButton)sender)) {
            return;
        }

        if (source != null) {
            source.AcceptDrop(new WordContainer(buttonWords[(UIButton)sender]));
        }

        OnSelection.Raise(this, new SequenceCompleteEventArgs<PhraseSequenceElement>(buttonWords[(UIButton)sender]));
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (IsOpen) {
                OnCancel.Raise(this, EventArgs.Empty);
                Close();
            }
        }
    }

    public void Close() {
        Destroy(gameObject);
        OnExit.Raise(this, EventArgs.Empty);
    }

    void OnDestroy() {
        TutorialCanvas.main.UnregisterGameObject("WordSelector");
        CrystallizeEventManager.Environment.OnActorDeparted -= HandleActorDeparted;
    }
	
}
