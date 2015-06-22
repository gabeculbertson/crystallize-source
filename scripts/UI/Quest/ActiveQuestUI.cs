using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActiveQuestUI : UIMonoBehaviour {

	public GameObject questObjectivePrefab;
	public Text questNameText;
	public RectTransform questListParent;

    bool open = true;
	List<GameObject> questObjectiveInstances = new List<GameObject>();

	// Use this for initialization
	IEnumerator Start () {
		if (!LevelSettings.main.useQuesting) {
			gameObject.SetActive(false);
			yield break;
		}

		//SetActiveQuest (QuestManager.main.ActiveQuestID);

		CrystallizeEventManager.PlayerState.OnActiveQuestChanged += HandleOnActiveQuestChanged;
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;
        //gameObject.SetActive (false);

        yield return new WaitForSeconds(0.1f);

        //Debug.Log(QuestManager.main.ActiveQuestID);

        Close();

        var info = GameData.Instance.QuestData.Quests.GetItem(QuestManager.main.ActiveQuestID);
        if (info != null) {
            if (QuestManager.main.ActiveQuestID > 100) {
                SetActiveQuest(info.QuestID);
            }
        }
	}

    void HandleQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
        if (e.PlayerID == PlayerManager.Instance.PlayerID) {
            if (e.GetQuestInstance().State != ObjectiveState.Complete) {
                SetActiveQuest(e.QuestID);
            }
        }
    }

    void Open() {
        gameObject.SetActive(true);
        open = true;
    }

    void Close() {
        open = false;
    }

	void Update(){
        if (open) {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, 5f * Time.deltaTime);
        } else {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime);
        }
	}

	void HandleOnActiveQuestChanged (object sender, QuestEventArgs e)
	{
        var info = GameData.Instance.QuestData.Quests.GetItem(QuestManager.main.ActiveQuestID);
        if (info == null) 
            return;
        if (QuestManager.main.ActiveQuestID != info.QuestID) 
            return;

        gameObject.SetActive(true);

        SetActiveQuest(e.QuestID);
	}

	void SetActiveQuest(int questID){
        foreach (var i in questObjectiveInstances) {
			Destroy(i);
		}
		questObjectiveInstances.Clear ();

        //Debug.Log(questID);
		var info = GameData.Instance.QuestData.Quests.GetItem (questID);
        var questpd = PlayerData.Instance.QuestData.GetOrCreateQuestInstance(info.QuestID);
        questNameText.text = info.Title;

        var c = Color.white;
        if (questpd.State == ObjectiveState.Complete) {
            c = GUIPallet.Instance.successColor;
            Close();
        } else {
            Open();
        }
        questNameText.color = c;

        var count = info.GetObjectives().Count;
        for(int id = 0; id < count; id++){
            var instance = Instantiate(questObjectivePrefab) as GameObject;
            instance.GetComponentInChildren<ActiveQuestObjectiveUI>().Initialize(questpd, id);
            instance.transform.SetParent(questListParent);
            instance.GetComponentInChildren<Text>().color = c;
            questObjectiveInstances.Add(instance);
        }
	}

}
