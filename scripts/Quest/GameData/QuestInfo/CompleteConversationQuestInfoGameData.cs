using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompleteConversationQuestInfoGameData : QuestInfoGameData {

    public int ActorGlobalID { get; set; }

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
        PlayerData.Instance.Conversation.SetAvailable(cid.ID, true);
    }

    public override void ProcessMessage(System.EventArgs args) {
        //Debug.Log("Checking");
        var cid = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(ActorGlobalID);
        if (cid == null) {
            Debug.Log("Conversation not found: " + ActorGlobalID);
            return;
        }

        if (PlayerData.Instance.Conversation.GetConversationComplete(ActorGlobalID)) {//cid.ID)) {
            CompleteObjective(0);
        }
    }

}
