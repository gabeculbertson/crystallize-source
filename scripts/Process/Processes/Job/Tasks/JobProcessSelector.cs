using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobProcessSelector : IProcess<JobTaskRef, object> {

    ProcessFactoryRef<JobTaskRef, object> TaskFactory = new ProcessFactoryRef<JobTaskRef, object>();

    public event ProcessExitCallback OnExit;

    public void Initialize(JobTaskRef param1) {
        switch (param1.Job.ID) {
            case 0:
                TaskFactory.Set<WaiterProcess>();
                break;
            case 1:
                TaskFactory.Set<JanitorProcess>();
                break;
            case 2:
                TaskFactory.Set<RestaurantProces>();
                break;
            default:
                TaskFactory.Set<TempProcess<JobTaskRef, object>>();
                break;
        }
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
