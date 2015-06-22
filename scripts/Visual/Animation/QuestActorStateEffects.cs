using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class QuestActorStateEffects : MonoBehaviour {
	
	GameObject lockEffect;
	GameObject questionEffect;
	GameObject exclaimationEffect;
	GameObject checkEffect;

	GameObject[] Effects { 
		get {
			return new GameObject[]{ lockEffect, exclaimationEffect, questionEffect, checkEffect};
		}
	}

    int questID;
    ConversationClientState lastState = ConversationClientState.Hidden;
    bool initialized = false;

	// Use this for initialization
	void Start () {
        var id = transform.GetWorldID();
        var c = GameData.Instance.QuestData.GetQuestInfoFromWorldID(id);
        if (c == null) {
            Destroy(this);
            return;
        }
        questID = c.QuestID;

        lockEffect = Instantiate(EffectLibrary.Instance.uiClientLock) as GameObject;
		lockEffect.GetComponent<ClientLockUI> ().Initialize (transform);
        questionEffect = Instantiate(EffectLibrary.Instance.uiQuestionMark) as GameObject;
		questionEffect.GetComponent<QuestExclaimationPointUI> ().Initialize (transform);
        exclaimationEffect = Instantiate(EffectLibrary.Instance.uiExclaimationPoint) as GameObject;
		exclaimationEffect.GetComponent<QuestExclaimationPointUI> ().Initialize (transform);
        checkEffect = Instantiate(EffectLibrary.Instance.uiCheckMark) as GameObject;
		checkEffect.GetComponent<QuestCheckUI> ().Initialize (transform);
        initialized = true;

        RefreshOverheadIndicator();
	}

    void Update() {
        var s = GetState();
        
        if (s != lastState) {
            lastState = s;
            RefreshOverheadIndicator();
        }
    }

    void OnEnable() {
        if (initialized) {
            RefreshOverheadIndicator();
        }
    }

    void OnDisable() {
        foreach (var eff in Effects) {
            if (eff) {
                eff.SetActive(false);
            }
        }
    }

	void DialogSuccessEffect(){
		AudioManager.main.PlayDialogueSuccess ();
        var effect = Instantiate(EffectLibrary.Instance.conversationCompleteEffect) as GameObject;
		effect.transform.SetParent (MainCanvas.main.transform);
		effect.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}

	void RefreshOverheadIndicator(){
		foreach (var eff in Effects) {
			eff.SetActive(false);
		}

		switch (lastState) {
		case ConversationClientState.Locked:
			lockEffect.SetActive (true);
			return;

		case ConversationClientState.SeekingClient:
			questionEffect.SetActive (true);
			return;

		case ConversationClientState.Available:
			exclaimationEffect.SetActive (true);
			return;

		case ConversationClientState.Completed:
			checkEffect.SetActive (true);
			return;
		}
	}

    ConversationClientState GetState() {
        if (DialogueSystemManager.main.InteractionTarget) {
            return ConversationClientState.Hidden;
        }

        var q = GameData.Instance.QuestData.Quests.GetItem(questID);
        if (q == null) {
            return ConversationClientState.Hidden;
        }

        var aq = PlayerData.Instance.QuestData.GetQuestInstance(QuestManager.main.ActiveQuestID);
        if (aq != null) {
            if (aq.State != ObjectiveState.Complete) {
                //return ConversationClientState.Hidden;
            }
        }

        if (!q.IsAvailable()) {
            //Debug.Log("No available");
            return ConversationClientState.Hidden;
        }

        var questPlayerData = PlayerData.Instance.QuestData.GetQuestInstance(q.QuestID);
        if (questPlayerData == null) 
            return ConversationClientState.Available;
        if (questPlayerData.State == ObjectiveState.Available)
            return ConversationClientState.Available;
        if (questPlayerData.State == ObjectiveState.Hidden)
            return ConversationClientState.Available;
        if (questPlayerData.State == ObjectiveState.Complete)
            return ConversationClientState.Completed;

        return ConversationClientState.Hidden;
    }

}
