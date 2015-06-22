using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RankUpUI : UIMonoBehaviour {

    enum ReviewState {
        CoolingDown,
        Ready
    }

	public ExplicitInventorySlotUI slot;
    public Image cooldownBar;
    public CanvasGroup rankUpText;
    public Text rankText;

	void Start(){
        if (slot.Word == null) {
            Destroy(gameObject);
            return;
        }

        if (!PlayerData.Instance.WordStorage.ContainsFoundWord(slot.Word.WordID)) {
            Destroy(gameObject);
            return;
        }

        if (slot.Word.ElementType != PhraseSequenceElementType.FixedWord) {
            Destroy(gameObject);
            return;
        }

        //Debug.Log(slot.Word.ElementType);

        if (LevelSettings.main) {
            if (!LevelSettings.main.canRankUp || LevelSettings.main.isMultiplayer) {
                Destroy(gameObject);
                return;
            }
        }
	}

	// Update is called once per frame
	void Update () {
		if (UISystem.main.Mode == UIMode.Speaking) {
			canvasGroup.alpha = 0;
			return;
		}

        if (slot.Word == null) {
            canvasGroup.alpha = 0;
            return;
        }

        if (slot.Word.WordID < 1000000) {
            canvasGroup.alpha = 0;
            return;
        }

        var review = ReviewManager.main.reviewLog.GetReview(slot.Word.WordID);
        var state = GetReviewState(review);
        switch (state) {
            case ReviewState.CoolingDown:
                var end = review.GetNextReviewTime();
                var duration = review.GetDurationToNextReview();
                float fillAmount = (float)(end.Ticks - ReviewManager.main.simulatedTime.Ticks) / duration.Ticks;
                cooldownBar.fillAmount = fillAmount;
                rankUpText.alpha = 0;
                break;

            case ReviewState.Ready:
                cooldownBar.fillAmount = 0;
                rankUpText.alpha = 0.5f + 0.5f * Mathf.PingPong(Time.time * 2f, 1f);
                break;
        }

        rankText.text = review.Level.ToString();
	}

    ReviewState GetReviewState(PhraseReviewData review) {
        var end = review.GetNextReviewTime();
        if (end > ReviewManager.main.simulatedTime && review.Level > 0) {
            return ReviewState.CoolingDown;
        } else {
            return ReviewState.Ready;
        }
    }

	void RankUpEffect(){
		EffectManager.main.CreateFlashEffect(slot.rectTransform);
		EffectManager.main.CreateRisingTextEffect(slot.rectTransform, "Rank up!");
	}

}
