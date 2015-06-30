using UnityEngine;
using System.Collections;

public class UIDialogueTurnSequence : IProcess<DialogueState, DialogueState> {

    DialogueState state;

    public event ProcessExitCallback OnExit;

    public void Initialize(DialogueState param1) {
        state = param1;
        var e = (UIDialogueElement)param1.GetElement();
        e.SelectItem(UICallback);
    }

    void UICallback(object sender, int args) {
        Exit(new DialogueState(args, state.Dialogue, state.Context));
    }

    public void ForceExit() {
        Exit(new DialogueState(-1, state.Dialogue, state.Context));
    }

    void Exit(DialogueState state) {
        OnExit.Raise(this, state);
    }

}