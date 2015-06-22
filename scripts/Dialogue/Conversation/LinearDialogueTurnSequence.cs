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
        CrystallizeEventManager.Input.OnEnvironmentClick += OnEnvironmentClick;
    }

    void OnEnvironmentClick(object sender, EventArgs e) {
        Exit();
    }

    DialogueState GetNextState() {
        if (state.CurrentID == DialogueSequence.ConfusedExit) {
            return null;
        }
        
        if (state.GetElement().NextIDs.Count > 0) {
            return new DialogueState(state.GetElement().NextIDs[0], state.Dialogue);
        } else {
            return null;
        }
    }

    void Exit() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= OnEnvironmentClick;
        OnExit.Raise(this, GetNextState());
    }

    public void ForceExit() {
        Exit();
    }

}