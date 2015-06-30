using UnityEngine;
using System.Collections;

public class DialogueState {

    public int CurrentID { get; set; }
    public DialogueSequence Dialogue { get; set; }
	public ContextData Context {get; set;}
    public StringMap ActorMap{ get; set; }

    public DialogueState(int id, DialogueSequence dialogue, ContextData context) {
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
        if (ActorMap != null) {
            if (ActorMap.ContainsKey(tag)) {
                return ActorMap.GetItem(tag).Value;
            }
        }
        return tag;
    }

}