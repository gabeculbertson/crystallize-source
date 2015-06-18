using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordInventory {

    public List<string> WordIDs { get; set; }

    public WordInventory() {
        WordIDs = new List<string>();
    }

    public void Add(string wordID) {
        if (!WordIDs.Contains(wordID)) {
            WordIDs.Add(wordID);
        }
    }

    public void Remove(string wordID) {
        if (WordIDs.Contains(wordID)) {
            WordIDs.Remove(wordID);
        }
    }

    public bool Contains(string wordID) {
        return WordIDs.Contains(wordID);
    }

}