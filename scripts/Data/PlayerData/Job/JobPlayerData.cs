using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobPlayerData : ISerializableDictionaryItem<int> {

    public int JobID { get; set; }
    public bool Shown { get; set; }
    public bool Unlocked {get; set;}
    public float ExperiencePoints { get; set; }
    public List<TaskEntryPlayerData> Tasks { get; set; }

    public int Key {
        get { return JobID; }
    }

    public int Repetitions {
        get {
            return Tasks.Count;
        }
    }

    public JobPlayerData() {
        JobID = -1;
        Shown = false;
        Unlocked = false;
        ExperiencePoints = 0;
        Tasks = new List<TaskEntryPlayerData>();
    }

    public JobPlayerData(int jobID, bool unlocked) {
        JobID = jobID;
    }

    public void AddTask(JobTaskRef task) {
        var i = task.Index;
        if (i != -1) {
            Tasks.Add(new TaskEntryPlayerData(i));
        } else {
            Debug.LogError("JobGameData must contain task.");
        }
    }

    public int ViewedTasks() {
        var h = new HashSet<int>();
        foreach (var t in Tasks) {
            if (!h.Contains(t.TaskID)) {
                h.Add(t.TaskID);
            }
        }
        return h.Count;
    }

}
