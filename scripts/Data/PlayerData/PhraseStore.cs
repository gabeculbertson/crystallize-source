using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseStore {

    public List<PhraseSequence> Phrases { get; set; }

    public PhraseStore() {
        Phrases = new List<PhraseSequence>();
    }

    public bool ContainsPhrase(PhraseSequence phrase) {
        foreach (var p in Phrases) {
            if (PhraseSequence.IsPhraseEquivalent(p, phrase)) {
                return true;
            }
        }
        return false;
    }

    public void AddPhrase(PhraseSequence phrase) {
        if (!ContainsPhrase(phrase)) {
            Phrases.Add(phrase);
        }
    }

}
