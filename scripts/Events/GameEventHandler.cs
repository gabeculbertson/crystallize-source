using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class GameEventHandler : MonoBehaviour {

    public static GameEventHandler GetInstance() {
        return new GameObject("GameEventHandler").AddComponent<GameEventHandler>();
    }

	// Use this for initialization
	void Awake () {
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
