using UnityEngine;
using System;
using System.Collections;

public class PipeSelectionSequence<I, O> : IProcess<I, O> {

    public O Data { get; set; }

    public event ProcessExitCallback<O> OnExit;

    public PipeSelectionSequence(O data) {
        Data = data;
    }

    public void Continue() {
        OnExit.Raise(this, new ProcessExitEventArgs<O>(Data));
    }

    public void ForceExit() {
        
    }



    public void Initialize(ProcessRequestEventArgs<I, O> args) {
        throw new NotImplementedException();
    }
}