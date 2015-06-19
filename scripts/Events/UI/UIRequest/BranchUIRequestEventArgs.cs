using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchUIRequestEventArgs : UIRequestEventArgs {

	public List<BranchedDialogueElement> Branches { get; set; }

	public BranchUIRequestEventArgs(GameObject menuParent, List<BranchedDialogueElement> branches) : base(menuParent) {
		this.Branches = branches;
	}

}
