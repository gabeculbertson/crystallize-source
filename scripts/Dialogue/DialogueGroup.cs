using UnityEngine;
using System;
using System.Collections;
using Crystallize;

public class DialogueGroup : MonoBehaviour, ITriggerEventHandler, IInteractionPoint {

    public DialogueActor[] actors;

    Coroutine playSequence;

	// Use this for initialization
	void Start () {
        foreach (var a in actors) {
            a.inGroup = true;
        }
	}

    void OnEnable() {
        ActorTracker.AddActor(gameObject);
    }

    void OnDisable() {
        ActorTracker.RemoveActor(gameObject);
    }

    public void BeginDialogue(NPCDialogue dialogue) {
        playSequence = StartCoroutine(RunDialogue(dialogue));
    }

    public IEnumerator RunDialogue(NPCDialogue dialogue) {
        for(int i = 0; i < actors.Length; i++){
            if(i != dialogue.ActorIndicies[0]){
                actors[i].SetPhrase(new PhraseSequence("..."));
            }
        }

        var cd = GameData.Instance.DialogueData.PersonContextData.GetItem(transform.GetWorldID());
        foreach (var l in dialogue.Lines) {
            var i = dialogue.GetActorIndex(l);
            if (!IndexValid(i)) {
                i = 0;
            }
            actors[i].SetLine(l, cd);

            yield return new WaitForSeconds(GetLineDuration(l));
        }
    }

    float GetLineDuration(DialogueActorLine line) {
        var a = line.GetAudioClip();
        //Debug.Log(a + "; " + a.length);
        if (a != null) {
            return a.length;
        } else {
            return line.Phrase.PhraseElements.Count * 0.4f;
        }
    }

    bool IndexValid(int index) {
        if (index < 0) {
            return false;
        }

        if (index >= actors.Length) {
            return false;
        }

        return true;
    }

    public void StopDialogue() {
        if (playSequence != null) {
            StopCoroutine(playSequence);
            playSequence = null;
        }

        foreach (var a in actors) {
            a.SetPhrase(null);
        }
    }

    public void HandleTriggerEntered(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            CrystallizeEventManager.Environment.RaiseActorApproached(this, EventArgs.Empty);
        }
    }

    public void HandleTriggerExited(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            StopDialogue();
            CrystallizeEventManager.Environment.RaiseActorDeparted(this, EventArgs.Empty);
        }
    }

    public bool GetInteractionEnabled() {
        return false;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public float GetRadius() {
        return 2f;
    }
}
