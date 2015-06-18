using UnityEngine;
using System;
using System.Collections;

public class PipeSelectionSequence<O> : ISelectionSequence<O> {

    public O Data { get; set; }

    public event EventHandler OnCancel;
    public event EventHandler OnExit;
    public event SequenceCompleteCallback<O> OnSelection;

    public PipeSelectionSequence(O data) {
        Data = data;
    }

    public void Continue() {
        OnSelection.Raise(this, new SequenceCompleteEventArgs<O>(Data));
    }

}