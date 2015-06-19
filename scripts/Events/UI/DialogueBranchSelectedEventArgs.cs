using UnityEngine;
using System;
using System.Collections;

public class DialogueBranchSelectedEventArgs : EventArgs {

	public BranchedDialogueElement DialogueBranch { get; set; }

	public DialogueBranchSelectedEventArgs(BranchedDialogueElement dialogueBranch){
		DialogueBranch = dialogueBranch;
	}

}
