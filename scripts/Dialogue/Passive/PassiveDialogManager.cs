using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PassiveDialogManager : MonoBehaviour {

    const float DialogDistanceThreshold = 4f;

    public static PassiveDialogManager main { get; set; }

    public GameObject dialogTriggerPrefab;

    HashSet<PassiveDialogActor> actors;

    Queue<GameObject> dialogTriggerInstances = new Queue<GameObject>();

    public event EventHandler OnDialogOpened;

    void Awake() {
        main = this;
    }

    // Use this for initialization
    void Start() {
        actors = new HashSet<PassiveDialogActor>(GetComponentsInChildren<PassiveDialogActor>());

        CreateDialogTriggers();
    }

    // Update is called once per frame
    void Update() {

    }

    void CreateDialogTriggers() {
        while (dialogTriggerInstances.Count > 0) {
            Destroy(dialogTriggerInstances.Dequeue());
        }

        var remainingActors = new HashSet<PassiveDialogActor>(actors);
        var removedActors = new Queue<PassiveDialogActor>();
        foreach (var actor in remainingActors) {
            if (actor.isSolo) {
                CreateDialogTrigger(actor);

                removedActors.Enqueue(actor);
            }
        }

        while (removedActors.Count > 0) {
            remainingActors.Remove(removedActors.Dequeue());
        }

        while (remainingActors.Count > 1) {
            var currentActor = remainingActors.First();
            remainingActors.Remove(currentActor);

            var shortest = float.PositiveInfinity;
            PassiveDialogActor closestActor = null;
            foreach (var a in remainingActors) {
                var dist = Vector3.Distance(a.transform.position, currentActor.transform.position);
                if (dist < shortest) {
                    shortest = dist;
                    closestActor = a;
                }
            }

            if (shortest < DialogDistanceThreshold) {
                remainingActors.Remove(closestActor);
                CreateDialogTrigger(currentActor, closestActor);
            }
        }
    }

    void CreateDialogTrigger(PassiveDialogActor actor) {
        Vector3 center = actor.transform.position;
        var go = (GameObject)Instantiate(dialogTriggerPrefab, center, Quaternion.identity);
        go.GetComponent<PassiveDialogTrigger>().Initialize(actor);
        go.GetComponent<PassiveDialogTrigger>().OnDialogOpened += HandleOnDialogOpened;
    }

    void CreateDialogTrigger(PassiveDialogActor actor1, PassiveDialogActor actor2) {
        Vector3 center = Vector3.Lerp(actor1.transform.position, actor2.transform.position, 0.5f);
        var go = (GameObject)Instantiate(dialogTriggerPrefab, center, Quaternion.identity);
        go.GetComponent<PassiveDialogTrigger>().Initialize(actor1, actor2);
        go.GetComponent<PassiveDialogTrigger>().OnDialogOpened += HandleOnDialogOpened;
    }

    void HandleOnDialogOpened(object sender, EventArgs e) {
        if (OnDialogOpened != null) {
            OnDialogOpened(sender, e);
        }
    }


}