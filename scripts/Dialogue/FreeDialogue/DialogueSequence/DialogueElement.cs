﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueElement : ISerializableDictionaryItem<int> {

    public int ID { get; set; }
    public PhraseSequence Prompt { get; set; }
    public PhraseSequence Phrase { get; set; }
    public ConditionBranch Condition { get; set; }
    public int DefaultNextID { get; set; }
    public List<int> NextIDs { get; set; }

    public int Key
    {
        get
        {
            return ID;
        }
    }

    public DialogueElement()
    {
        Phrase = new PhraseSequence();
        ID = -1;
        DefaultNextID = -1;
        NextIDs = new List<int>();
    }

}