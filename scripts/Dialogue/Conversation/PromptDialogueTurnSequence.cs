using UnityEngine;
using System;
using System.Collections;

public class PromptDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static ProcessRequestHandler<object, PhraseSequence> RequestPhrasePanel;

    public static PromptDialogueTurnSequence GetInstance() {
        return new PromptDialogueTurnSequence();
    }

    DialogueState state;

    public event ProcessExitCallback OnReturn;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public PromptDialogueTurnSequence() {
        
    }

    public void Initialize(DialogueState data) {
        this.state = data;
        RequestPhrasePanel(this, new ProcessRequestEventArgs<object, PhraseSequence>(null, HandlePhrasePanelExit, this));
    }

    void HandlePhrasePanelExit(object sender, ProcessExitEventArgs<PhraseSequence> args) {
        foreach (var id in state.GetElement().NextIDs) {
            //Debug.Log(state.Dialogue.GetElement(id).Prompt);
            //Debug.Log(args.Data);
            if (PhraseSequence.IsPhraseEquivalent(state.Dialogue.GetElement(id).Prompt, args.Data)) {
                PlayerManager.main.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(args.Data);
                Exit(new ProcessExitEventArgs<DialogueState>(new DialogueState(id, state.Dialogue)));
                return;
            }
        }
        RequestPhrasePanel(null, new ProcessRequestEventArgs<object, PhraseSequence>(null, HandlePhrasePanelExit, this));
    }

    void Exit(ProcessExitEventArgs<DialogueState> args) {
        OnReturn.Raise(this, args);
    }

    public void ForceExit() {
        Exit(null);
    }

}