using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class PromptDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static readonly ProcessFactoryRef<BranchDialogueElement, PhraseSequence> RequestPhrasePanel = new ProcessFactoryRef<BranchDialogueElement, PhraseSequence>();

    DialogueState state;
    DialogueState nextState;

    public event ProcessExitCallback OnExit;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public void Initialize(DialogueState data) {
        this.state = data;
        
        var phrases = ((BranchDialogueElement)data.GetElement()).Branches.Select((b) => b.Prompt).ToList();
        phrases.Add(new PhraseSequence("?"));
        var ui = UILibrary.PhraseSelector.Get(phrases);
        ui.Complete += ui_Complete;
        //RequestPhrasePanel.Get(null, HandlePhrasePanelExit, this);
    }

    void ui_Complete(object sender, EventArgs<PhraseSequence> e) {
        HandlePhrasePanelExit(sender, e.Data);
    }

    void HandlePhrasePanelExit(object sender, PhraseSequence args) {
        //Debug.Log("[" + args.GetText() + "]" + (args.GetText().Trim() == "?"));
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
                //var pos = UILibrary.PositiveFeedback.Get("");
                //pos.Complete += pos_Complete;
                Exit(nextState);
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