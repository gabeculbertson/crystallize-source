using UnityEngine;
using System.Collections;
using Util.Serialization;

[System.Serializable]
public class NetworkSpeechBubbleRequestedEventArgs : System.EventArgs {

    public string Target { get; set; }
    public string Phrase { get; set; }
    public PointerType SpeechBubblePointerType { get; set; }
    public bool CanEdit { get; set; }
    public bool HasMore { get; set; }
    public bool CheckGrammar { get; set; }

    public NetworkSpeechBubbleRequestedEventArgs(Transform target) {
        Target = target.name;
        Phrase = null;
        HasMore = false;
        CanEdit = false;
    }

    public NetworkSpeechBubbleRequestedEventArgs(Transform target, PhraseSequence phrase, bool hasMore, bool canEdit, bool checkGrammar) {
        Target = target.name;
        Phrase = Serializer.SaveToXmlString<PhraseSequence>(phrase);
        HasMore = hasMore;
        CanEdit = canEdit;
        SpeechBubblePointerType = PointerType.Normal;
        CheckGrammar = false;
            //checkGrammar;
    }

    public Transform GetTarget(){
        var a = ActorTracker.GetActorForName(Target);
        if (a == null) {
            return null;
        }
        return a.transform;
    }

    public PhraseSequence GetPhraseSequence() {
        return Serializer.LoadFromXmlString<PhraseSequence>(Phrase);
    }

}
