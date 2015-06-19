using UnityEngine;
using System.Collections;
using Crystallize;

public class ConversationStart_tmp : MonoBehaviour {

    public GameObject cameraController;

    public DialogueSequenceUnityXml dialogue = new DialogueSequenceUnityXml();

    bool running = false;

    GameObject cameraControllerInstance;

	// Use this for initialization
	void Start () {
        
	}

    void Begin()
    {
        if (cameraControllerInstance)
        {
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
        //ContinueDialogue();
    }

    void End()
    {
        if (cameraControllerInstance)
        {
            Destroy(cameraControllerInstance);
        }

        PlayerController.UnlockMovement(this);

        GetComponent<DialogueActor>().SetPhrase(null);
        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Speaking));

        running = false;
    }

    void SetDialogueElement(DialogueActor actor, DialogueSequence dialogue, int currentElement)
    {
        actor.SetPhrase(dialogue.GetElement(currentElement).Phrase);

        //Debug.Log(dialogue.GetElementType(currentElement));
        switch (dialogue.GetElementType(currentElement))
        {
            case DialogueElementType.Linear:
                CrystallizeEventManager.UI.RaisePromptLinearDialogueContinue(this, new DialogueSequenceEventArgs(dialogue, currentElement));
                CrystallizeEventManager.UI.OnResolveLinearDialogueContinue += HandleResolveLinearDialogueContinue;
                break;
            case DialogueElementType.End:
                CrystallizeEventManager.UI.RaisePromptEndDialogueContinue(this, new DialogueSequenceEventArgs(dialogue, currentElement));
                CrystallizeEventManager.UI.OnResolveEndDialogueContinue += HandleResolveEndDialogueContinue;
                break;
            case DialogueElementType.Prompted:
                CrystallizeEventManager.UI.RaisePromptPromptDialogueContinue(this, new DialogueSequenceEventArgs(dialogue, currentElement));
                CrystallizeEventManager.UI.OnResolvePromptDialogueContinue += HandleResolveLinearDialogueContinue;
                break;
        }
    }

    void HandleResolveLinearDialogueContinue(object sender, DialogueSequenceEventArgs args)
    {
        CrystallizeEventManager.UI.OnResolveLinearDialogueContinue -= HandleResolveLinearDialogueContinue;
        SetDialogueElement(GetComponent<DialogueActor>(), args.Dialogue,args.CurrentElement);
    }

    void HandleResolveEndDialogueContinue(object sender, DialogueSequenceEventArgs args)
    {
        CrystallizeEventManager.UI.OnResolveLinearDialogueContinue -= HandleResolveLinearDialogueContinue;
        CrystallizeEventManager.UI.OnResolveEndDialogueContinue -= HandleResolveEndDialogueContinue;
        End();
    }
    
	// Update is called once per frame
	IEnumerator OnMouseUp () {
        yield return null;

        if (!running)
        {
            Begin();
            var d = dialogue.GetObject();
            SetDialogueElement(GetComponent<DialogueActor>(), d, 0);
        }
	}
}
