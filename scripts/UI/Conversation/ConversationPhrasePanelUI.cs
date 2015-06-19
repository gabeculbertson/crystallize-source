using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationPhrasePanelUI : UIMonoBehaviour, IProcess<object, PhraseSequence> {

    const string ResourcePath = "UI/ConversationPhrasePanel";

    static ConversationPhrasePanelUI _instance;

    public static ProcessRequestHandler<PhraseSequence, PhraseSequence> RequestReplaceWordPhraseEditor;

    public static ConversationPhrasePanelUI GetInstance(object obj) {
        return GetInstance();
    }

    public static ConversationPhrasePanelUI GetInstance() {
        //if (!_instance) {
            _instance = GameObjectUtil.GetResourceInstance<ConversationPhrasePanelUI>(ResourcePath);
        //}
        //_instance.Initialize();
        return _instance;
    }

    public GameObject phrasePrefab;

    public event ProcessExitCallback<PhraseSequence> OnExit;

    List<GameObject> phraseInstances = new List<GameObject>();
    Coroutine fadeOut;

    public void ForceExit() {
        Exit(null);
    }

    public void Exit(ProcessExitEventArgs<PhraseSequence> args) {
        OnExit.Raise(this, args);
        //FadeOut();
        Destroy(gameObject);
    }

    void Initialize() {
        if (fadeOut != null) {
            StopCoroutine(fadeOut);
            fadeOut = null;
        }
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1f;
    }

	void Start () {
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
        RequestReplaceWordPhraseEditor(sender, new ProcessRequestEventArgs<PhraseSequence, PhraseSequence>(p, HandlePhraseSelection));
    }

    void HandlePhraseSelection(object sender, ProcessExitEventArgs<PhraseSequence> args) {
        Exit(args);
    }

    void HandlePhraseCollected(object sender, PhraseEventArgs e)
    {
        //Initialize();
        Refresh();
    }

    //void FadeOut() {
    //    fadeOut = StartCoroutine(FadeOutCoroutine());
    //}

    //IEnumerator FadeOutCoroutine() {
    //    canvasGroup.interactable = false;
    //    while (canvasGroup.alpha > 0) {
    //        canvasGroup.alpha -= Time.deltaTime;

    //        yield return null;
    //    }

    //    fadeOut = null;
    //}



    public void Initialize(ProcessRequestEventArgs<object, PhraseSequence> args) {
        throw new System.NotImplementedException();
    }
}
