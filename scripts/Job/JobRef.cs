using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobRef : IDRef<JobGameData, JobPlayerData> {
	
    public override JobGameData GameDataInstance {
		get {
			return GameData.Instance.Jobs.GetItem (ID);
		}
		set{}
    }

    public override JobPlayerData PlayerDataInstance {
		get {
			return PlayerData.Instance.Jobs.GetOrCreateItem (ID);
		}
		set{}
    }

    public JobRef(int id) : base(id) { }

    public JobRef(string name) : base(-1) {
        var j = GameData.Instance.Jobs.GetItem(name);
        ID = j.ID;
    }

    public string ViewedEventsString() {
        return string.Format("{0}/{1}", PlayerDataInstance.ViewedTasks(), GameDataInstance.Tasks.Count);
    }

}
