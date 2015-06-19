using UnityEngine;
using System;
using System.Collections;

public class ConversationSequence : MonoBehaviour, IProcess<GameObject, object> {

    public static ProcessRequestHandler<DialogueState, DialogueState> RequestLinearDialogueTurn;
    public static ProcessRequestHandler<DialogueState, DialogueState> RequestPromptDialogueTurn;
    public static ProcessRequestHandler<GameObject, object> RequestConversationCamera;

    public static ConversationSequence GetInstance(GameObject target) {
        var go = new GameObject("Conversation");
        var c = go.AddComponent<ConversationSequence>();
        c.Initialize(target);
        return c;
    }

    public event ProcessExitCallback<object> OnExit;

    DialogueActor actor;
    DialogueSequence dialogue;

    public void Initialize(ProcessRequestEventArgs<GameObject, object> args) {
        Initialize(args.Data);
    }

    public void ForceExit() {
        Exit();
    }

    void Initialize(GameObject target) {
        actor = target.GetComponent<DialogueActor>();
        dialogue = target.GetInterface<IInstanceReference<DialogueSequence>>().Instance;
    }

    IEnumerator Start() {
        var pgo = PlayerManager.main.PlayerGameObject.GetComponent<Rigidbody>();
        pgo.position = actor.transform.position + actor.transform.forward * 2f;
        pgo.transform.forward = -actor.transform.forward;

        yield return null;
        yield return null;
        
        RequestConversationCamera(this, new ProcessRequestEventArgs<GameObject,object>(actor.gameObject, null));

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        pgo.velocity = Vector3.zero;

        PlayerController.LockMovement(this);

        SetDialogueElement(new DialogueState(0, dialogue));
    }

    void Exit() {
        actor.SetPhrase(null);
        PlayerController.UnlockMovement(this);

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        OnExit.Raise(this, null);
        //Debug.Log("exit");
        Destroy(gameObject);
    }

    void SetDialogueElement(DialogueState dialogueState) {
        actor.SetPhrase(dialogueState.GetElement().Phrase);
        //Debug.Log(dialogueState.GetElement().NextIDs.Count);

        switch (dialogueState.Dialogue.GetElementType(dialogueState.CurrentID)) {
            case DialogueElementType.Linear: case DialogueElementType.End:
                RequestLinearDialogueTurn(this, new ProcessRequestEventArgs<DialogueState, DialogueState>(dialogueState, HandleTurnExit));
                break;
            case DialogueElementType.Prompted:
                RequestPromptDialogueTurn(this, new ProcessRequestEventArgs<DialogueState, DialogueState>(dialogueState, HandleTurnExit));
                break;
        }
    }

    void HandleTurnExit(object sender, ProcessExitEventArgs<DialogueState> args) {
        if (args == null) {
            Exit();
        } else {
            SetDialogueElement(args.Data);
        }
    }

}