using UnityEngine;
using System.Collections;

public class EndConversationProcess : ConversationProcessPart, IProcess<ConversationArgs, object> {


    DialogueActor actor;
    ContextData context;

    public event ProcessExitCallback OnExit;

    public void Initialize(ConversationArgs args) {
        actor = args.Target.GetComponent<DialogueActor>();
        context = args.Context;

        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        actor.SetPhrase(null);
        PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        PlayerController.UnlockMovement(this);
        StopCamera();

        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        OnExit.Raise(this, null);
    }
}