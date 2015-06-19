using UnityEngine;
using System.Collections;

public class GameEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.PlayerState.OnCollectPhraseRequested += HandleCollectPhraseRequested;
        CrystallizeEventManager.PlayerState.OnCollectWordRequested += HandleCollectWordRequested;
	}

    void HandleCollectWordRequested(object sender, SequenceRequestEventArgs<PhraseSequenceElement, PhraseSequenceElement> e) {
        if (PlayerData.Instance.WordStorage.ContainsFoundWord(e.Data)) {
            PlayerData.Instance.WordStorage.AddFoundWord(e.Data);
            
            e.PipeThrough();
            CrystallizeEventManager.PlayerState.RaiseWordCollected(this, new PhraseEventArgs(e.Data));
        }
    }

    void HandleCollectPhraseRequested(object sender, SequenceRequestEventArgs<PhraseSequence, PhraseSequence> e) {
        if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(e.Data)) {
            PlayerData.Instance.PhraseStorage.AddPhrase(e.Data);
            foreach (var word in e.Data.PhraseElements) {
                CrystallizeEventManager.PlayerState.RequestCollectWord(word, null);
            }
            
            e.PipeThrough();
            CrystallizeEventManager.PlayerState.RaisePhraseCollected(this, new PhraseEventArgs(e.Data));
        }
    }
	
}
