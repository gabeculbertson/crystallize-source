using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobTaskRef  {

    public JobRef Job { get; private set; }
    public JobTaskGameData Data { get; private set; }

    public JobTaskRef(JobRef job, JobTaskGameData data) {
        this.Job = job;
        this.Data = data;
    }

}
