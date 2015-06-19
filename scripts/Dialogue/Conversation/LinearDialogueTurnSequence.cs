using UnityEngine;
using System;
using System.Collections;

public class LinearDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    public static LinearDialogueTurnSequence GetInstance() {
        return new LinearDialogueTurnSequence();
    }

    DialogueState state;

    public event ProcessExitCallback OnReturn;
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
        OnReturn.Raise(this, GetNextState());
    }

    public void ForceExit() {
        Exit();
    }

}