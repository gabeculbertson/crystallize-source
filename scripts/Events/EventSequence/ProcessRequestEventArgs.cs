using UnityEngine; 
using System;
using System.Collections;

public class ProcessRequestEventArgs<I, O> : EventArgs {

    public I Data { get; set; }
    public ProcessExitCallback<O> Callback { get; set; }
    public IProcess Parent { get; set; }

    //public ProcessRequestEventArgs(I data, ProcessExitCallback<O> callback) : this(data, callback, null) { }

    public ProcessRequestEventArgs(I data, ProcessExitCallback<O> callback, IProcess parent) {
        Data = data;
        Callback = callback;
        Parent = parent;
    }

}
