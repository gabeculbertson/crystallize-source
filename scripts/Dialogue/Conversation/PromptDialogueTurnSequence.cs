using UnityEngine;
using System;
using System.Collections;

public class PromptDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static readonly ProcessFactoryRef<object, PhraseSequence> RequestPhrasePanel = new ProcessFactoryRef<object,PhraseSequence>();

    DialogueState state;
    DialogueState nextState;

    public event ProcessExitCallback OnExit;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public PromptDialogueTurnSequence() {
        
    }

    public void Initialize(DialogueState data) {
        this.state = data;
        RequestPhrasePanel.Get(null, HandlePhrasePanelExit, this);
    }

    void HandlePhrasePanelExit(object sender, PhraseSequence args) {
        Debug.Log("[" + args.GetText() + "]" + (args.GetText().Trim() == "?"));
        if (args.GetText().Trim() == "?") {
            PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(args);
            Exit(new DialogueState(DialogueSequence.ConfusedExit, state.Dialogue, state.Context));
            return;
        }

        var promptElement = (BranchDialogueElement)state.GetElement();
        foreach (var link in promptElement.Branches) {
            if (PhraseSequence.IsPhraseEquivalent(link.Prompt, args)) {
                PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(args);
                nextState = new DialogueState(link.NextID, state.Dialogue, state.Context);
                var pos = UILibrary.PositiveFeedback.Get("");
                pos.Complete += pos_Complete;
                return;
            }
        }
        var neg = UILibrary.NegativeFeedback.Get("");
        neg.Complete += neg_Complete;
    }

    void pos_Complete(object sender, EventArgs<object> e) {
        Exit(nextState);
    }

    void neg_Complete(object sender, EventArgs<object> e) {
        RequestPhrasePanel.Get(null, HandlePhrasePanelExit, this);
    }

    public void ForceExit() {
        Exit(null);
    }

    void Exit(DialogueState args) {
        OnExit.Raise(this, args);
    }

}