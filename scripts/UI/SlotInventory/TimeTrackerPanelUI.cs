//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;

//// TODO: This is a massive mess
//public class TimeTrackerPanelUI : MonoBehaviour {

//    public ExplicitInventorySlotUI slot;
//    public Image timeBar;
//    public Image timeBar2;
//    public Text timeText;
//    public Text rankText;
//    public Transform rankBox;

//    // Use this for initialization
//    void Start () {
//    }
	
//    // Update is called once per frame
//    void Update () {
//        RefreshState ();
//    }

//    void RefreshState(){
//        float fillAmount = 0;
//        if (slot.Word != null && LevelSettings.main.canRankUp) {
//            var level = Mathf.RoundToInt(PhraseEvaluator.GetPhraseLevel(slot.Word)) - 1;
//            if(rankText){
//                rankText.text = level.ToString();
//            }

//            if(rankBox){
//                rankBox.gameObject.SetActive(true);
//                if(!LevelSettings.CanRankUp(slot.Word)){
//                    rankBox.GetComponent<Image>().color = Color.gray;
//                } else {
//                    rankBox.GetComponent<Image>().color = Color.white;
//                }
//            }

//            var review = ReviewManager.main.reviewLog.GetReview(slot.Word.GetText());
//            var duration = review.GetDurationToNextReview();
//            var end = review.GetNextReviewTime();
//            if(end > ReviewManager.main.simulatedTime && review.Level > 0){
//                fillAmount = (float)(end.Ticks - ReviewManager.main.simulatedTime.Ticks)/duration.Ticks;
//                if(timeText){
//                    timeText.text = ((int)(end - ReviewManager.main.simulatedTime).TotalSeconds) + " s";
//                }
//            } else {
//                if(timeText){
//                    timeText.text = "";
//                }
//            }
//        } else {
//            if(timeText){
//                timeText.text = "";
//            }

//            if(rankBox){
//                rankBox.gameObject.SetActive(false);
//            }
//        }

//        if (rankBox && PlayerManager.main.State == PlayerState.Conversation) {
//            rankBox.gameObject.SetActive(false);
//        }

//        if (timeBar) {
//            timeBar.fillAmount = fillAmount;
//        }

//        if (timeBar2) {
//            timeBar2.fillAmount = fillAmount;
//        }
//    }
//}
