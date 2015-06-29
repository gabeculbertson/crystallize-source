using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class DialogueSequence {

    public List<SceneObjectGameData> Actors { get; set; }
    public SerializableDictionary<int, DialogueElement> Elements { get; set; }

    public const int ConfusedExit = -2;

    public DialogueSequence()
    {
        Actors = new List<SceneObjectGameData>();
        Elements = new SerializableDictionary<int, DialogueElement>();
    }

    public SceneObjectGameData GetActor(int index) {
        if (Actors.IndexInRange(index)) {
            return Actors[index];
        }
        return new SceneObjectGameData("[default]");
    }

    public DialogueElement GetElement(int i)
    {
        return Elements.GetItem(i);
    }

    public DialogueElement GetNewDialogueElement(Type t)
    {
        if (!typeof(DialogueElement).IsAssignableFrom(t)) {
            Debug.LogError(t + " does not derive from DialogueElement!");
            return null;
        }

        int count = 0;
        foreach (var ele in Elements.Items)
        {
            if (ele.ID >= count)
            {
                count = ele.ID + 1;
            }
        }
        var newEle = (DialogueElement)Activator.CreateInstance(t);
        newEle.ID = count;
        Elements.AddItem(newEle);
        return newEle;
    }

    public T GetNewDialogueElement<T>() where T : DialogueElement, new() {
        int count = 0;
        foreach (var ele in Elements.Items) {
            if (ele.ID >= count) {
                count = ele.ID + 1;
            }
        }
        var newEle = new T();
        newEle.ID = count;
        Elements.AddItem(newEle);
        return newEle;
    }

    public DialogueElement AddNewDialogueElement(DialogueElement e) {
        int count = 0;
        foreach (var ele in Elements.Items) {
            if (ele.ID >= count) {
                count = ele.ID + 1;
            }
        }
        e.ID = count;
        Elements.AddItem(e);
        return e;
    }

}
