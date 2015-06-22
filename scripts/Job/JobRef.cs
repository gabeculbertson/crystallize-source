using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobRef : IDRef<JobGameData, JobPlayerData> {

    public override JobGameData GameDataInstance {
        get {
            return GameData.Instance.Jobs.GetItem(ID);
        }
    }

    public override JobPlayerData PlayerDataInstance {
        get {
            return PlayerData.Instance.Jobs.GetItem(ID);
        }
    }

    public JobRef(int id) : base(id) { }

}
