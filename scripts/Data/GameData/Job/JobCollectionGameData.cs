using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class JobCollectionGameData : DictionaryCollectionGameData<JobGameData> {

    public JobGameData GetItem(string jobName) {
        return (from j in Items where j.Name == jobName select j).FirstOrDefault();
    }

}
