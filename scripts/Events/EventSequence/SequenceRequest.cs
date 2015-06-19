using UnityEngine;
using System;
using System.Collections;

public class SequenceRequest<O> {

    public event SequenceRequestCallback<O> OnCallback;
    public void RaiseCallback(ISelectionSequence<O> args) {
        OnCallback.Raise(this, new SequenceCallbackEventArgs<O>(args));
    }

}