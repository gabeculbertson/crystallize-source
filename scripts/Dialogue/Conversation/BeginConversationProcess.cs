using UnityEngine;
using System.Collections;

public class BeginConversationProcess : ConversationProcessPart, IProcess<ConversationArgs, object> {

    DialogueActor actor;
    ContextData context;

    public event ProcessExitCallback OnExit;

    public void Initialize(ConversationArgs args) {
        actor = args.Target.GetComponent<DialogueActor>();
        context = args.Context;

        CoroutineManager.Instance.StartCoroutine(PrepareConversation(args.Dialogue));
    }

    IEnumerator PrepareConversation(DialogueSequence dialogue) {
        var pgo = PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>();
        pgo.position = actor.transform.position + actor.transform.forward * 2f;
        pgo.transform.forward = -actor.transform.forward;
        pgo.velocity = Vector3.zero;

        yield return null;

        pgo.position = actor.transform.position + actor.transform.forward * 2f;
        pgo.transform.forward = -actor.transform.forward;
        pgo.velocity = Vector3.zero;

        StartCamera(actor.gameObject);
        //ConversationSequence.RequestConversationCamera.Get(actor.gameObject, null, this);

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        PlayerController.LockMovement(this);

        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}