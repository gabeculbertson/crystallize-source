using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseSetGameData : ISerializableDictionaryItem<string> {

    public string Name { get; set; }
    public List<PhraseSequence> Phrases { get; set; }

    public string Key {
        get { return Name; }
    }

    public PhraseSetGameData() {
        Name = "";
        Phrases = new List<PhraseSequence>();
    }

    public PhraseSetGameData(string name) : this(){
        Name = name;
    }

    public PhraseSequence GetOrCreatePhrase(int index) {
        while (Phrases.Count <= index) {
            Phrases.Add(new PhraseSequence());
        }

        return Phrases[index];
    }
    
}