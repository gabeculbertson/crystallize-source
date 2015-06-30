using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class ReviewPlayerData {

    public List<ItemReviewPlayerData> Reviews { get; set; }

    public ReviewPlayerData() {
        Reviews = new List<ItemReviewPlayerData>();
    }

    public void GetNewReviews() {
        foreach (var w in PlayerData.Instance.WordStorage.FoundWords) {
            var pse = new PhraseSequenceElement(w, 0);
            var p = new PhraseSequence();
            p.Add(pse);
            //Debug.Log("Contains review: " + p.GetText() + "; " + ContainsReview(p));
            if (!ContainsReview(p)) {
                //Debug.Log("Adding review: " + p.GetText());
                Reviews.Add(new ItemReviewPlayerData(p));
            }
        }

        foreach (var p in PlayerData.Instance.PhraseStorage.Phrases) {
            if (!ContainsReview(p)) {
                //Debug.Log("Adding review: " + p.GetText());
                Reviews.Add(new ItemReviewPlayerData(p));
            }
        }
    }

    public bool ContainsReview(PhraseSequence p) {
        var rev = (from r in Reviews where PhraseSequence.IsPhraseEquivalent(p, r.Phrase) select r).FirstOrDefault();
        return rev != null;
    }

    public List<ItemReviewPlayerData> GetCurrentReviews() {
        return (from r in Reviews where r.NeedsReview() select r).ToList();
    }

}
