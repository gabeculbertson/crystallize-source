using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragDropFreeInputUI : MonoBehaviour {

    public GameObject emptySpacePrefab;
    public GameObject elementPrefab;
    public RectTransform elementParent;

    PhraseSequence phrase = new PhraseSequence();
    List<GameObject> elementInstances = new List<GameObject>();

    public bool OpenedManually { get; set; }

    public bool IsOpen {
        get {
            return gameObject.activeSelf;
        }
    }

    public PhraseSequence Phrase {
        get {
            return phrase;
        }
    }

	// Use this for initialization
	void Start () {
        TutorialCanvas.main.RegisterGameObject("FreeInput", gameObject);

        if (LevelSettings.main) {
            if (!LevelSettings.main.allowFreeInputUI) {
                Destroy(gameObject);
                return;
            }
        }

        UpdateElements();

        UISystem.main.OnInputEvent += HandleInputEvent;
        CrystallizeEventManager.UI.OnBeginDragWord += HandleOnBeginDrag;
        CrystallizeEventManager.UI.OnDropWord += HandleOnDrop;
        CrystallizeEventManager.UI.OnWordClicked += HandleWordClicked;

        // TODO: this is really bad, should be with all the other UI requests
        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;

        if (GetComponentInChildren<DragDropFreeInputTextInputUI>()) {
            GetComponentInChildren<DragDropFreeInputTextInputUI>().OnElementChosen += HandleOnElementChosen;
        }

        Close();
    }

    void HandleWordClicked(object sender, WordClickedEventArgs e) {
        if (e.Destination == "Say") {
            if (!UISystem.main.ContainsCenterPanel() || IsOpen) {
                Open();
                OpenedManually = true;
                phrase.PhraseElements.Add(e.Word);
                UpdateElements();
            }
        }
    }

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is DragDropFreeInputUIRequestEventArgs) {
            if (UISystem.main.Mode == UIMode.Exploring) {
                var fie = (DragDropFreeInputUIRequestEventArgs)e;
                Open();
                phrase = fie.Phrase;
                UpdateElements();
            }
        }
    }

    void HandleInputEvent(object sender, UIInputEventArgs e) {
        if (e.KeyCode == KeyCode.Return){
            if (IsOpen) {
                EnterPhrase();
            } else {
                /*if (!UISystem.main.ContainsCenterPanel()) {
                    Open();
                    OpenedManually = true;
                }*/
            }
        }
    }

    void HandleOnElementChosen(object sender, PhraseEventArgs e) {
        phrase.PhraseElements.Add(e.Word);
        UpdateElements();
    }

    void HandleOnBeginDrag(object sender, PhraseEventArgs e) {
        if (!UISystem.main.ContainsCenterPanel() && !IsOpen) {
            Open();
            OpenedManually = false;
        }
    }

    void HandleOnDrop(object sender, PhraseEventArgs e) {
        if (!OpenedManually && phrase.GetElements().Count == 0) {
            Close();
        }
    }

    void ClearElements() {
        foreach (var e in elementInstances) {
            Destroy(e);
        }
        elementInstances.Clear();
    }

    void UpdateElements() {
        ClearElements();

        if (phrase.GetElements().Count == 0) {
            AddEmptyElement(0, 80f, 160f);
        } else {
            AddEmptyElement(0, 0, 64f);

            int count = 0;
            foreach (var ele in phrase.GetElements()) {
                AddElement(count, ele);
                count++;
                AddEmptyElement(count, 0, 64f);
            }
        }
    }

    void AddElement(int index, PhraseSequenceElement element) {
        var go = Instantiate(elementPrefab) as GameObject;
        go.transform.SetParent(elementParent);
        elementInstances.Add(go);

        go.GetComponent<DragDropFreeInputElementUI>().Initialize(index, element);
        go.GetComponent<DragDropFreeInputElementUI>().OnClicked += HandleOnClicked;
        go.GetComponent<DragDropFreeInputElementUI>().OnDragStarted += HandleOnClicked;
    }

    void HandleOnClicked(object sender, System.EventArgs e) {
        var ele = (DragDropFreeInputElementUI)sender;
        phrase.RemoveAt(ele.Index);

        UpdateElements();
    }

    void AddEmptyElement(int index, float restingWidth, float openWidth) {
        var go = Instantiate(emptySpacePrefab) as GameObject;
        go.transform.SetParent(elementParent);
        elementInstances.Add(go);

        go.GetComponent<DragDropFreeInputEmptyUI>().Initialize(index, restingWidth, openWidth);
        go.GetComponent<DragDropFreeInputEmptyUI>().OnPhraseDropped += HandleOnPhraseDropped;
    }

    void HandleOnPhraseDropped(object sender, PhraseEventArgs args) {
        var c = (DragDropFreeInputEmptyUI)sender;
        PhraseSequenceElement e = args.Word;
        
        phrase.PhraseElements.Insert(c.Index, e);

        UpdateElements();
    }

    public void EnterPhrase() {
        PlayerManager.main.PlayerGameObject.GetComponent<Crystallize.DialogueActor>().SetPhrase(phrase);
        Close();
    }

    public void Open() {
        UISystem.main.AddCenterPanel(this);
        gameObject.SetActive(true);
        UpdateElements();
    }

    public void Close() {
        UISystem.main.RemoveCenterPanel(this);
        ClearElements();
        phrase = new PhraseSequence();
        gameObject.SetActive(false);
    }

}
