using UnityEngine;
using System;
using System.Collections;

public class LinearDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    DialogueState state;

    public event ProcessExitCallback OnExit;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public LinearDialogueTurnSequence() {
    }

    public void Initialize(DialogueState data) {
        this.state = data;
        var target = state.GetTarget();
        target.GetComponent<DialogueActor>().SetLine(((LineDialogueElement)data.GetElement()).Line, data.Context);
        CrystallizeEventManager.Input.OnEnvironmentClick += OnEnvironmentClick;
    }

    void OnEnvironmentClick(object sender, EventArgs e) {
        Exit();
    }

    DialogueState GetNextState() {
        if (state.CurrentID == DialogueSequence.ConfusedExit) {
            return null;
        }

        return new DialogueState(state.GetElement().DefaultNextID,state.Dialogue, state.Context);
    }

    void Exit() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= OnEnvironmentClick;
        OnExit.Raise(this, GetNextState());
    }

    public void ForceExit() {
        Exit();
    }

}