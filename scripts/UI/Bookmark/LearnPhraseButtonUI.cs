using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LearnPhraseButtonUI : MonoBehaviour, IPointerClickHandler, IInitializable<PhraseSequence> {

    PhraseSequence phrase;

    void Refresh()
    {
        if (PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase)) {
            GetComponent<Image>().color = Color.black;
        } else {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    public void Initialize(PhraseSequence param1) {
        phrase = param1;
        Refresh();
        CrystallizeEventManager.PlayerState.OnSucceedCollectPhrase += HandleSucceedCollectPhrase;
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnSucceedCollectPhrase -= HandleSucceedCollectPhrase;
    }

    void HandleSucceedCollectPhrase(object sender, PhraseEventArgs e) {
        Refresh();
    }

    public void OnPointerClick(PointerEventData eventData) {
        CrystallizeEventManager.PlayerState.RaiseAttemptCollectPhrase(this, new PhraseEventArgs(phrase));
    }

}
