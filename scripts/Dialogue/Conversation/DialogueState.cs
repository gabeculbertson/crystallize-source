using UnityEngine;
using System.Collections;

public class DialogueState {

    public int CurrentID { get; set; }
    public DialogueSequence Dialogue { get; set; }

    public DialogueState(int id, DialogueSequence dialogue) {
        CurrentID = id;
        Dialogue = dialogue;
    }

    public DialogueElement GetElement() {
        return Dialogue.GetElement(CurrentID);
    }

}