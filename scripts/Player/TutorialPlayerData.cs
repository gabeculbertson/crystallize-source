using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialPlayerData {

    public HashSet<int> ViewedTutorials { get; set; }
    public List<PhraseProgressionPlayerData> PhraseProgressions { get; set; }

    public TutorialPlayerData() {
        ViewedTutorials = new HashSet<int>();
        PhraseProgressions = new List<PhraseProgressionPlayerData>();
    }

    public bool GetTutorialViewed(int tutorialID) {
        return ViewedTutorials.Contains(tutorialID);
    }

    public void SetTutorialViewed(int tutorialID) {
        ViewedTutorials.Add(tutorialID);
    }

    public PhraseProgressionPlayerData GetPhraseProgression(PhraseSequence phrase) {
        foreach (var p in PhraseProgressions) {
            if (PhraseSequence.IsPhraseEquivalent(p.Phrase, phrase)) {
                return p;
            }
        }
        return null;
    }

    public int GetPhraseProgressionStep(PhraseSequence phrase) {
        var p = GetPhraseProgression(phrase);
        if (p == null) {
            return 0;
        }
        return p.Step;
    }

    public void SetPhraseProgressionInteraction(PhraseSequence phrase, int globalID) {
        var p = GetPhraseProgression(phrase);
        if (p == null) {
            p = new PhraseProgressionPlayerData();
            p.Phrase = phrase;
            PhraseProgressions.Add(p);
        }
        p.AddActor(globalID);
    }

    public List<bool> GetMissingWords(PhraseSequence phrase) {
        var missing = new List<bool>();
        var prog = GameData.Instance.ProgressionData.PhraseProgression.GetProgression(phrase);
        var step = GetPhraseProgressionStep(phrase);

        for (int i = 0; i < phrase.PhraseElements.Count; i++) {
            missing.Add(prog.GetWordMissing(step, i));
        }
        
        return missing;
    }

}
