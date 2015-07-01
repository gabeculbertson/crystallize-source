using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayerDataConnector {

    public static void RevealJob(JobRef job) {
        var i = PlayerData.Instance.Jobs.GetOrCreateItem(job.ID);
        i.Shown = true;
    }

    public static void UnlockJob(JobRef job) {
        var i = PlayerData.Instance.Jobs.GetOrCreateItem(job.ID);
        i.Unlocked = true;
        //CrystallizeEventManager.PlayerState.raisej(null, null);
    }

    public static void UpdateShownJobs() {
        foreach (var j in GameData.Instance.Jobs.Items) {
            var r = new JobRef(j.ID);
            if (j.GetJobRequirements().IsFulfilled() && !j.Hide) {
                r.PlayerDataInstance.Shown = true;
            }

            if (r.PlayerDataInstance.Shown && j.GetPhraseRequirements().IsFulfilled()) {
                new JobRef(j.ID).PlayerDataInstance.Unlocked = true;
            }
        }
    }

    public static void AddRepetitionToJob(JobRef job, JobTaskRef task) {
        job.PlayerDataInstance.AddTask(task);
    }

    public static void UnlockHome(HomeRef home) {
        PlayerData.Instance.Homes.GetOrCreateItem(home.ID).Unlocked = true;
            //.AddItem(new HomePlayerData(home.ID, true));
        CrystallizeEventManager.PlayerState.RaiseHomesChanged(null, null);
    }

    public static void AddMoney(int amount) {
        PlayerData.Instance.Money += amount;
        CrystallizeEventManager.PlayerState.RaiseMoneyChanged(null, null);
    }

    public static ReviewExperienceArgs AddReviewExperience(int amount) {
        var lastLvl = PlayerData.Instance.Proficiency.GetReviewLevel();
        PlayerData.Instance.Proficiency.ReviewExperience += amount;
        var thisLvl = PlayerData.Instance.Proficiency.GetReviewLevel();
        bool lvlUp = thisLvl != lastLvl;
        var nextLvlXp = ProficiencyPlayerData.GetReviewExperienceForLevel(thisLvl + 1);
        var lvlXp = PlayerData.Instance.Proficiency.GetReviewLevelExperience();
        PlayerData.Instance.Proficiency.SetParametersForLevel();
        return new ReviewExperienceArgs(amount, thisLvl, lvlUp, lvlXp, nextLvlXp);
    }

    public static bool ContainsLearnedItem(PhraseSequence phrase) {
        if (phrase.IsWord) {
            return PlayerData.Instance.WordStorage.ContainsFoundWord(phrase.Word);
        } else {
            return PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase);
        }
    }

    public static void CollectPhrase(PhraseSequence phrase) {
        if (!PlayerData.Instance.PhraseStorage.ContainsPhrase(phrase)) {
            PlayerData.Instance.PhraseStorage.AddPhrase(phrase);
            Debug.Log("phrase added");
            CrystallizeEventManager.PlayerState.RaisePhraseCollected(null, new PhraseEventArgs(phrase));
        }
    }

    public static void CollectWord(PhraseSequenceElement word) {
        if (!PlayerData.Instance.WordStorage.ContainsFoundWord(word)) {
            PlayerData.Instance.WordStorage.AddFoundWord(word);
            CrystallizeEventManager.PlayerState.RaiseWordCollected(null, new PhraseEventArgs(word));
        }
    }

}
