using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class MultipleChoiceReviewUI : BaseReviewUI, ITemporaryUI<object, int> {
    const string ResourcePath = "UI/MultipleChoiceReview";

    static int[] standardWordIDs = new int[] { 1323080, 1291050, 1387990, 1206900 };
    
    public static MultipleChoiceReviewUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<MultipleChoiceReviewUI>(ResourcePath);
    }

    public GameObject buttonPrefab;
    public GameObject solutionPrefab;
    public Text itemText;
    public Text questionText;
    public Image backgroundImage;
    public RectTransform buttonParent;

    List<GameObject> instances = new List<GameObject>();

    public void Initialize(object param1) {
        transform.SetParent(MainCanvas.main.transform, false);
        Refresh();
    }

    protected override void Refresh() {
        PlayerData.Instance.Reviews.GetNewReviews();
        var reviews = PlayerData.Instance.Reviews.GetCurrentReviews();

        if (reviews.Count > 0) {
            ActiveReview = reviews[UnityEngine.Random.Range(0, reviews.Count)];
            DisplayReview(ActiveReview);
        } else {
            Exit();
        }
    }

    void Clear() {
        foreach (var b in instances) {
            Destroy(b);
        }
    }

    void DisplayReview(ItemReviewPlayerData review) {
        itemText.text = review.Phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji);
        questionText.text = "What does this mean?";
        backgroundImage.color = Color.white;
        List<PhraseSequence> choices = new List<PhraseSequence>();
        if (review.IsWord) {
            choices = GetWordChoices();
        } else {
            choices = GetPhrasesChoices();
        }
        UIUtil.GenerateChildren(choices, instances, buttonParent, GetButtonInstance);
    }

    GameObject GetButtonInstance(PhraseSequence p) {
        var instance = Instantiate<GameObject>(buttonPrefab);
        instance.GetComponentInChildren<Text>().text = p.Translation;
        instance.AddComponent<DataContainer>().Store(p);
        instance.GetComponent<UIButton>().OnClicked += MultipleChoiceReviewUI_OnClicked;
        return instance;
    }

    void MultipleChoiceReviewUI_OnClicked(object sender, EventArgs e) {
        var p = ((Component)sender).GetComponent<DataContainer>().Retrieve<PhraseSequence>();
        
        Clear();

        var t = "";
        if (ActiveReview.Phrase.IsWord) {
            t = ActiveReview.Phrase.Word.GetTranslation();
        } else {
            t = ActiveReview.Phrase.Translation;
        }

        var solutionInstance = Instantiate<GameObject>(solutionPrefab);
        Debug.Log(t == null);
        solutionInstance.GetComponentInChildren<Text>().text = t;
        solutionInstance.transform.SetParent(buttonParent.transform, false);
        instances.Add(solutionInstance);

        var continueButtonInstance = Instantiate<GameObject>(buttonPrefab);
        continueButtonInstance.GetComponentInChildren<Text>().text = "Continue";
        continueButtonInstance.GetComponent<UIButton>().OnClicked += Continue_OnClicked;
        continueButtonInstance.transform.SetParent(buttonParent.transform, false);
        instances.Add(continueButtonInstance);

        if (PhraseSequence.IsPhraseEquivalent(p, ActiveReview.Phrase)) {
            backgroundImage.color = GUIPallet.Instance.successColor.Lighten(0.5f);
            questionText.text = "Right! The correct answer is...";
            SetResult(1);
        } else {
            backgroundImage.color = GUIPallet.Instance.failureColor.Lighten(0.5f);
            questionText.text = "Wrong. The correct answer is...";
            SetResult(0);
        }

        ActiveReview = null;
    }

    void Continue_OnClicked(object sender, EventArgs e) {
        Debug.Log("Continue");
        Refresh();
    }

    List<PhraseSequence> GetPhrasesChoices() {
        var phrases = new List<PhraseSequence>(PlayerData.Instance.PhraseStorage.Phrases);
        int count = 0;
        while (phrases.Count < 5) {
            var w = new PhraseSequenceElement(standardWordIDs[count], 0);
            var p = new PhraseSequence(w);
            p.Translation = w.GetTranslation();
            phrases.Add(p);
            count++;
        }

        return phrases.PickN(5);
    }

    List<PhraseSequence> GetWordChoices() {
        var words = PlayerData.Instance.WordStorage.FoundWords.Select((w) => new PhraseSequenceElement(w, 0)).ToList();
        int count = 0;
        while (words.Count < 5) {
            words.Add(new PhraseSequenceElement(standardWordIDs[count], 0));
            count++;
        }

        var selected = words.PickN(5);
        return selected.Select((w) => new PhraseSequence(w)).ToList();
    }

}
