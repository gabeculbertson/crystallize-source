using UnityEngine;
using System.Collections;

public class WordClickedEventArgs : System.EventArgs {

    public PhraseSequenceElement Word { get; set; }
    public string Destination { get; set; }

    public WordClickedEventArgs(PhraseSequenceElement word, string destination) {
        this.Word = word;
        this.Destination = destination;
    }

}
