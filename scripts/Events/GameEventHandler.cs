using UnityEngine;
using System.Collections;

public class GameEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.PlayerState.OnAttemptCollectPhrase += HandleAttemptCollectPhrase;
	}

    void HandleAttemptCollectPhrase(object sender, PhraseEventArgs e) {
        if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(e.Phrase)) {
            PlayerData.Instance.PhraseStorage.AddPhrase(e.Phrase);
            CrystallizeEventManager.PlayerState.RaiseSucceedCollectPhrase(this, e);
        }
    }
	
}
