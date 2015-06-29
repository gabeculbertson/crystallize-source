using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class UIDialogueElement : DialogueElement {

    public abstract void SelectItem(ProcessExitCallback<int> callback);

}

public class UIDialogueElement<T> : UIDialogueElement {

    public UIFactoryRef<List<T>, T> Factory { get; set; }
    public Func<List<T>> GetArgs { get; set; }
    public Func<T, int> GetNextElement { get; set; }

    public UIDialogueElement() : base() { 

    }

    public UIDialogueElement(UIFactoryRef<List<T>, T> factory, Func<List<T>> getArgs, Func<T, int> getNext) {
        Factory = factory;
        GetArgs = getArgs;
        GetNextElement = getNext;
    }

    public override void SelectItem(ProcessExitCallback<int> callback) {
        var ui = Factory.Get(GetArgs());
        ui.Complete += (s, e) => ui_Complete(s, e, callback);
    }

    void ui_Complete(object sender, EventArgs<T> e, ProcessExitCallback<int> callback) {
        callback(this, GetNextElement(e.Data));
    }

}