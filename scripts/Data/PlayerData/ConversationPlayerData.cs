using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationPlayerData {

    public HashSet<int> AvailableConversations { get; set; }
    public HashSet<int> CompletedConversations { get; set; }
    public HashSet<DialogueBranchID> CompletedBranches { get; set; }

    public ConversationPlayerData() {
        AvailableConversations = new HashSet<int>();
        CompletedConversations = new HashSet<int>();
        CompletedBranches = new HashSet<DialogueBranchID>();
    }

    public bool GetConversationComplete(int conversationID) {
        return CompletedConversations.Contains(conversationID);
    }

    public void SetConversationComplete(int conversationID) {
        if (!CompletedConversations.Contains(conversationID)) {
            CompletedConversations.Add(conversationID);
        }
    }

    public bool GetBranchCompleted(int globalID, int branchID) {
        return CompletedBranches.Contains(new DialogueBranchID(globalID, branchID));
    }

    public void SetBranchComplete(int globalID, int branchID) {
        var bid = new DialogueBranchID(globalID, branchID);
        if(!CompletedBranches.Contains(bid)){
            CompletedBranches.Add(bid);
        }
    }

    public bool IsAvailable(int conversationID) {
        if (!AvailableConversations.Contains(conversationID)) {
            if (GameData.Instance.DialogueData.LinearDialogues.GetItem(conversationID).AvailableByDefault) {
                return true;
            }
            return false;
        }
        return true;
    }

    public void SetAvailable(int conversationID, bool available) {
        if (available) {
            if (!AvailableConversations.Contains(conversationID)) {
                AvailableConversations.Add(conversationID);
            }
        } else {
            if (AvailableConversations.Contains(conversationID)) {
                AvailableConversations.Remove(conversationID);
            }
        }
    }

}
