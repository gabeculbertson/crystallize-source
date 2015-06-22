using UnityEngine;
using System.Collections;
using Crystallize;

public class ConversationStart_tmp : MonoBehaviour, IInstanceReference<DialogueSequence> {

    public DialogueSequenceUnityXml dialogue = new DialogueSequenceUnityXml();

    public DialogueSequence Instance {
        get {
            return dialogue.GetObject();
        }
    }
    
}
