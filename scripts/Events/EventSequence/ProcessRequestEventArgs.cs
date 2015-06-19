using UnityEngine; 
using System;
using System.Collections; 

public class ProcessRequestEventArgs<I, O> : EventArgs {

    public I Data { get; set; }
    public ProcessExitCallback<O> Callback { get; set; }
    

    public ProcessRequestEventArgs(I data, ProcessExitCallback<O> callback) {
        Data = data;
        Callback = callback;
    }

}
