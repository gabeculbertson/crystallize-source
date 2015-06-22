using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationPhrasePanelUI : UIPanel, IPanelItemSelector<PhraseSequence> {

    const string ResourcePath = "UI/ConversationPhrasePanel";

    static ConversationPhrasePanelUI _instance;

    public static ConversationPhrasePanelUI GetInstance() {
        if (!_instance) {
            _instance = GameObjectUtil.GetResourceInstance<ConversationPhrasePanelUI>(ResourcePath);
        }
        _instance.Initialize();
        return _instance;
    }

    public GameObject phrasePrefab;

    public event EventHandler<EventArgs<PhraseSequence>> OnItemSelected;

    List<GameObject> phraseInstances = new List<GameObject>();

    void Initialize() {
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1f;
    }

    public void Initialize(object args) {
        Initialize();
    }

    void Start() {
        transform.SetParent(MainCanvas.main.transform);
        transform.position = new Vector2(Screen.width * .5f, 80f);
        CrystallizeEventManager.PlayerState.OnPhraseCollected += HandlePhraseCollected;
        Refresh();
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnPhraseCollected -= HandlePhraseCollected;
    }

    void Refresh()
    {
        UIUtil.GenerateChildren(PlayerData.Instance.PhraseStorage.Phrases, phraseInstances, transform, GetPhraseInstance);
    }

    void HandlePhraseCollected(object sender, PhraseEventArgs e) {
        Refresh();
    }

    GameObject GetPhraseInstance(PhraseSequence phrase)
    {
        var instance = Instantiate<GameObject>(phrasePrefab);
        instance.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
        instance.GetComponent<UIButton>().OnClicked += HandlePhraseClicked;
        return instance;
    }

    void HandlePhraseClicked(object sender, System.EventArgs e)
    {
        var c = (Component)sender;
        var p = c.gameObject.GetInterface<IPhraseContainer>().Phrase;
        OnItemSelected.Raise(this, new EventArgs<PhraseSequence>(p));
        //RequestReplaceWordPhraseEditor(sender, new ProcessRequestEventArgs<PhraseSequence, PhraseSequence>(p, HandlePhraseSelection, this));
    }

}
