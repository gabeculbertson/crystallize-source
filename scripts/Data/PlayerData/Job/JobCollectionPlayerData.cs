using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobCollectionPlayerData : UniqueKeySerializableDictionary<JobPlayerData> {

    public JobPlayerData GetOrCreateItem(int id) {
        if (ContainsKey(id)) {
            return GetItem(id);
        }
        var i = new JobPlayerData();
        i.JobID = id;
        AddItem(i);
        return i;
    }

}
