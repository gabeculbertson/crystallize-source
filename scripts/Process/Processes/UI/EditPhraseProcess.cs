using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class EditPhraseProcess : IUIProcess<PhraseSequence, PhraseSequence> {

    //public static 

    public event ProcessExitCallback OnExit;

    public void SetUIInstance(ITemporaryUI<PhraseSequence, PhraseSequence> uiInstance) {
        throw new NotImplementedException();
    }

    public void Initialize(PhraseSequence param1) {
        throw new NotImplementedException();
    }

    public void ForceExit() {
        throw new NotImplementedException();
    }

}
