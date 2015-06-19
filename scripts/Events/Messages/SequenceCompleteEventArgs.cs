using UnityEngine;
using System;
using System.Collections;

public class ProcessExitEventArgs<T> : EventArgs {

    public T Data { get; set; }

    public ProcessExitEventArgs(T data) {
        Data = data;
    }

}
