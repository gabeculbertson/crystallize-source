﻿using UnityEngine;
using System;
using System.Collections;

public interface IProcess {
    event ProcessExitCallback OnExit;
    void ForceExit();
}

public interface IProcess<I> : IProcess, IInitializable<I> {

}

public interface IProcess<I, O> : IProcess<I>{

}

public interface IUIProcess<I, O> : IProcess<I, O> {
    void SetUIInstance(ITemporaryUI<I, O> uiInstance);
}
