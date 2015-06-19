using UnityEngine;
using System.Collections;

public class InteractiveDialogTrigger : MonoBehaviour {

    InteractiveDialogActor actor;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Initialize(InteractiveDialogActor actor) {
        this.actor = actor;
    }

    void OnTriggerEnter(Collider other) {
        if (!other.attachedRigidbody) {
            return;
        }

        if (other.attachedRigidbody.tag == "Player") {
            var triggerConsumer = other.attachedRigidbody.gameObject.GetInterfaceInChildren<ITriggerConsumer>();
            if (triggerConsumer != null) {
                triggerConsumer.TriggerEntered(gameObject);
            }
            actor.TriggerEntered();
        }
    }

    void OnTriggerExit(Collider other) {
        if (!other.attachedRigidbody) {
            return;
        }

        if (other.attachedRigidbody.tag == "Player") {
            var triggerConsumer = other.attachedRigidbody.gameObject.GetInterfaceInChildren<ITriggerConsumer>();
            if (triggerConsumer != null) {
                triggerConsumer.TriggerExited(gameObject);
            }
            actor.CloseDialogue();
        }
    }

}