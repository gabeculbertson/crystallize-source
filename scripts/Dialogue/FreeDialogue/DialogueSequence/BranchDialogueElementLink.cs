﻿using UnityEngine;
using System.Collections;

public class BranchDialogueElementLink {

    public int NextID { get; set; }
    public PhraseSequence Prompt { get; set; }

    public BranchDialogueElementLink() {
        Prompt = new PhraseSequence();
    }

}