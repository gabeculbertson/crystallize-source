using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewUI : UIPanel, ITemporaryUI<object, int> {

    const string ResourcePath = "UI/Review";
    public static ReviewUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<ReviewUI>(ResourcePath);
    }

    public Text itemText;
    public Text answerText;
    public GameObject checkButton;

    public event EventHandler<EventArgs<int>> Complete;

    int count = 0;
    ItemReviewPlayerData activeReview;

    //public IEnumerator Start() {
    //    Refresh();
    //}

    public void Initialize(object param1) {
        transform.SetParent(MainCanvas.main.transform, false);
        Refresh();
    }

    void Refresh() {
        //Debug.Log(PlayerData.Instance.PhraseStorage.Phrases.Count);
        PlayerData.Instance.Reviews.GetNewReviews();
        var reviews = PlayerData.Instance.Reviews.GetCurrentReviews();

        if (reviews.Count > 0) {
            activeReview = reviews[UnityEngine.Random.Range(0, reviews.Count)];
            SetReview(activeReview);
        } else {
            Complete.Raise(this, new EventArgs<int>(count));
            Close();
            //itemText.text = "No more reviews";
        }
    }

    void SetReview(ItemReviewPlayerData review) {
        itemText.text = review.Phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji);
        if(review.IsWord){
            var entry =  DictionaryData.Instance.GetEntryFromID(review.Word.WordID);
            if (entry == null) {
                Debug.Log("No entry for: [" + review.Word + "] ID:" + review.Word.WordID);
            }
            var s = "";
            foreach(var e in entry.English){
                s += e + "\n";
            }
            s = s.Substring(0, s.Length - 1);
            answerText.text = s;
        } else {
            answerText.text = review.Phrase.Translation;
        }

        checkButton.SetActive(true);
    }

    public void Skip() {
        Complete.Raise(this, new EventArgs<int>(count));
        Close();
    }

    public void ShowAnswer() {
        if (activeReview != null) {
            checkButton.SetActive(false);
        } else {
            Refresh();
        }
    }

    public void SetResult(int result) {
        count++;
        if (activeReview != null) {
            activeReview.AddEntry(result);
            activeReview = null;
        }
        Refresh();
    }
    
}
