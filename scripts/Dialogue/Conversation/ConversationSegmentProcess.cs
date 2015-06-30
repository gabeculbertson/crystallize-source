using UnityEngine;
using System.Collections;

public class ConversationSegmentProcess : IProcess<ConversationArgs, object> {

    DialogueActor actor;
    ContextData context;

    public event ProcessExitCallback OnExit;

    public void Initialize(ConversationArgs args) {
        actor = args.Target.GetComponent<DialogueActor>();
        context = args.Context;

        SetDialogueElement(new DialogueState(0, args.Dialogue, args.Context));
    }

    void SetDialogueElement(DialogueState dialogueState) {
        //actor.SetLine(dialogueState.GetElement().Line, context);
        //Debug.Log(dialogueState.GetElement().NextIDs.Count);
        var e = dialogueState.GetElement();

        if (e is LineDialogueElement) {
            ConversationSequence.RequestLinearDialogueTurn.Get(dialogueState, HandleTurnExit, this);
        } else if (e is BranchDialogueElement) {
            ConversationSequence.RequestPromptDialogueTurn.Get(dialogueState, HandleTurnExit, this);
        } else if (e is AnimationDialogueElement) {
            ConversationSequence.RequestAnimationDialogueTurn.Get(dialogueState, HandleTurnExit, this);
        } else if (e is UIDialogueElement) {
            ConversationSequence.RequestUIDialogueTurn.Get(dialogueState, HandleTurnExit, this);
        } else {
            int id = -1;
            if (e != null) {
                id = e.DefaultNextID;
            }
            HandleTurnExit(null, new DialogueState(id, dialogueState.Dialogue, null));
        }
    }

    void HandleTurnExit(object sender, DialogueState e) {
        if (e.GetElement() == null) {
            Exit();
            return;
        }

        if (e.CurrentID == DialogueSequence.ConfusedExit) {
            var p = new PhraseSequence("(stupid foreigner...)");
            actor.SetPhrase(p);
            ConversationSequence.RequestLinearDialogueTurn.Get(e, HandleTurnExit, this);
            return;
        }

        SetDialogueElement(e);
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}