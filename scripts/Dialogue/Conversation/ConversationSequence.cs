using UnityEngine;
using System;
using System.Collections;

public class ConversationSequence : MonoBehaviour, ISelectionSequence<object> {

    public static ConversationSequence GetInstance(DialogueSequence dialogue) {
        var go = new GameObject("Conversation");
        var c = go.AddComponent<ConversationSequence>();
        c.dialogue = dialogue;
        return c;
    }

    public GameObject cameraController;
    public DialogueActor actor;

    public event EventHandler OnCancel;
    public event EventHandler OnExit;
    public event SequenceCompleteCallback<object> OnSelection;

    DialogueSequence dialogue;
    GameObject cameraControllerInstance;
    bool running;

    void Start() {
        //SetDialogueElement(GetComponent<DialogueActor>(), d, 0);
    }

    void Begin() {
        if (cameraControllerInstance) {
            Destroy(cameraControllerInstance);
        }

        cameraControllerInstance = Instantiate<GameObject>(cameraController);
        cameraControllerInstance.GetComponent<ConversationCameraController>().conversationTarget = transform;
        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        var pgo = PlayerManager.main.PlayerGameObject;
        pgo.transform.position = transform.position + transform.forward * 2f;
        pgo.transform.forward = -transform.forward;

        PlayerController.LockMovement(this);

        running = true;
    }

    void SetDialogueElement(DialogueActor actor, DialogueSequence dialogue, int currentElement) {
        actor.SetPhrase(dialogue.GetElement(currentElement).Phrase);

        //Debug.Log(dialogue.GetElementType(currentElement));
        switch (dialogue.GetElementType(currentElement)) {
            case DialogueElementType.Linear:

                CrystallizeEventManager.UI.RaisePromptLinearDialogueContinue(this, new DialogueSequenceEventArgs(dialogue, currentElement));
                //CrystallizeEventManager.UI.OnResolveLinearDialogueContinue += HandleResolveLinearDialogueContinue;
                break;
            case DialogueElementType.End:
                CrystallizeEventManager.UI.RaisePromptEndDialogueContinue(this, new DialogueSequenceEventArgs(dialogue, currentElement));
                //CrystallizeEventManager.UI.OnResolveEndDialogueContinue += HandleResolveEndDialogueContinue;
                break;
            case DialogueElementType.Prompted:
                CrystallizeEventManager.UI.RaisePromptPromptDialogueContinue(this, new DialogueSequenceEventArgs(dialogue, currentElement));
                //CrystallizeEventManager.UI.OnResolvePromptDialogueContinue += HandleResolveLinearDialogueContinue;
                break;
        }
    }

}