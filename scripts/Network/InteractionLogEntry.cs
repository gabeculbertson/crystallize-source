using UnityEngine;
using System.Collections;

public class InteractionLogEntry {

    static InteractionLogEntry _break = new InteractionLogEntry();

    public static InteractionLogEntry Break {
        get {
            return _break;
        }
    }

    public int Player { get; set; }
    public PhraseSequence Phrase { get; set; }

    public InteractionLogEntry() {
        Player = -1;
        Phrase = null;
    }

    public InteractionLogEntry(int player, PhraseSequence phrase){
        Player = player;
        Phrase = phrase;
    }

}
