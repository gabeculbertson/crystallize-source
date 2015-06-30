using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class ItemReviewPlayerData {

    public static TimeSpan GetIntervalForRank(int rank) {
        if (rank == 0) {
            return new TimeSpan(0, 1, 0);
        } else if (rank == 1) {
            return new TimeSpan(0, 5, 0);
        } else if (rank == 2) {
            return new TimeSpan(0, 10, 0);
        } else {
            return new TimeSpan((rank - 2).Squared(), 0, 0, 0);
        }
    }

    public PhraseSequence Phrase { get; set; }
    public int Rank { get; set; }
    public List<ReviewEntryPlayerData> Entries { get; set; }

    public bool IsWord {
        get{
            return Phrase.PhraseElements.Count == 1;
        }
    }

    public PhraseSequenceElement Word {
        get {
            return Phrase.PhraseElements[0];
        }
    }

    public ItemReviewPlayerData() {
        Phrase = new PhraseSequence();
        Rank = 0;
        Entries = new List<ReviewEntryPlayerData>();
    }

    public ItemReviewPlayerData(PhraseSequence phrase) : this() {
        Phrase = phrase;
    }

    public bool NeedsReview(){
        if (Entries.Count == 0) {
            return true;
        }
        return (Entries.Last().Time + GetIntervalForRank(Rank)) <= ReviewTimeManager.GetTime();
    }

    public void AddEntry(int result) {
        Entries.Add(new ReviewEntryPlayerData(result));
        if (result == 0) {
            Rank = 0;
        } else if (result == 1) {
            Rank++;
        } else if (result == 2) {
            Rank += 2;
        }
        Debug.Log(Phrase.GetText() + ": rank " + Rank);
    }

}
