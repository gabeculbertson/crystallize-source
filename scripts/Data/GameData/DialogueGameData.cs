using UnityEngine;
using System.Collections;

public class DialogueGameData {

	public int CurrentBranchedDialogueID { get; set; }
    public int CurrentLinearDialogueID { get; set; }
    public int CurrentNPCDialogueID { get; set; }

    public SerializableDictionary<int, LinearDialogueSection> LinearDialogues { get; set; }
    public SerializableDictionary<int, LinearDialogueHolder> LinearDialogueHolders { get; set; }
    public SerializableDictionary<int, NPCDialogue> NPCDialogues { get; set; }
    public SerializableDictionary<int, NPCDialogueHolder> NPCDialogueHolders { get; set; }
	public SerializableDictionary<int, BranchedDialogue> BranchedDialogues { get; set; }
	public SerializableDictionary<int, BranchedDialogueHolder> BranchedDialogueHolders { get; set; }
	public SerializableDictionary<int, ContextData> PersonContextData { get; set; }
	public SerializableDictionary<int, PhraseHolder> PersonPhrases { get; set; }
    public SerializableDictionary<int, NPCInventoryGameData> NPCInventories { get; set; }

	public DialogueGameData(){
		CurrentBranchedDialogueID = 1000000;
        CurrentLinearDialogueID = 1000000;
        CurrentNPCDialogueID = 1000000;
        LinearDialogues = new SerializableDictionary<int, LinearDialogueSection>();
        NPCDialogues = new SerializableDictionary<int, NPCDialogue>();
        NPCDialogueHolders = new SerializableDictionary<int, NPCDialogueHolder>();
        LinearDialogueHolders = new SerializableDictionary<int, LinearDialogueHolder>();
		BranchedDialogueHolders = new SerializableDictionary<int, BranchedDialogueHolder> ();
		BranchedDialogues = new SerializableDictionary<int, BranchedDialogue> ();
		PersonContextData = new SerializableDictionary<int, ContextData> ();
		PersonPhrases = new SerializableDictionary<int, PhraseHolder> ();
        NPCInventories = new SerializableDictionary<int, NPCInventoryGameData>();
	}

    public LinearDialogueSection GetLinearDialogueForWorldObject(int worldID) {
        var holder = LinearDialogueHolders.GetItem(worldID);
        if (holder == null) {
            return null;
        }

        return LinearDialogues.GetItem(holder.DialogueID);
    }

    public NPCDialogue GetNPCDialogueForWorldObject(int worldID) {
        var holder = NPCDialogueHolders.GetItem(worldID);
        if (holder == null) {
            return null;
        }

        return NPCDialogues.GetItem(holder.DialogueID);
    }

	public BranchedDialogue GetBranchedDialogueForWorldObject(int worldID){
		var holder = BranchedDialogueHolders.GetItem (worldID);
		if (holder == null) {
			return null;
		}

		return BranchedDialogues.GetItem(holder.BranchedDialogueID);
	}

    public LinearDialogueSection AddNewLinearDialogue() {
        var bd = new LinearDialogueSection(CurrentLinearDialogueID);
        CurrentLinearDialogueID += 10;
        LinearDialogues.AddItem(bd);
        return bd;
    }

    public NPCDialogue AddNewNPCDialogue() {
        var bd = new NPCDialogue(CurrentNPCDialogueID);
        CurrentNPCDialogueID += 10;
        NPCDialogues.AddItem(bd);
        return bd;
    }

	public BranchedDialogue AddNewBranchedDialogue(){
		var bd = new BranchedDialogue (CurrentBranchedDialogueID);
		CurrentBranchedDialogueID += 10;
		BranchedDialogues.AddItem (bd);
		return bd;
	}

    public LinearDialogueSection AttachNewLinearDialogue(int worldID) {
        var d = AddNewLinearDialogue();
        LinearDialogueHolders.AddItem(new LinearDialogueHolder(worldID, d.ID));
        return d;
    }

    public NPCDialogue AttachNewNPCDialogue(int worldID) {
        var d = AddNewNPCDialogue();
        NPCDialogueHolders.AddItem(new NPCDialogueHolder(worldID, d.ID));
        return d;
    }

	public BranchedDialogue AttachNewBranchedDialogue(int worldID){
		var bd = AddNewBranchedDialogue ();
		BranchedDialogueHolders.AddItem(new BranchedDialogueHolder(worldID, bd.ID));
		return bd;
	}

	public ContextData AttachNewPersonContext(int worldID){
		var pcd = ContextData.GetPersonContextData (worldID);
		PersonContextData.AddItem(pcd);
		return pcd;
	}

    public NPCInventoryGameData AttachNewInventory(int worldID) {
        var inv = new NPCInventoryGameData(worldID);
        NPCInventories.AddItem(inv);
        return inv;
    }

}
