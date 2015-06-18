using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[XmlInclude(typeof(MoneyQuestReward))]
public class QuestReward {

    public virtual void GrantReward() {
        Debug.Log("No reward.");
    }

    public virtual string GetRewardDescription() {
        return "N/A";
    }

}
