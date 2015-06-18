using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestConfirmationUI : UIMonoBehaviour {

    public GameObject rewardPrefab;

	public Text questTitleText;
	public Text questDescriptionText;
    public RectTransform rewardParent;

	QuestInfoGameData quest;

    float time = 0;

	public void Initialize(QuestInfoGameData quest){
		questTitleText.text = quest.Title;
		questDescriptionText.text = quest.Description;
		this.quest = quest;
		transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);

        foreach (var r in quest.Rewards) {
            var instance = Instantiate(rewardPrefab) as GameObject;
            instance.transform.SetParent(rewardParent);
            instance.GetComponentInChildren<Text>().text = r.GetRewardDescription();
        }
	}

    void Awake() {
        canvasGroup.alpha = 0;
    }

    void Update() {
        time += Time.deltaTime;
        if (time > 0.5f) {
            canvasGroup.alpha += Time.deltaTime;
        }
    }

	public void Confirm(){
		CrystallizeEventManager.UI.RaiseUIInteraction (this, new QuestConfirmedEventArgs (quest.QuestID));
		Close ();
	}

	public void Close(){
		Destroy (gameObject);
	}

}
