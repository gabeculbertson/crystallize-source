using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewUI : BaseReviewUI, ITemporaryUI<object, int> {

    const string ResourcePath = "UI/Review";
    public static ReviewUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<ReviewUI>(ResourcePath);
    }

    public Text itemText;
    public Text answerText;
    public GameObject checkButton;

    public void Initialize(object param1) {
        transform.SetParent(MainCanvas.main.transform, false);
        Refresh();
    }

    protected override void Refresh() {
        //Debug.Log(PlayerData.Instance.PhraseStorage.Phrases.Count);
        PlayerData.Instance.Reviews.GetNewReviews();
        var reviews = PlayerData.Instance.Reviews.GetCurrentReviews();

        if (reviews.Count > 0) {
            ActiveReview = reviews[UnityEngine.Random.Range(0, reviews.Count)];
            DisplayReview(ActiveReview);
        } else {
            Exit();
        }
    }

    void DisplayReview(ItemReviewPlayerData review) {
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

    public void ShowAnswer() {
        if (ActiveReview != null) {
            checkButton.SetActive(false);
        } else {
            Refresh();
        }
    }

    protected override void OnSetResult() {
        Refresh();
    }
    
    
}
