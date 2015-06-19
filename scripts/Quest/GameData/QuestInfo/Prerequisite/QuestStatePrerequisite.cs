using UnityEngine;
using System.Collections;

public class QuestStatePrerequisite : StatePrerequisite {

    public int QuestID { get; set; }

    public override bool IsFulfilled() {
        var qi = PlayerData.Instance.QuestData.GetQuestInstance(QuestID);
        if (qi == null) {
            //Debug.Log("No qi");
            return false;
        } else {
            //Debug.Log(qi.State);
            return qi.State == ObjectiveState.Complete;
        }
    }

}
