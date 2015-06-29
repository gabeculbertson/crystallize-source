using UnityEngine;
using System.Collections;

public class LineDialogueElement : DialogueElement {

    public DialogueActorLine Line { get; set; }

    public LineDialogueElement() : base(){
        Line = new DialogueActorLine();
    }

    public override string ToString() {
        return "LineDialogueElement[" + Line.Phrase.ToString() + "]";
    }

}