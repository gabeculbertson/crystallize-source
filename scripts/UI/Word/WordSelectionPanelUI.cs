using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordSelectionPanelUI : MonoBehaviour, IProcess<PhraseSequenceElement, PhraseSequenceElement> {

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

    public event ProcessExitCallback<PhraseSequenceElement> OnExit;

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

    public void ForceExit() {
        Exit(null);
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        Exit(null);
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

        Exit(new ProcessExitEventArgs<PhraseSequenceElement>(buttonWords[(UIButton)sender]));
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            Exit(null);
        }
    }

    public void Exit(ProcessExitEventArgs<PhraseSequenceElement> word) {
        if (IsOpen) {
            OnExit.Raise(this, word);
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        TutorialCanvas.main.UnregisterGameObject("WordSelector");
        CrystallizeEventManager.Environment.OnActorDeparted -= HandleActorDeparted;
    }



    public void Initialize(ProcessRequestEventArgs<PhraseSequenceElement, PhraseSequenceElement> args) {
        throw new NotImplementedException();
    }
}
