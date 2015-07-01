using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobTaskRef  {

    public JobRef Job { get; private set; }
    public JobTaskGameData Data { get; private set; }
    public int Variation { get; private set; }

    public int Index {
        get {
            return Job.GameDataInstance.Tasks.IndexOf(Data);
        }
    }

    public JobTaskRef(JobRef job, JobTaskGameData data) {
        this.Job = job;
        this.Data = data;
        this.Variation = 0;
    }

    public JobTaskRef(JobRef job, JobTaskGameData data, int variation) : this(job, data) {
        this.Job = job;
        this.Data = data;
        this.Variation = variation;
    }

}
