using UnityEngine;
using System.Collections;

public class AnimationDialogueElement : DialogueElement {

    public DialogueAnimation Animation { get; set; }

    public AnimationDialogueElement()
        : base() {

    }

    public override string ToString() {
        return "AnimationDialogueElement[" + Animation + "]";
    }

}