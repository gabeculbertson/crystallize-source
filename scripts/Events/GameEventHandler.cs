using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class GameEventHandler : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Process.Connect<GameObject, object>(ref ExploreSceneSequence.RequestDialogue, ConversationSequence.GetInstance);
        Process.Connect<GameObject, object>(ref ConversationSequence.RequestConversationCamera, ConversationCameraController.GetInstance);
        Process.Connect<DialogueState, DialogueState>(ref ConversationSequence.RequestLinearDialogueTurn, LinearDialogueTurnSequence.GetInstance);
        Process.Connect<DialogueState, DialogueState>(ref ConversationSequence.RequestPromptDialogueTurn, PromptDialogueTurnSequence.GetInstance);
        Process.Connect<object, PhraseSequence>(ref PromptDialogueTurnSequence.RequestPhrasePanel, ConversationPhrasePanelUI.GetInstance);
        Process.Connect<PhraseSequence, PhraseSequence>(ref ConversationPhrasePanelUI.RequestReplaceWordPhraseEditor, ReplaceWordPhraseEditorUI.GetInstance);
        Process.Connect<PhraseSequenceElement, PhraseSequenceElement>(ref ReplaceWordPhraseEditorUI.RequestWordSelection, WordSelectionPanelUI.GetInstance);
 
        CrystallizeEventManager.PlayerState.OnCollectWordRequested += HandleCollectWordRequested;
        CrystallizeEventManager.PlayerState.OnCollectPhraseRequested += HandleCollectPhraseRequested;
	}

    //ProcessRequestHandler<I, O> Handler<I, O>(GetProcessInstance<I, O> getProcess) {
    //    return (s, e) => ProcessRequestHandler(getProcess, s, e);
    //}

    //void ProcessRequestHandler<I, O>(GetProcessInstance<I, O> getProcessInstance, object sender, ProcessRequestEventArgs<I, O> args) {
    //    var process = getProcessInstance();

    //    if (args.Parent != null) {
    //        ProcessExitCallback forceChildExit = (s, e) => process.ForceExit();
    //        ProcessExitCallback detachChild = (s, e) => {
    //            args.Parent.OnReturn -= forceChildExit;
    //            Debug.Log("Detached child: " + process);
    //        };
    //        process.OnReturn += detachChild;
    //        args.Parent.OnReturn += forceChildExit;
    //    }

    //    ProcessExitCallback castCallback = (s, e) => args.Callback(s, (ProcessExitEventArgs<O>)e);
    //    process.OnReturn += castCallback;

    //    Debug.Log("Started: " + process);
    //    process.OnReturn += (s, e) => Debug.Log("Ended: " + process);

    //    process.Initialize(args.Data);
    //}

    public void HandleCollectWordRequested(object sender, PhraseEventArgs args) {
        var word = args.Word;
        if (PlayerData.Instance.WordStorage.ContainsFoundWord(word)) {
            PlayerData.Instance.WordStorage.AddFoundWord(word);

            CrystallizeEventManager.PlayerState.RaiseWordCollected(this, new PhraseEventArgs(word));
        }
    }

    void HandleCollectPhraseRequested(object sender, PhraseEventArgs args) {
        var phrase = args.Phrase;
        if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase)) {
            PlayerData.Instance.PhraseStorage.AddPhrase(phrase);
            foreach (var word in phrase.PhraseElements) {
                CrystallizeEventManager.PlayerState.RaiseCollectWordRequested(null, new PhraseEventArgs(word));
            }

            CrystallizeEventManager.PlayerState.RaisePhraseCollected(this, new PhraseEventArgs(phrase));
        }
    }
	
}
