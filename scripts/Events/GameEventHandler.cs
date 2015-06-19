using UnityEngine;
using System.Collections;

public class GameEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ConversationSequence.RequestLinearDialogueTurn.SetHandler(LinearDialogueTurnSequence.GetInstance);

        //CrystallizeEventManager.PlayerState.OnCollectPhraseRequested += HandleCollectPhraseRequested;
        //CrystallizeEventManager.PlayerState.OnCollectWordRequested += HandleCollectWordRequested;
        //CrystallizeEventManager.UI.OnDialogueRequested += HandleDialogueRequested;
        //CrystallizeEventManager.UI.OnLinearDialogueTurnRequested += HandleLinearDialogueTurnRequested;
        //CrystallizeEventManager.UI.OnPromptDialogueTurnRequested += HandlePromptDialogueTurnRequested;
        //CrystallizeEventManager.Environment.OnConversationCameraRequested += HandleConversationCameraRequested;
	}

    //void HandleConversationCameraRequested(object sender, SequenceRequestEventArgs<GameObject, object> args) {
    //    var instance = ConversationCameraController.GetInstance(args.Data);
    //    args.SequenceRequest.RaiseCallback(instance);
    //}

    //void HandleLinearDialogueTurnRequested(object sender, SequenceRequestEventArgs<DialogueState, DialogueState> args) {
    //    var instance = new LinearDialogueTurnSequence(args.Data);
    //    args.SequenceRequest.RaiseCallback(instance);
    //}

    //void HandlePromptDialogueTurnRequested(object sender, SequenceRequestEventArgs<DialogueState, DialogueState> args) {
    //    var instance = new PromptDialogueTurnSequence(args.Data);
    //    args.SequenceRequest.RaiseCallback(instance);
    //}

    //void HandleDialogueRequested(object sender, SequenceRequestEventArgs<GameObject, object> args) {
    //    var i = ConversationSequence.GetInstance(args.Data);
    //    args.SequenceRequest.RaiseCallback(i);
    //}

    //void HandleCollectWordRequested(PhraseSequenceElement word) {
    //    if (PlayerData.Instance.WordStorage.ContainsFoundWord(word)) {
    //        PlayerData.Instance.WordStorage.AddFoundWord(word);
            
    //        CrystallizeEventManager.PlayerState.RaiseWordCollected(this, new PhraseEventArgs(word));
    //    }
    //}

    //void HandleCollectPhraseRequested(PhraseSequence phrase) {
    //    if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase)) {
    //        PlayerData.Instance.PhraseStorage.AddPhrase(phrase);
    //        foreach (var word in phrase.PhraseElements) {
    //            HandleCollectWordRequested(word);
    //            //CrystallizeEventManager.PlayerState.RequestCollectWord(word, null);
    //        }

    //        CrystallizeEventManager.PlayerState.RaisePhraseCollected(this, new PhraseEventArgs(phrase));
    //    }
    //}
	
}
