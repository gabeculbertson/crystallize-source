using UnityEngine;
using System;
using System.Collections;

public class MovementDialogueAnimation : DialogueAnimation {

    public float Speed { get; set; }
    public string Target { get; set; }

    public override event EventHandler OnComplete;

    public MovementDialogueAnimation() {
        Speed = 3f;
        Target = "";
    }

    public override void Play(GameObject actor) {
        CoroutineManager.Instance.StartCoroutine(MoveToTarget(actor));
    }

    IEnumerator MoveToTarget(GameObject actor) {
        var t = GameObject.Find(Target).transform;

        while (Vector3.Distance(actor.transform.position, t.position) > 0.05f) {
            t.GetComponent<Rigidbody>().position = Vector3.MoveTowards(t.transform.position, t.position, Speed * Time.deltaTime);
            
            yield return null;
        }

        OnComplete.Raise(this, EventArgs.Empty);
    }

}