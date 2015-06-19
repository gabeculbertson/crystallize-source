using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestRelevantPeople : MonoBehaviour {

    public List<GameObject> people = new List<GameObject>();

	// Use this for initialization
	void Start () {
	    CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;

        HandleQuestStateChanged(null, System.EventArgs.Empty);
	}

    void HandleQuestStateChanged(object sender, System.EventArgs args){
        if (!gameObject) {
            return;
        }
        
        //Debug.Log("Cahnged.");
        var quest = GameData.Instance.QuestData.GetQuestInfoFromWorldID(transform.GetWorldID());
        var inst = PlayerManager.main.playerData.QuestData.GetOrCreateQuestInstance(quest.QuestID);
        if (inst == null) {
            //Debug.Log("No instance");
            return;
        }
        
        if (inst.State == ObjectiveState.Active) {
            foreach (var p in people) {
                if (p) {
                    //Debug.Log("setting " + p + " true");
                    p.GetInterface<IQuestInteractionPoint>().SetRelevant(true);
                }
            }
        } else {
            foreach (var p in people) {
                if (p) {
                    //Debug.Log("setting " + p + " false");
                    p.GetInterface<IQuestInteractionPoint>().SetRelevant(false);
                }
            }
        }
    }
	
}
