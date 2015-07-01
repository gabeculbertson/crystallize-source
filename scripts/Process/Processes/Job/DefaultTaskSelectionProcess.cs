using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class BaseTaskSelectorProcess<T> : IProcess<TaskSelectorArgs, JobTaskRef> where T : JobTaskSelectorGameData {
    
    public event ProcessExitCallback OnExit;

    public void Initialize(TaskSelectorArgs param1) {
        Exit(SelectTask(param1.Job, (T)param1.Selector));
    }

    public abstract JobTaskRef SelectTask(JobRef job, T selector);

    public void ForceExit() {
        Exit(null);
    }

    protected void Exit(JobTaskRef task) {
        OnExit.Raise(this, task);
    }
}

public class DefaultTaskSelectionProcess : BaseTaskSelectorProcess<JobTaskSelectorGameData> {

    public override JobTaskRef SelectTask(JobRef job, JobTaskSelectorGameData selector) {
        return new JobTaskRef(job, job.GameDataInstance.Tasks[0], 0);
    }

}
