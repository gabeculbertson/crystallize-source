using UnityEngine;
using System;
using System.Collections;

public class PromptDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static readonly ProcessFactoryRef<object, PhraseSequence> RequestPhrasePanel = new ProcessFactoryRef<object,PhraseSequence>();

    DialogueState state;

    public event ProcessExitCallback OnExit;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public PromptDialogueTurnSequence() {
        
    }

    public void Initialize(DialogueState data) {
        this.state = data;
        RequestPhrasePanel.Get(null, HandlePhrasePanelExit, this);
    }

    void HandlePhrasePanelExit(object sender, PhraseSequence args) {
        foreach (var id in state.GetElement().NextIDs) {
            if (PhraseSequence.IsPhraseEquivalent(state.Dialogue.GetElement(id).Prompt, args)) {
                PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(args);
                Exit(new ProcessExitEventArgs<DialogueState>(new DialogueState(id, state.Dialogue)));
                return;
            }
        }
        RequestPhrasePanel.Get(null, HandlePhrasePanelExit, this);
    }

    public void ForceExit() {
        Exit(null);
    }

    void Exit(ProcessExitEventArgs<DialogueState> args) {
        OnExit.Raise(this, args);
    }

}