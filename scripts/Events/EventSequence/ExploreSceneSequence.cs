using UnityEngine; 
using System.Collections; 

public class ExploreSceneSequence : MonoBehaviour {

    public static ProcessRequestHandler<GameObject, object> RequestDialogue;

    void Start() {
        
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (!UISystem.MouseOverUI()) {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit)) {
                    SelectAction(hit.collider.GetBaseGameObject());
                }
            }
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
        //Debug.Log("DialogueRequested");
        Pause();
        RequestDialogue(this, new ProcessRequestEventArgs<GameObject,object>(target, OnDialogueExit));
    }

    void OnDialogueExit(object sender, ProcessExitEventArgs<object> args) {
        Resume();
    }

}
