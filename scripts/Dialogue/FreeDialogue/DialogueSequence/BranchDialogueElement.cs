using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchDialogueElement : DialogueElement {

    public List<BranchDialogueElementLink> Branches { get; set; }

    public BranchDialogueElement()
        : base() {
        Branches = new List<BranchDialogueElementLink>();
    }

    public override string ToString() {
        return "BranchDialogueElement[" + Branches.Count + "]";
    }

}