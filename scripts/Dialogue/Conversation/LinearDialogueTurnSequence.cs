using UnityEngine;
using System;
using System.Collections;

public class LinearDialogueTurnSequence : ISelectionSequence<DialogueState> {

    DialogueState state;

    public event EventHandler OnCancel;
    public event EventHandler OnExit;
    public event SequenceCompleteCallback<DialogueState> OnSelection;
    public event EventHandler<PhraseEventArgs> OnPhraseRequested;

    public LinearDialogueTurnSequence(DialogueState state) {
        this.state = state;
        CrystallizeEventManager.Input.OnEnvironmentClick += OnEnvironmentClick;
    }

    void OnEnvironmentClick(object sender, EventArgs e) {
        OnSelection.Raise(this, new SequenceCompleteEventArgs<DialogueState>(GetNextState()));
    }

    DialogueState GetNextState() {
        return new DialogueState(state.GetElement().NextIDs[0], state.Dialogue);
    }

}