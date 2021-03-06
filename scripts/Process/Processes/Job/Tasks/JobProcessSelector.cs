﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobProcessSelector : IProcess<JobTaskRef, object> {

    ProcessFactoryRef<JobTaskRef, object> TaskFactory = new ProcessFactoryRef<JobTaskRef, object>();

    public event ProcessExitCallback OnExit;

    public void Initialize(JobTaskRef param1) {
        //Debug.Log(param1.Data.Name);
        TaskFactory.Set(param1.Data.ProcessType.ProcessType);
        TaskFactory.Get(param1, ChildCallback, this);
    }

    void ChildCallback(object sender, object args) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
