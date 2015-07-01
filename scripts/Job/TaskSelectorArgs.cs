using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TaskSelectorArgs {

    public JobRef Job { get; set; }
    public JobTaskSelectorGameData Selector { get; set; }

    public TaskSelectorArgs(JobRef job, JobTaskSelectorGameData selector) {
        this.Job = job;
        this.Selector = selector;
    }

}
