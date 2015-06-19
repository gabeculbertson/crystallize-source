using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GiveItemQuestInfoGameData : QuestInfoGameData {

    public int TargetGlobalID { get; set; }
    public int TargetItemID { get; set; }

    public GiveItemQuestInfoGameData()
        : base() {
            TargetGlobalID = -1;
            TargetItemID = -1;
    }

    public override System.Collections.Generic.List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var o = new List<QuestObjectiveInfoGameData>();
        o.Add(new QuestObjectiveInfoGameData("Give the item"));

        return o;
    }

    public override void ProcessMessage(System.EventArgs args) {
        //Debug.Log("Got: " + args);
        if (!(args is ItemGivenEventArgs)) {
            return;
        }
        var igArgs = (ItemGivenEventArgs)args;
        if (igArgs.GlobalID != TargetGlobalID) {
            return;
        }

        Debug.Log("Got item: " + igArgs.ItemID);
        if (igArgs.ItemID != TargetItemID) {
            return;
        }

        CompleteObjective(0);
        //CompleteQuest();
    }

}
