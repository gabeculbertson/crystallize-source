using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DaySessionArgs : TimeSessionArgs {

    public JobRef Job { get; private set; }
    public bool ForceSelection { get; private set; }

    public DaySessionArgs(string level, JobRef job) : base(level) {
        Job = job;
    }

    public DaySessionArgs(string level, JobRef job, bool forceSelection)
        : this(level,job) {
        Job = job;
        ForceSelection = forceSelection;
    }

}
