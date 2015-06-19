using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReplaceWordPhraseEditorUI : MonoBehaviour, IWindowUI, IInitializable<PhraseSequence>, IProcess<PhraseSequence, PhraseSequence> {

    const string ResourcePath = "UI/ReplaceWordPhraseEditor";

    public static ReplaceWordPhraseEditorUI GetInstance()
    {
        return GameObjectUtil.GetResourceInstance<ReplaceWordPhraseEditorUI>(ResourcePath);
    }

    public static ProcessRequestHandler<PhraseSequenceElement, PhraseSequenceElement> RequestWordSelection;


    public GameObject wordPrefab;
    public Transform wordParent;

    public event ProcessExitCallback OnReturn;

    PhraseSequence phrase;
    List<GameObject> wordInstances = new List<GameObject>();

    int selectedWord = -1;

    public void Initialize(PhraseSequence phrase) {
        this.phrase = new PhraseSequence(phrase);
        //Debug.Log(this.phrase.GetText());
        transform.SetParent(MainCanvas.main.transform, false);
    }

    public void Close() {
        ForceExit();
    }

    public void ForceExit() {
        Debug.Log("Exit forced.");
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

    public void Exit(ProcessExitEventArgs<PhraseSequence> args)
    {
        //Debug.Log(args + StackTraceUtility.ExtractStackTrace());
        OnReturn.Raise(this, args);
        Destroy(gameObject);
    }

    public void Confirm()
    {
        Exit(new ProcessExitEventArgs<PhraseSequence>(phrase));
    }

    void HandleWordClicked(object sender, System.EventArgs e) {
        selectedWord = wordInstances.IndexOf(((Component)sender).gameObject);
        var word = phrase.PhraseElements[selectedWord];

        RequestWordSelection(this, new ProcessRequestEventArgs<PhraseSequenceElement, PhraseSequenceElement>(word, OnWordSelected, this));
        //CrystallizeEventManager.UI.RequestWordSelectionRequested(selectedWord, OnWordSelectionOpened);
    }

    void OnWordSelected(object sender, ProcessExitEventArgs<PhraseSequenceElement> e) {
        if (phrase.PhraseElements.IndexInRange(selectedWord)) {
            phrase.PhraseElements[selectedWord] = e.Data;
        }
        Refresh();
    }

}
