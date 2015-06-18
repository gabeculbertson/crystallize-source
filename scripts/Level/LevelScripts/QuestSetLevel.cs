using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestSetLevel : MonoBehaviour {

    public List<Transform> targets = new List<Transform>();

    List<int> cids = new List<int>();

	// Use this for initialization
	void Start () {
        foreach (var t in targets) {
            var wid = t.GetWorldID();
            var c = GameData.Instance.QuestData.GetQuestInfoFromWorldID(wid);
            if(c ==  null){
                continue;
            }

            cids.Add(c.QuestID);
        }
        ObjectiveManager.main.SetObjective(this, false);
	}
	
	// Update is called once per frame
	void Update () {
        var finished = true;
	    foreach(var cid in cids){
            if (PlayerManager.main.playerData.QuestData.GetQuestInstance(cid).State != ObjectiveState.Complete) {
                finished = false;
                break;
            }
        }
        if (finished) {
            ObjectiveManager.main.SetObjective(this, true);
            Destroy(this);
        }
	}
}
