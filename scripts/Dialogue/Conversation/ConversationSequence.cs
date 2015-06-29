using UnityEngine;
using System;
using System.Collections;

public class ConversationSequence : IProcess<ConversationArgs, object> {

    public static readonly ProcessFactoryRef<DialogueState, DialogueState> RequestLinearDialogueTurn = new ProcessFactoryRef<DialogueState,DialogueState>();
    public static readonly ProcessFactoryRef<DialogueState, DialogueState> RequestPromptDialogueTurn = new ProcessFactoryRef<DialogueState,DialogueState>();
    public static readonly ProcessFactoryRef<DialogueState, DialogueState> RequestAnimationDialogueTurn = new ProcessFactoryRef<DialogueState, DialogueState>();
    public static readonly ProcessFactoryRef<DialogueState, DialogueState> RequestUIDialogueTurn = new ProcessFactoryRef<DialogueState, DialogueState>();
    public static readonly ProcessFactoryRef<GameObject, object> RequestConversationCamera = new ProcessFactoryRef<GameObject,object>();

    public event ProcessExitCallback OnExit;

    ConversationArgs args;
    DialogueActor actor;
    ContextData context;

    public void Initialize(ConversationArgs args) {
        this.args = args;
        actor = args.Target.GetComponent<DialogueActor>();
        context = args.Context;

        ProcessLibrary.BeginConversation.Get(args, BeginCallback, this);
    }

    void BeginCallback(object obj, object args) {
        ProcessLibrary.ConversationSegment.Get(this.args, MidCallback, this);
    }

    void MidCallback(object obj, object args) {
        ProcessLibrary.EndConversation.Get(this.args, EndCallback, this);
    }

    void EndCallback(object obj, object args) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}