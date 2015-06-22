using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobPlayerData : ISerializableDictionaryItem<int> {

    public int JobID { get; set; }
    public bool Unlocked {get; set;}
    public float ExperiencePoints { get; set; }

    public int Key {
        get { return JobID; }
    }

    public JobPlayerData() {
        JobID = -1;
        Unlocked = false;
        ExperiencePoints = 0;
    }

    public JobPlayerData(int jobID, bool unlocked) {
        JobID = jobID;
    }

}
