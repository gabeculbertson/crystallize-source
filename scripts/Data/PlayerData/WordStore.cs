using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class WordStore {

    public List<PhraseSequenceElement> InventoryElements { get; set; }
    public List<int> ObjectiveWords { get; set; }
    public List<int> FoundWords { get; set; }

    // TODO: get rid of these
    public List<string> InventoryWordIDs { get; set; }
    public HashSet<string> ObjectiveWordIDs { get; set; }
    public HashSet<string> FoundWordIDs { get; set; }
    public HashSet<string> UnlockedWordIDs { get; set; }

    public WordStore() {
        InventoryElements = new List<PhraseSequenceElement>();
        ObjectiveWords = new List<int>();
        FoundWords = new List<int>();

        InventoryWordIDs = new List<string>();
        ObjectiveWordIDs = new HashSet<string>();
        FoundWordIDs = new HashSet<string>();
        UnlockedWordIDs = new HashSet<string>();
    }

    public void AddObjectiveWord(int wordID) {
        if (!ObjectiveWords.Contains(wordID) && !FoundWords.Contains(wordID)) {
            ObjectiveWords.Add(wordID);
        }
    }

    public void RemoveObjectiveWord(int wordID) {
        if (ObjectiveWords.Contains(wordID)) {
            ObjectiveWords.Remove(wordID);
        }
    }

    public bool ContainsObjectiveWord(PhraseSequenceElement word) {
        return ObjectiveWords.Contains(word.WordID);
    }

    public bool AddFoundWord(PhraseSequenceElement word) {
        return AddFoundWord(word.WordID);
    }

    public bool AddFoundWord(int wordID) {
        if (!FoundWords.Contains(wordID)) {
            RemoveObjectiveWord(wordID);
            FoundWords.Add(wordID);

            // TODO: should not be here, need to keep events out of data classes
            CrystallizeEventManager.PlayerState.RaiseGameEvent(this, System.EventArgs.Empty);

            DataLogger.LogTimestampedData("Found", wordID.ToString());
            return true;
        }
        return false;
    }

    public void RemoveFoundWord(int wordID) {
        if (FoundWords.Contains(wordID)) {
            FoundWords.Remove(wordID);
        }
    }

    public bool ContainsFoundWord(PhraseSequenceElement word) {
        return ContainsFoundWord(word.WordID);
    }

    public bool ContainsFoundWord(int wordID) {
        return FoundWords.Contains(wordID);
    }

    public bool ContainsFoundWord(PhraseSegmentData phrase) {
        return FoundWordIDs.Contains(phrase.ID);
    }

    public bool ContainsInventoryWord(PhraseSequenceElement word) {
        foreach (var w in InventoryElements) {
            if (PhraseSequenceElement.IsEqual(word, w)) {
                return true;
            }
        }
        return false;
    }

    public void AddUnlockedWord(PhraseSegmentData phrase) {
        if (!UnlockedWordIDs.Contains(phrase.ID)) {
            UnlockedWordIDs.Add(phrase.ID);
        }
    }

    public void RemoveObjectiveWord(PhraseSegmentData phrase) {
        if (ObjectiveWordIDs.Contains(phrase.ID)) {
            ObjectiveWordIDs.Remove(phrase.ID);
        }
    }

    public int GetInventoryCount() {
        var c = InventoryElements.Count;
        if (c == 0) {
            return 0;
        }

        while (InventoryElements[c - 1] == null) {
            c--;
            if (c == 0) {
                break;
            }
        }

        return c;
    }

}