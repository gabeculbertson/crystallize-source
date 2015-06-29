using UnityEngine;
using System.Collections;

public class DialogueAnimationProcess : IProcess<DialogueState, DialogueState> {

    DialogueState state;

    public event ProcessExitCallback OnExit;

    public void ForceExit() {
        Exit();
    }

    public void Initialize(DialogueState param1) {
        state = param1;
        var anim = ((AnimationDialogueElement)state.GetElement()).Animation;
        anim.OnComplete += anim_OnComplete;
        anim.Play(state.GetTarget());
    }

    void anim_OnComplete(object sender, System.EventArgs e) {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, new DialogueState(state.GetElement().DefaultNextID, state.Dialogue, state.Context));
    }

}