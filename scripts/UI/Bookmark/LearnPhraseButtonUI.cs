using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LearnPhraseButtonUI : MonoBehaviour, IPointerClickHandler, IInitializable<PhraseSequence> {

    PhraseSequence phrase;

    void Refresh()
    {
        if (PlayerDataConnector.ContainsLearnedItem(phrase)) {
            GetComponent<Image>().color = Color.gray;
        } else {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    public void Initialize(PhraseSequence param1) {
        phrase = param1;
        Refresh();
        CrystallizeEventManager.PlayerState.OnPhraseCollected += HandleSucceedCollectPhrase;
        CrystallizeEventManager.PlayerState.OnWordCollected += PlayerState_OnWordCollected;
    }

    void PlayerState_OnWordCollected(object sender, PhraseEventArgs e) {
        Refresh();
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnPhraseCollected -= HandleSucceedCollectPhrase;
        CrystallizeEventManager.PlayerState.OnWordCollected -= PlayerState_OnWordCollected;
    }

    void HandleSucceedCollectPhrase(object sender, PhraseEventArgs e) {
        Refresh();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (phrase.PhraseElements.Count == 1) {
            CrystallizeEventManager.PlayerState.RaiseCollectWordRequested(this, new PhraseEventArgs(phrase));
        } else {
            CrystallizeEventManager.PlayerState.RaiseCollectPhraseRequested(this, new PhraseEventArgs(phrase));
        }
        //CrystallizeEventManager.PlayerState.RequestCollectPhrase(phrase, null);
    }

}
