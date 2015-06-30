using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class JobGameData : ISerializableDictionaryItem<int>, IHasID {

    public int ID { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public JobUnlockPrerequisiteGameData Prerequisite { get; set; }
    public List<JobTaskGameData> Tasks { get; set; }
    public List<JobRequirementGameData> Requirements { get; set; }
    //public List<PhraseSequence> LearnablePhrases { get; set; }

    public int Key {
        get { return ID; }
    }

    public JobGameData() {
        ID = -1;
        Name = "";
        Difficulty = 1;
        Prerequisite = new JobUnlockPrerequisiteGameData();
        Tasks = new List<JobTaskGameData>();
        Requirements = new List<JobRequirementGameData>();
        //LearnablePhrases = new List<PhraseSequence>();
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

}