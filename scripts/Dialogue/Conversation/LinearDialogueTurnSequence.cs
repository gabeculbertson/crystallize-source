using UnityEngine;
using System;
using System.Collections;

public class LinearDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static LinearDialogueTurnSequence GetInstance(DialogueState state) {
        return new LinearDialogueTurnSequence(state);
    }

    DialogueState state;

    public event ProcessExitCallback<DialogueState> OnExit;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public LinearDialogueTurnSequence(DialogueState state) {
        this.state = state;
        CrystallizeEventManager.Input.OnEnvironmentClick += OnEnvironmentClick;
    }

    void OnEnvironmentClick(object sender, EventArgs e) {
        Exit();
    }

    ProcessExitEventArgs<DialogueState> GetNextState() {
        if (state.GetElement().NextIDs.Count > 0) {
            return new ProcessExitEventArgs<DialogueState>(
                new DialogueState(state.GetElement().NextIDs[0], state.Dialogue));
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



    public void Initialize(ProcessRequestEventArgs<DialogueState, DialogueState> args) {
        throw new NotImplementedException();
    }
}