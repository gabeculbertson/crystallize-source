using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompleteBranchQuestInfoGameData : QuestInfoGameData {

    public int ActorGlobalID { get; set; }
    public int BranchID { get; set; }

    public override List<QuestObjectiveInfoGameData> GetDefaultObjectives() {
        var o = new List<QuestObjectiveInfoGameData>();
        o.Add(new QuestObjectiveInfoGameData("Complete the conversation"));
        return o;
    }

    protected override void Begin() {
        var cid = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(ActorGlobalID);
        if (cid == null) {
            Debug.Log("Invalid actor: " + ActorGlobalID);
            return;
        }
        PlayerManager.main.playerData.Conversation.SetAvailable(cid.ID, true);
    }

    public override void ProcessMessage(System.EventArgs args) {
        //Debug.Log("Checking for: " + ActorGlobalID + "; " + BranchID);
        if (PlayerData.Instance.Conversation.GetBranchCompleted(ActorGlobalID, BranchID)){
            CompleteObjective(0);
        }
    }

}
