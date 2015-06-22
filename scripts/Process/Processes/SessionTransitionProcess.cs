using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class SessionTransitionProcess : IProcess<string, object> {

    public event ProcessExitCallback OnExit;

    ITemporaryUI<string, object> instance;

    public void Initialize(string param1) {
        instance = UILibrary.SessionTransition.Get(param1);
        instance.Complete += HandleTransitionComplete;
    }

    void HandleTransitionComplete(object sender, EventArgs e) {
        Exit();
    }

    public void ForceExit() {
        instance.Close();
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
