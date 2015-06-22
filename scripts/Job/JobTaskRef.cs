using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobTaskRef {

    public JobTaskGameData Data { get; private set; }

    public JobTaskRef(JobTaskGameData data) {
        this.Data = data;
    }

}
