﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class UIProcess<I, O> : IProcess<I, O> {

    UIFactoryRef<I,O> factory;
    ITemporaryUI<I,O> instance;

    public UIProcess(UIFactoryRef<I,O> getInstance) {
        this.factory = getInstance;
    }

    public event ProcessExitCallback OnExit;

    public void Initialize(I param1) {
        instance = factory.Get(param1);
        instance.Complete += HandleComplete;
    }

    public void ForceExit() {
        Exit(default(O));
    }

    void HandleComplete(object sender, EventArgs e) {
        Exit(default(O));
    }

    void Exit(O obj) {
        OnExit.Raise(this, obj);
    }

}
