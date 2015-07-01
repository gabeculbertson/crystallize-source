using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PreviousJobRequirementGameData : JobRequirementGameData {

    public string JobName { get; set; }

    public override bool IsFulfilled() {
        var j = new JobRef(JobName);
        return j.PlayerDataInstance.Unlocked;
    }

}
