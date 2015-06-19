using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionStackGameData {

    public List<InteractionType> Interactions { get; set; }

    public InteractionStackGameData() {
        Interactions = new List<InteractionType>();
    }

    public InteractionType GetTopInteraction() {
        return InteractionType.None;
    }

    InteractionType GetInteractionType(int worldID) {
        if (worldID == -1) {
            return InteractionType.None;
        }

        var qd = GameData.Instance.QuestData.GetQuestInfoFromWorldID(worldID);
        if (qd != null) {
            return InteractionType.StartQuest;
        }

        var ld = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(worldID);
        if (ld != null) {
            return InteractionType.LinearDialogue;
        }

        var nd = GameData.Instance.DialogueData.GetNPCDialogueForWorldObject(worldID);
        if (nd != null) {
            return InteractionType.NPCDialogue;
        }

        var bd = GameData.Instance.DialogueData.GetBranchedDialogueForWorldObject(worldID);
        if (bd != null) {
            return InteractionType.BranchDialogue;
        }

        var phr = GameData.Instance.DialogueData.PersonPhrases.GetItem(worldID);
        if (phr != null) {
            return InteractionType.SinglePhrase;
        }

        return InteractionType.None;
    }

}
