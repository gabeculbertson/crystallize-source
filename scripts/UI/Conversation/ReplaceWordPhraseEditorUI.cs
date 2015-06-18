using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReplaceWordPhraseEditorUI : MonoBehaviour, IWindowUI, IInitializable<PhraseSequence> {

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

    PhraseSequence phrase;
    List<GameObject> wordInstances = new List<GameObject>();

    void Start() {
        Refresh();
    }

    void Refresh() {
        UIUtil.GenerateChildren(phrase.PhraseElements, wordInstances, wordParent, GetWordInstance);
        Debug.Log(wordInstances.Count);
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

    void HandleWordClicked(object sender, System.EventArgs e)
    {
        
    }

    public void Confirm()
    {
        CrystallizeEventManager.UI.RaiseModifiedPhraseSelected(this, new PhraseEventArgs(phrase));
    }

}
