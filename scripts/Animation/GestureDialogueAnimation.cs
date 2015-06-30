using UnityEngine;
using System.Collections;

public class GestureDialogueAnimation : DialogueAnimation {

    public string Animation { get; set; }

    public override event System.EventHandler OnComplete;

    Animator animator;

    public GestureDialogueAnimation() {
        Animation = "";
    }

    public GestureDialogueAnimation(string animation) {
        Animation = animation;
    }

    public override void Play(GameObject actor) {
        animator = actor.GetComponentInChildren<Animator>();

        CoroutineManager.Instance.StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence() {
        animator.CrossFade(Animation, 0.1f);

        yield return new WaitForSeconds(0.5f);
        //while (!animator.GetCurrentAnimatorStateInfo(0).IsName(Animation)) {
        //    yield return null;
        //}
        
        //while (animator.GetCurrentAnimatorStateInfo(0).IsName(Animation)) {
        //    yield return null;
        //}

        OnComplete.Raise(this, System.EventArgs.Empty);
    }

}