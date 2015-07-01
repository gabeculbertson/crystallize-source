using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DayProcess : TimeSessionProcess<DaySessionArgs, object>, IProcess<DaySessionArgs, object> {

    public static readonly ProcessFactoryRef<TaskSelectorArgs, JobTaskRef> RequestJobTask = new ProcessFactoryRef<TaskSelectorArgs, JobTaskRef>();
    public static readonly ProcessFactoryRef<JobTaskRef, object> RequestJob = new ProcessFactoryRef<JobTaskRef, object>();

    JobTaskRef task;
    DaySessionArgs args;

    public override void Initialize(DaySessionArgs input) {
        this.args = input;
        //Debug.Log(input.Job.GameDataInstance.TaskSelector.SelectionProcess.ProcessType);
        
        if (input.ForceSelection) {
            RequestJobTask.Set(typeof(UITaskSelectorProcess));
            RequestJobTask.Get(new TaskSelectorArgs(input.Job, null), SelectTaskCallback, this);
        } else {
            RequestJobTask.Set(input.Job.GameDataInstance.TaskSelector.SelectionProcess.ProcessType);
            RequestJobTask.Get(input.Job.GameDataInstance.TaskSelector.GetArgs(input.Job), SelectTaskCallback, this);
        }
    }

    protected override string SelectNextLevel(DaySessionArgs args) {
        if (task != null) {
            return task.Data.AreaName;
        } else {
            return args.LevelName;
        }
    }

    protected override void AfterLoad() {
        var skip = UILibrary.SkipSessionButton.Get(null);
        skip.Complete += Skip_Complete;

        RequestJob.Get(task, JobCompleteCallback, this);

        //TODO: get rid of direct reference
        ResourceLearnEventHandler.GetInstance();
        CollectUI.GetInstance();
    }

    void SelectTaskCallback(object sender, JobTaskRef task) {
        this.task = task;
        Run(args);
    }

    void Skip_Complete(object sender, EventArgs<object> e) {
        Exit();
    }

    void JobCompleteCallback(object sender, object args) {
        PlayerDataConnector.AddRepetitionToJob(this.args.Job, task);
        Exit();
    }

}
