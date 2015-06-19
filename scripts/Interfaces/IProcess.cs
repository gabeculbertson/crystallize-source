using UnityEngine;
using System;
using System.Collections;

public interface IProcess {
    event ProcessExitCallback OnReturn;
    void ForceExit();
}

public interface IProcess<I, O> : IProcess{
    void Initialize(I data);
}
