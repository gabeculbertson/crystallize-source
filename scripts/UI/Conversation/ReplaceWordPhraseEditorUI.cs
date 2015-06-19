using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReplaceWordPhraseEditorUI : MonoBehaviour, IWindowUI, IInitializable<PhraseSequence>, IProcess<PhraseSequence, PhraseSequence> {

    const string ResourcePath = "UI/ReplaceWordPhraseEditor";

    static GameObject instance;

    static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>(ResourcePath);
        }
    }

    public static ReplaceWordPhraseEditorUI GetInstance()
    {
        return GameObjectUtil.GetResourceInstance<ReplaceWordPhraseEditorUI>(ResourcePath);
    }


    public GameObject wordPrefab;
    public Transform wordParent;

    public event ProcessExitCallback<PhraseSequence> OnExit;

    PhraseSequence phrase;
    List<GameObject> wordInstances = new List<GameObject>();

    int selectedWord = -1;

    public void Close() {
        ForceExit();
    }

    public void ForceExit() {
        Exit(null);
    }

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

    public void Exit(ProcessExitEventArgs<PhraseSequence> args)
    {
        OnExit.Raise(this, args);
        Destroy(gameObject);
    }

    public void Confirm()
    {
        Exit(new ProcessExitEventArgs<PhraseSequence>(phrase));
    }

    void HandleWordClicked(object sender, System.EventArgs e) {
        selectedWord = wordInstances.IndexOf(((Component)sender).gameObject);
        //CrystallizeEventManager.UI.RequestWordSelectionRequested(selectedWord, OnWordSelectionOpened);
    }

    void OnWordSelected(object sender, ProcessExitEventArgs<PhraseSequenceElement> e) {
        if (phrase.PhraseElements.IndexInRange(selectedWord)) {
            phrase.PhraseElements[selectedWord] = e.Data;
        }
        Refresh();
    }



    public void Initialize(ProcessRequestEventArgs<PhraseSequence, PhraseSequence> args) {
        throw new NotImplementedException();
    }
}
