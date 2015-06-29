using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DayProcess : TimeSessionProcess<DaySessionArgs, object>, IProcess<DaySessionArgs, object> {

    public static readonly ProcessFactoryRef<JobTaskRef, object> RequestJob = new ProcessFactoryRef<JobTaskRef, object>();

    JobTaskRef task;
    DaySessionArgs args;

    protected override string SelectNextLevel(DaySessionArgs args) {
        if (args.Job == null) {
            return args.LevelName;
        } else {
            task = new JobTaskRef(args.Job, args.Job.GameDataInstance.Tasks[0]);
            return task.Data.AreaName;
        }
    }

    protected override void BeforeInitialize(DaySessionArgs input) {
        this.args = input;
    }

    protected override void AfterLoad() {
        var skip = UILibrary.SkipSessionButton.Get(null);
        skip.Complete += Skip_Complete;

        RequestJob.Get(task, JobCompleteCallback, this);

        //TODO: get rid of direct reference
        ResourceLearnEventHandler.GetInstance();
        CollectUI.GetInstance();
    }

    void Skip_Complete(object sender, EventArgs<object> e) {
        Exit();
    }

    void JobCompleteCallback(object sender, object args) {
        Exit();
    }

}
