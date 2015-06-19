using UnityEngine;
using System;
using System.Collections;

public class PipeSelectionSequence<I, O> : IProcess<I, O> {

    public O Data { get; set; }

    public event ProcessExitCallback OnReturn;

    public PipeSelectionSequence(O data) {
        Data = data;
    }

    public void Initialize(I args) {
        
    }

    public void Continue() {
        OnReturn.Raise(this, new ProcessExitEventArgs<O>(Data));
    }

    public void ForceExit() {
        
    }

}