using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewProcess : IProcess<object, int> {

    public event ProcessExitCallback OnExit;

    ITemporaryUI<object, int> ui;

    public void Initialize(object param1) {
        ui = UILibrary.Review.Get(null);
        ui.Complete += ui_Complete;
        ui.Initialize(null);
    }

    void ui_Complete(object sender, EventArgs<int> e) {
        Exit(e.Data);
    }

    public void ForceExit() {
        Exit(0);
    }

    void Exit(int count) {
        OnExit.Raise(this, count);
    }

}
