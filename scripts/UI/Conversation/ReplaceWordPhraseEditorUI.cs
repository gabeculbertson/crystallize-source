using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReplaceWordPhraseEditorUI : MonoBehaviour, IWindowUI, IInitializable<PhraseSequence>, ISelectionSequence<PhraseSequence> {

    const string ResourcePath = "UI/ReplaceWordPhraseEditor";

    static GameObject instance;

    public static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>(ResourcePath);
        }
    }

    public static ReplaceWordPhraseEditorUI GetInstance()
    {
        if (instance)
        {
            Destroy(instance);
        }

        instance = Instantiate<GameObject>(Prefab);
        return instance.GetComponent<ReplaceWordPhraseEditorUI>();
    }


    public GameObject wordPrefab;
    public Transform wordParent;

    public event EventHandler OnCancel;
    public event EventHandler OnExit;
    public event SequenceCompleteCallback<PhraseSequence> OnSelection;

    PhraseSequence phrase;
    List<GameObject> wordInstances = new List<GameObject>();

    int selectedWord = -1;

    void Start() {
        Refresh();
    }

    void Refresh() {
        UIUtil.GenerateChildren(phrase.PhraseElements, wordInstances, wordParent, GetWordInstance);
        //Debug.Log(wordInstances.Count);
    }

    public void Initialize(PhraseSequence phrase)
    {
        this.phrase = new PhraseSequence(phrase);
        //Debug.Log(this.phrase.GetText());
        transform.SetParent(MainCanvas.main.transform, false);
    }

    GameObject GetWordInstance(PhraseSequenceElement word)
    {
        var wordInstance = Instantiate<GameObject>(wordPrefab);
        wordInstance.GetComponent<GenericSpeechBubbleWordUI>().Initialize(word);
        wordInstance.GetComponent<GenericSpeechBubbleWordUI>().OnClicked += HandleWordClicked;
        //wordInstance.transform.SetParent(wordParent);
        return wordInstance;
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void Confirm()
    {
        OnSelection.Raise(this, new SequenceCompleteEventArgs<PhraseSequence>(phrase));
        //CrystallizeEventManager.UI.RaiseModifiedPhraseSelected(this, new PhraseEventArgs(phrase));
    }

    void HandleWordClicked(object sender, System.EventArgs e) {
        selectedWord = wordInstances.IndexOf(((Component)sender).gameObject);
        CrystallizeEventManager.UI.RequestWordSelectionRequested(selectedWord, OnWordSelectionOpened);
    }

    void OnWordSelectionOpened(object sender, SequenceCallbackEventArgs<PhraseSequenceElement> e) {
        e.Sequence.OnSelection += OnWordSelected;
    }

    void OnWordSelected(object sender, SequenceCompleteEventArgs<PhraseSequenceElement> e) {
        if (phrase.PhraseElements.IndexInRange(selectedWord)) {
            phrase.PhraseElements[selectedWord] = e.Data;
        }
        Refresh();
    }

}
