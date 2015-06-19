using UnityEngine;
using System;
using System.Collections;

public interface IProcess<I, O> {

    event ProcessExitCallback<O> OnExit;

    void Initialize(ProcessRequestEventArgs<I, O> args);
    void ForceExit();

}
