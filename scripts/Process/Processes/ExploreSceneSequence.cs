using UnityEngine; 
using System.Collections; 

public class ExploreSceneSequence : IProcess<object, object> {

    public event ProcessExitCallback OnExit;

    bool enabled = true;

    public void Initialize(object param1) {
        CrystallizeEventManager.Input.OnEnvironmentClick += HandleEnvironmentClick;
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= HandleEnvironmentClick;
    }

    void HandleEnvironmentClick(object sender, System.EventArgs args) {
        if (!enabled) {
            return;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit)) {
            SelectAction(hit.collider.GetBaseGameObject());
        }
    }

    void Pause() {
        enabled = false;
    }

    void Resume() {
        enabled = true;
    }

    void SelectAction(GameObject target) {
        var dialogue = target.GetInterface<IInstanceReference<DialogueSequence>>();
        if (dialogue != null) {
            StartDialogue(target);
        }
    }

    void StartDialogue(GameObject target) {
        Pause();
        var args = new ConversationArgs(target, target.GetInterface<IInstanceReference<DialogueSequence>>().Instance);
        ProcessLibrary.Conversation.Get(args, OnDialogueExit, this);
    }

    void OnDialogueExit(object sender, object args) {
        Resume();
    }

}
