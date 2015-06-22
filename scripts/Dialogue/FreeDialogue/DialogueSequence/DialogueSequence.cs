using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueSequence {

    public SerializableDictionary<int, DialogueElement> Elements { get; set; }

    public const int ConfusedExit = -2;

    public DialogueSequence()
    {
        Elements = new SerializableDictionary<int, DialogueElement>();
    }

    public DialogueElement GetElement(int i)
    {
        return Elements.GetItem(i);
    }

    public DialogueElementType GetElementType(int i)
    {
        var e = GetElement(i);
        if (e == null)
        {
            return DialogueElementType.Null;
        }

        if (e.NextIDs.Count == 0)
        {
            return DialogueElementType.End;
        }
        else if(!GetElement(e.NextIDs[0]).Prompt.IsEmpty)
        {
            return DialogueElementType.Prompted;
        }
        else if (GetElement(e.NextIDs[0]).Condition != null)
        {
            return DialogueElementType.Branched;
        }
        else
        {
            return DialogueElementType.Linear;
        }
    }

    public DialogueElement GetNewDialogueElement()
    {
        int count = 0;
        foreach (var ele in Elements.Items)
        {
            if (ele.ID >= count)
            {
                count = ele.ID + 1;
            }
        }
        var newEle = new DialogueElement();
        newEle.ID = count;
        Elements.AddItem(newEle);
        return newEle;
    }

}
