using UnityEngine;
using System;
using System.Collections;

public class SequenceCompleteEventArgs<T> : EventArgs {

    public T Data { get; set; }

    public SequenceCompleteEventArgs(T data) {
        Data = data;
    }

}
