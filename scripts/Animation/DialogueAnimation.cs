using UnityEngine;
using System;
using System.Collections;

public abstract class DialogueAnimation {

    public abstract event EventHandler OnComplete;
    public abstract void Play(GameObject actor);

}