using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestSummaryUI : UIMonoBehaviour {

    public GameObject questPrefab;
    public GameObject questObjectivePrefab;

    public Text questDescriptionText;
    public RectTransform selectionBackground;
    public RectTransform activeQuestIndicator;
    public RectTransform questListParent;
    public RectTransform objectiveListParent;

    List<GameObject> questInstances = new List<GameObject>();
    List<GameObject> objectiveInstances = new List<GameObject>();

    QuestPanelQuestSectionUI selectedQuest;

    // Use this for initialization
    void Start() {
        rectTransform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        UpdateQuestPanel();
        CrystallizeEventManager.UI.OnProgressEvent += HandleOnProgressEvent;
    }

    void HandleOnProgressEvent(object sender, System.EventArgs e) {
        UpdateQuestPanel();
    }

    void ClearQuestPanel() {
        foreach (var qi in questInstances) {
            Destroy(qi);
        }
        questInstances.Clear();
    }

    void UpdateQuestPanel() {
        ClearQuestPanel();

        foreach (var q in GameData.Instance.QuestData.Quests.Items) { //QuestInfo.GetAllQuestInfo()) {
            var questState = PlayerData.Instance.QuestData.GetQuestInstance(q.QuestID);
            if (questState == null) {
                continue;
            }

            if (questState.State == ObjectiveState.Hidden) {
                continue;
            }

            //TODO: fix this
            if (q.QuestID < 100) {
                continue;
            }

            Debug.Log("Adding quest: " + q.QuestID + q.WorldID);

            var questInstance = Instantiate(questPrefab) as GameObject;
            questInstance.GetComponent<QuestPanelQuestSectionUI>().Initialize(q, questState.State == ObjectiveState.Complete);
            questInstance.GetComponent<QuestPanelQuestSectionUI>().OnClicked += HandleOnQuestClicked;
            questInstance.transform.SetParent(questListParent);
            questInstances.Add(questInstance);
        }
    }

    void ClearObjectivePanel() {
        foreach (var qi in objectiveInstances) {
            Destroy(qi);
        }
        objectiveInstances.Clear();
    }

    void UpdateDescriptionPanel(QuestInfoGameData quest) {
        ClearObjectivePanel();

        var questState = PlayerData.Instance.QuestData.GetQuestInstance(quest.QuestID);

        questDescriptionText.text = quest.Description;
        var count = quest.GetObjectives().Count;
        for (int index = 0; index < count; index++) {
            var objectiveInstance = Instantiate(questObjectivePrefab) as GameObject;
            objectiveInstance.transform.SetParent(objectiveListParent);
            objectiveInstance.GetComponent<ActiveQuestObjectiveUI>().Initialize(questState, index);

            objectiveInstances.Add(objectiveInstance);
        }
    }

    void HandleOnQuestClicked(object sender, System.EventArgs e) {
        var quest = (QuestPanelQuestSectionUI)sender;
        if (selectedQuest != quest) {
            selectedQuest = quest;
            selectionBackground.position = quest.transform.position;
            UpdateDescriptionPanel(quest.quest);
        } else {
            QuestManager.main.ActiveQuestID = quest.quest.QuestID;
            activeQuestIndicator.transform.position = quest.rectTransform.position - quest.rectTransform.rect.width * 0.5f * Vector3.right;
        }
    }

    public void Close() {
        Destroy(gameObject);
        CrystallizeEventManager.UI.OnProgressEvent -= HandleOnProgressEvent;
    }

}
