using UnityEngine;
using System;
using System.Collections;

public class PromptDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static ProcessRequestHandler<object, PhraseSequence> RequestPhrasePanel;

    DialogueState state;

    public event ProcessExitCallback<DialogueState> OnExit;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public PromptDialogueTurnSequence(DialogueState state) {
        this.state = state;
        RequestPhrasePanel(this, new ProcessRequestEventArgs<object, PhraseSequence>(null, HandlePhrasePanelExit));
    }

    void HandlePhrasePanelExit(object sender, ProcessExitEventArgs<PhraseSequence> args) {
        foreach (var id in state.GetElement().NextIDs) {
            if (PhraseSequence.IsPhraseEquivalent(state.Dialogue.GetElement(id).Prompt, args.Data)) {
                PlayerManager.main.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(args.Data);
                Exit(new ProcessExitEventArgs<DialogueState>(new DialogueState(id, state.Dialogue)));
                return;
            }
        }
        RequestPhrasePanel(null, new ProcessRequestEventArgs<object, PhraseSequence>(null, HandlePhrasePanelExit));
    }

    void Exit(ProcessExitEventArgs<DialogueState> args) {
        OnExit.Raise(this, args);
    }

    public void ForceExit() {
        Exit(null);
    }



    public void Initialize(ProcessRequestEventArgs<DialogueState, DialogueState> args) {
        throw new NotImplementedException();
    }
}