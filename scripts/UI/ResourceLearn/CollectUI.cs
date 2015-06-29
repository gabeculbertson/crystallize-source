using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectUI : MonoBehaviour {

    const string ResourcePath = "UI/Collect";
    public static CollectUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<CollectUI>(ResourcePath);
    }

    public GameObject phrasePrefab;
    public GameObject wordPrefab;
    public RectTransform collectionParent;
    public Text remainingCountText;

    void Start() {
        transform.SetParent(MainCanvas.main.transform, false);

        CrystallizeEventManager.PlayerState.OnWordCollected += PlayerState_OnWordCollected;
        CrystallizeEventManager.PlayerState.OnPhraseCollected += PlayerState_OnPhraseCollected;
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnWordCollected -= PlayerState_OnWordCollected;
        CrystallizeEventManager.PlayerState.OnPhraseCollected -= PlayerState_OnPhraseCollected;
    }

    void PlayerState_OnWordCollected(object sender, PhraseEventArgs e) {
        AddWord(e.Word);
    }

    void PlayerState_OnPhraseCollected(object sender, PhraseEventArgs e) {
        AddPhrase(e.Phrase);
    }

    void Refresh() {
        remainingCountText.text = string.Format("Words [{0}/{1}]\t\tPhrases [{2}/{3}]",
            ResourceLearnEventHandler.GetWords(), PlayerData.Instance.Proficiency.Words,
            ResourceLearnEventHandler.GetPhrases(), PlayerData.Instance.Proficiency.Phrases);
    }

    public void AddWord(PhraseSequenceElement word) {
        var instance = Instantiate<GameObject>(wordPrefab);
        instance.GetComponent<CollectedWordUI>().Initialize(word);
        instance.transform.SetParent(collectionParent, false);
        Refresh();
    }

    public void AddPhrase(PhraseSequence phrase) {
        var instance = Instantiate<GameObject>(phrasePrefab);
        instance.GetComponent<CollectedPhraseUI>().Initialize(phrase);
        instance.transform.SetParent(collectionParent, false);
        Refresh();
    }

}