using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReplaceWordPhraseEditorUI : MonoBehaviour, IWindowUI, ITemporaryUI<PhraseSequence, PhraseSequence> {

    const string ResourcePath = "UI/ReplaceWordPhraseEditor";

    public static ReplaceWordPhraseEditorUI GetInstance()
    {
        return GameObjectUtil.GetResourceInstance<ReplaceWordPhraseEditorUI>(ResourcePath);
    }

    public static ProcessFactoryRef<PhraseSequenceElement, PhraseSequenceElement> RequestWordSelection;


    public GameObject wordPrefab;
    public Transform wordParent;

    public event EventHandler<EventArgs<PhraseSequence>> Complete;

    PhraseSequence phrase;
    List<GameObject> wordInstances = new List<GameObject>();

    int selectedWord = -1;

    public void Initialize(PhraseSequence phrase) {
        this.phrase = new PhraseSequence(phrase);
        transform.SetParent(MainCanvas.main.transform, false);
        transform.position = new Vector2(Screen.width * 0.5f, 300f);
    }

    public void Close() {
        Exit(null);
    }

    void Start() {
        Refresh();
    }

    void Refresh() {
        UIUtil.GenerateChildren(phrase.PhraseElements, wordInstances, wordParent, GetWordInstance);
        //Debug.Log(wordInstances.Count);
    }

    GameObject GetWordInstance(PhraseSequenceElement word)
    {
        var wordInstance = Instantiate<GameObject>(wordPrefab);
        wordInstance.GetComponent<GenericSpeechBubbleWordUI>().Initialize(word);
        wordInstance.GetComponent<GenericSpeechBubbleWordUI>().OnClicked += HandleWordClicked;
        //wordInstance.transform.SetParent(wordParent);
        return wordInstance;
    }

    void Exit(EventArgs<PhraseSequence> args)
    {
        Complete.Raise(this, args);
        Destroy(gameObject);
    }

    public void Confirm()
    {
        Exit(new EventArgs<PhraseSequence>(phrase));
    }

    void HandleWordClicked(object sender, System.EventArgs e) {
        selectedWord = wordInstances.IndexOf(((Component)sender).gameObject);
        var word = phrase.PhraseElements[selectedWord];

        RequestWordSelection.Get(word, OnWordSelected, null);// this);
    }

    void OnWordSelected(object sender, PhraseSequenceElement e) {
        if (phrase.PhraseElements.IndexInRange(selectedWord)) {
            phrase.PhraseElements[selectedWord] = e;
        }
        Refresh();
    }

}
