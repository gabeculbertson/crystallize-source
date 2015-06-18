using UnityEngine;
using System.Collections;

public class DialogueBranchID {

    public int GlobalID { get; set; }
    public int BranchID { get; set; }

    public DialogueBranchID() {

    }

    public DialogueBranchID(int globalID, int branchID) {
        GlobalID = globalID;
        BranchID = branchID;
    }

    public override bool Equals(object obj) {
        var dbid = obj as DialogueBranchID;
        if (dbid == null) {
            return false;
        }
        return GlobalID == dbid.GlobalID && BranchID == dbid.BranchID;
    }

    public override int GetHashCode() {
        int hash = 23;
        hash = hash * 31 + GlobalID.GetHashCode();
        hash = hash * 31 + BranchID.GetHashCode();
        return hash;
    }

}
