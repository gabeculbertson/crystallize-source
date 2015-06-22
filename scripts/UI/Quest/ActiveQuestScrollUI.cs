using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActiveQuestScrollUI : UIMonoBehaviour {

    public Text indexText;
    public Button scrollUpButton;
    public Button scrollDownButton;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //var q = new List<QuestInstanceData>();
        int count = 0;
        int index = 0;
        foreach (var q in PlayerData.Instance.QuestData.QuestInstances) {
            if (q.State == ObjectiveState.Active) {
                count++;
                if (q.QuestID == QuestManager.main.ActiveQuestID) {
                    index = count;
                }
            }
        }

        if (count <= 1) {
            canvasGroup.alpha = 0;
        } else {
            canvasGroup.alpha = 1f;
            indexText.text = string.Format("{0}/{1}", index, count);

            scrollUpButton.GetComponent<CanvasGroup>().alpha = 1f;
            scrollDownButton.GetComponent<CanvasGroup>().alpha = 1f;

            if (index == count) {
                scrollUpButton.GetComponent<CanvasGroup>().alpha = 0;
            }

            if (index == 1) {
                scrollDownButton.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        if (count >= 1 && index == 0) {
            SetFirst();
        }
    }

    public void ScrollUp() {
        var found = false;
        var qs = PlayerData.Instance.QuestData.QuestInstances;
        for (int i = 0; i < qs.Count; i++) {
            var q = qs[i];
            if (q.State == ObjectiveState.Active) {
                if (found) {
                    QuestManager.main.ActiveQuestID = q.QuestID;
                    break;
                } else if (q.QuestID == QuestManager.main.ActiveQuestID) {
                    found = true;
                }
            }
        }
    }

    public void ScrollDown() {
        var found = false;
        var qs = PlayerData.Instance.QuestData.QuestInstances;
        for (int i = qs.Count - 1; i >= 0; i--) {
            var q = qs[i];
            if (q.State == ObjectiveState.Active) {
                if (found) {
                    QuestManager.main.ActiveQuestID = q.QuestID;
                    break;
                } else if (q.QuestID == QuestManager.main.ActiveQuestID) {
                    found = true;
                }
            }
        }
    }

    public void SetFirst() {
        var qs = PlayerData.Instance.QuestData.QuestInstances;
        for (int i = 0; i < qs.Count; i++) {
            var q = qs[i];
            if (q.State == ObjectiveState.Active) {
                QuestManager.main.ActiveQuestID = q.QuestID;
                Debug.Log("Setting: " + q.QuestID);
                break;
            }
        }
    }

}
