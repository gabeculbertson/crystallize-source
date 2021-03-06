﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class JobGameData : ISerializableDictionaryItem<int>, ISetableKey<int>, IHasID {

    public int ID { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public JobTaskSelectorGameData TaskSelector { get; set; }
    public List<JobTaskGameData> Tasks { get; set; }
    public List<JobRequirementGameData> Requirements { get; set; }

    public bool Hide { get; set; }

    public int Key {
        get { return ID; }
    }

    public JobGameData() {
        ID = -1;
        Name = "";
        Difficulty = 1;
        Tasks = new List<JobTaskGameData>();
        Requirements = new List<JobRequirementGameData>();
        TaskSelector = new JobTaskSelectorGameData();
        //LearnablePhrases = new List<PhraseSequence>();
    }

    public void SetKey(int key) {
        ID = key;
    }

    public void AddRequirement(PhraseSequence phrase) {
        Requirements.Add(new PhraseJobRequirementGameData(phrase));
    }

    public bool RequirementsFullfilled() {
        foreach (var r in Requirements) {
            if (!r.IsFulfilled()) {
                return false;
            }
        }
        return true;
    }

    public IEnumerable<PhraseJobRequirementGameData> GetPhraseRequirements() {
        return from r in Requirements 
               where r is PhraseJobRequirementGameData
               select (PhraseJobRequirementGameData)r;
    }

    public IEnumerable<PreviousJobRequirementGameData> GetJobRequirements() {
        return from r in Requirements
               where r is PreviousJobRequirementGameData
               select (PreviousJobRequirementGameData)r;
    }

}