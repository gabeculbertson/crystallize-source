using UnityEngine;
using System;
using System.Collections;

public class SequenceCallbackEventArgs<T> : EventArgs {

    public ISelectionSequence<T> Sequence { get; set; }

    public SequenceCallbackEventArgs(ISelectionSequence<T> sequence) {
        Sequence = sequence;
    }

}