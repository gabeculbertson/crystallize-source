using UnityEngine;
using System;
using System.Collections;

public class ConversationSequence : IProcess<ConversationArgs, object> {

    public static readonly ProcessFactoryRef<DialogueState, DialogueState> RequestLinearDialogueTurn = new ProcessFactoryRef<DialogueState,DialogueState>();
    public static readonly ProcessFactoryRef<DialogueState, DialogueState> RequestPromptDialogueTurn = new ProcessFactoryRef<DialogueState,DialogueState>();
    public static readonly ProcessFactoryRef<GameObject, object> RequestConversationCamera = new ProcessFactoryRef<GameObject,object>();

    public event ProcessExitCallback OnExit;

    DialogueActor actor;
    DialogueSequence dialogue;
    ContextData context;

    public void Initialize(ConversationArgs target) {
        actor = target.Target.GetComponent<DialogueActor>();
        dialogue = target.Dialogue;
        context = target.Context;

        CoroutineManager.Instance.StartCoroutine(PrepareConversation());
    }

    public void ForceExit() {
        Exit();
    }

    IEnumerator PrepareConversation() {
        var pgo = PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();
        pgo.position = actor.transform.position + actor.transform.forward * 2f;
        pgo.transform.forward = -actor.transform.forward;
        pgo.velocity = Vector3.zero;

        yield return null;

        pgo.position = actor.transform.position + actor.transform.forward * 2f;
        pgo.transform.forward = -actor.transform.forward;
        pgo.velocity = Vector3.zero;
        
        RequestConversationCamera.Get(actor.gameObject, null, this);

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        PlayerController.LockMovement(this);

        SetDialogueElement(new DialogueState(0, dialogue));
    }

    void Exit() {
        actor.SetPhrase(null);
        PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        PlayerController.UnlockMovement(this);

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        OnExit.Raise(this, null);
    }

    void SetDialogueElement(DialogueState dialogueState) {
        actor.SetLine(dialogueState.GetElement().Line, context);
        //Debug.Log(dialogueState.GetElement().NextIDs.Count);

        switch (dialogueState.Dialogue.GetElementType(dialogueState.CurrentID)) {
            case DialogueElementType.Linear: case DialogueElementType.End:
                RequestLinearDialogueTurn.Get(dialogueState, HandleTurnExit, this);
                break;
            case DialogueElementType.Prompted:
                RequestPromptDialogueTurn.Get(dialogueState, HandleTurnExit, this);
                break;
        }
    }

    void HandleTurnExit(object sender, DialogueState e) {
        if (e == null) {
            Exit();
        }
        
        if (e.CurrentID == DialogueSequence.ConfusedExit) {
            var p = new PhraseSequence("(stupid foreigner...)");
            actor.SetPhrase(p);
            RequestLinearDialogueTurn.Get(e, HandleTurnExit, this);
            return;
        }

        SetDialogueElement(e);
    }

}