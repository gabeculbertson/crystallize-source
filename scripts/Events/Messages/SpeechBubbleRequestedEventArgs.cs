using UnityEngine;
using System.Collections;

public class SpeechBubbleRequestedEventArgs : System.EventArgs {

    public Transform Target { get; set; }
    public PhraseSequence Phrase { get; set; }
    public PointerType SpeechBubblePointerType { get; set; }
    public bool CanEdit { get; set; }
    public bool HasMore { get; set; }
    public bool CheckGrammar { get; set; }

    public SpeechBubbleRequestedEventArgs(Transform target) {
        Target = target;
        Phrase = null;
        HasMore = false;
        CanEdit = false;
    }

    public SpeechBubbleRequestedEventArgs(Transform target, PhraseSequence phrase, bool hasMore, bool canEdit, bool checkGrammar) {
        Target = target;
        Phrase = phrase;
        HasMore = hasMore;
        CanEdit = canEdit;
        SpeechBubblePointerType = PointerType.Normal;
        CheckGrammar = false;
            //checkGrammar;
    }

}
