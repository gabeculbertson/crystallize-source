using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobGameData : ISerializableDictionaryItem<int>, IHasID {

    public int ID { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public List<JobTaskGameData> Tasks { get; set; }

    public int Key {
        get { return ID; }
    }

    public JobGameData() {
        ID = -1;
        Name = "";
        Difficulty = 1;
        Tasks = new List<JobTaskGameData>();
    }

}
