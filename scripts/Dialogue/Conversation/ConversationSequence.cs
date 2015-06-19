using UnityEngine;
using System;
using System.Collections;

public class ConversationSequence : MonoBehaviour, IProcess<GameObject, object> {

    public static ProcessRequestHandler<DialogueState, DialogueState> RequestLinearDialogueTurn;
    public static ProcessRequestHandler<DialogueState, DialogueState> RequestPromptDialogueTurn;
    public static ProcessRequestHandler<GameObject, object> RequestConversationCamera;

    public static ConversationSequence GetInstance(GameObject target) {
        var i = GetInstance();
        i.Initialize(target);
        return i;
    }

    public static ConversationSequence GetInstance() {
        //Debug.Log("GettingInstance");
        var go = new GameObject("Conversation");
        var c = go.AddComponent<ConversationSequence>();
        return c;
    }

    public event ProcessExitCallback OnReturn;

    DialogueActor actor;
    DialogueSequence dialogue;

    public void ForceExit() {
        Exit();
    }

    public void Initialize(GameObject target) {
        actor = target.GetComponent<DialogueActor>();
        dialogue = target.GetInterface<IInstanceReference<DialogueSequence>>().Instance;
    }

    IEnumerator Start() {
        var pgo = PlayerManager.main.PlayerGameObject.GetComponent<Rigidbody>();
        pgo.position = actor.transform.position + actor.transform.forward * 2f;
        pgo.transform.forward = -actor.transform.forward;

        yield return null;
        yield return null;
        
        RequestConversationCamera(this, new ProcessRequestEventArgs<GameObject,object>(actor.gameObject, null, this));

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        pgo.velocity = Vector3.zero;

        PlayerController.LockMovement(this);

        SetDialogueElement(new DialogueState(0, dialogue));
    }

    void Exit() {
        actor.SetPhrase(null);
        PlayerManager.main.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        PlayerController.UnlockMovement(this);

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        OnReturn.Raise(this, null);
        Destroy(gameObject);
    }

    void SetDialogueElement(DialogueState dialogueState) {
        actor.SetPhrase(dialogueState.GetElement().Phrase);
        //Debug.Log(dialogueState.GetElement().NextIDs.Count);

        switch (dialogueState.Dialogue.GetElementType(dialogueState.CurrentID)) {
            case DialogueElementType.Linear: case DialogueElementType.End:
                RequestLinearDialogueTurn(this, new ProcessRequestEventArgs<DialogueState, DialogueState>(dialogueState, HandleTurnExit, this));
                break;
            case DialogueElementType.Prompted:
                RequestPromptDialogueTurn(this, new ProcessRequestEventArgs<DialogueState, DialogueState>(dialogueState, HandleTurnExit, this));
                break;
        }
    }

    void HandleTurnExit(object sender, ProcessExitEventArgs<DialogueState> e) {
        if (e == null) {
            Exit();
        } else {
            //var args = (ProcessExitEventArgs<DialogueState>)e;
            SetDialogueElement(e.Data);
        }
    }

}