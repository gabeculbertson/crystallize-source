using UnityEngine;
using System;
using System.Collections;

public class FlexibleInteractiveActor : InteractiveDialogActor {

    public event EventHandler<PhraseEventArgs> OnReactToPhrase;

    public override void ReactToPhrase(PhraseSegmentData phrase) {
        if (OnReactToPhrase != null) {
            OnReactToPhrase(this, new PhraseEventArgs(phrase));
        }
    }

}