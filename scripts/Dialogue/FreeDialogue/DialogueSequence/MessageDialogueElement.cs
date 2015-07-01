using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MessageDialogueElement : DialogueElement {

    public string Message { get; set; }

    public MessageDialogueElement()
        : base() {
        Message = "";
    }

    public MessageDialogueElement(string message)
        : this() {
        Message = message;
    }

}
