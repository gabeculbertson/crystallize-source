using UnityEngine;
using System.Collections;

public class ResourceLearnEventHandler : MonoBehaviour {

    static ResourceLearnEventHandler _instance;
    public static void GetInstance() {
        _instance = new GameObject("CollectEventHandler").AddComponent<ResourceLearnEventHandler>();
    }

    public static int GetWords(){
        if (_instance) {
            return _instance.collectedWordCount;
        }
        return 0;
    }

    public static int GetPhrases(){
        if (_instance) {
            return _instance.collectedPhraseCount;
        }
        return 0;
    }

    int collectedWordCount = 0;
    int collectedPhraseCount = 0;

    void Start() {
        CrystallizeEventManager.PlayerState.OnCollectWordRequested += PlayerState_OnCollectWordRequested;
        CrystallizeEventManager.PlayerState.OnCollectPhraseRequested += PlayerState_OnCollectPhraseRequested;
        CrystallizeEventManager.UI.OnWordClicked += UI_OnWordClicked;
    }

    void UI_OnWordClicked(object sender, WordClickedEventArgs e) {
        //CrystallizeEventManager.main
        PlayerState_OnCollectWordRequested(sender, new PhraseEventArgs(e.Word));
    }

    void PlayerState_OnCollectPhraseRequested(object sender, PhraseEventArgs e) {
        if (collectedPhraseCount < PlayerData.Instance.Proficiency.Phrases) {
            if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(e.Phrase)) {
                collectedPhraseCount++;
                PlayerDataConnector.CollectPhrase(e.Phrase);
            }
        } else {
            UILibrary.MessageBox.Get("You can't collect more phrases today. You can collect more phrases in a session by reviewing phrases to level up.");
        }
    }

    void PlayerState_OnCollectWordRequested(object sender, PhraseEventArgs e) {
        if (collectedWordCount < PlayerData.Instance.Proficiency.Words) {
            if (!PlayerData.Instance.WordStorage.ContainsFoundWord(e.Word)) {
                collectedWordCount++;
                PlayerDataConnector.CollectWord(e.Word);
            }
        } else {
            UILibrary.MessageBox.Get("You can't collect more words today. You can collect more words in a session by reviewing words to level up.");
        }
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnCollectWordRequested -= PlayerState_OnCollectWordRequested;
        CrystallizeEventManager.PlayerState.OnCollectPhraseRequested -= PlayerState_OnCollectPhraseRequested;
    }

}