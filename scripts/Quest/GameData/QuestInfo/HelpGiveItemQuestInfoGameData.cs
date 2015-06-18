using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpGiveItemQuestInfoGameData : QuestInfoGameData {

    public int OtherQuestID { get; set; }

    public HelpGiveItemQuestInfoGameData()
        : base() {
            OtherQuestID = -1;
    }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var o = new List<QuestObjectiveInfoGameData>();
        o.Add(new QuestObjectiveInfoGameData("Help your partner"));

        return o;
    }

    public override void ReceiveMessage(System.EventArgs args) {
        if (!(args is PartnerObjectiveCompleteEventArgs)) {
            return;
        }

        var qArgs = (PartnerObjectiveCompleteEventArgs)args;
        if (qArgs.QuestID != OtherQuestID) {
            return;
        }

        CompleteObjective(0);
        CompleteQuest();
    }

}
