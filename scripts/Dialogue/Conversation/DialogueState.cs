using UnityEngine;
using System.Collections;

public class DialogueState {

    public int CurrentID { get; set; }
    public DialogueSequence Dialogue { get; set; }
    public StringMap Context { get; set; }

    public DialogueState(int id, DialogueSequence dialogue, StringMap context) {
        CurrentID = id;
        Dialogue = dialogue;
        Context = context;
    }

    public DialogueElement GetElement() {
        return Dialogue.GetElement(CurrentID);
    }

    public GameObject GetTarget() {
        var name = GetActorName(Dialogue.GetActor(GetElement().ActorIndex).Name);
        return GameObject.Find(name);
    }

    public string GetActorName(string tag) {
        if (Context != null) {
            if (Context.ContainsKey(tag)) {
                return Context.GetItem(tag).Value;
            }
        }
        return tag;
    }

}