using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConversationArgs {
    public GameObject Target { get; set; }
    public DialogueSequence Dialogue { get; set; }

    public ConversationArgs(GameObject target, DialogueSequence dialogue) {
        this.Target = target;
        this.Dialogue = dialogue;
    }
}
