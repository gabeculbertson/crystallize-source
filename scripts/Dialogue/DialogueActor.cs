using UnityEngine;
using System;
using System.Collections;

public class DialogueActor : MonoBehaviour, ITriggerEventHandler, ISpeechTextSource, IPerson, IQuestInteractionPoint {

    public string actorName = "???";
    [NonSerialized]
    public bool inGroup = false;

    bool isRelevant = false;

    public PhraseSequence CurrentPhrase { get; set; }

    public event EventHandler<PhraseEventArgs> OnSpeechTextChanged;

    void Start() {
        //FloatingNameUI.main.SetName(this, name);
    }

    void OnEnable() {
        if (!transform.IsHumanControlled()) {
            ActorTracker.AddActor(gameObject);
        }
    }

    void OnDisable() {
        ActorTracker.RemoveActor(gameObject);
    }

    public void SetPhrase(PhraseSequence phrase, bool hasMore = false) {
        CurrentPhrase = phrase;
        // TODO: remove
        if (OnSpeechTextChanged != null) {
            OnSpeechTextChanged(this, new PhraseEventArgs(phrase, hasMore));
        }
        var canEdit = gameObject == PlayerManager.Instance.PlayerGameObject;
        var checkGrammar = gameObject.CompareTag("Player") || gameObject.CompareTag("OtherPlayer");
        var speechArgs = new SpeechBubbleRequestedEventArgs(transform, phrase, hasMore, canEdit, checkGrammar);
        CrystallizeEventManager.UI.RaiseSpeechBubbleRequested(this, speechArgs);
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, speechArgs);

        // TODO: no
        InteractionLog.LogInteraction(gameObject, phrase);
    }

    public void SetLine(DialogueActorLine line, ContextData context = null, bool hasMore = false) {
        if (line == null) {
            SetPhrase(null);
        } else {
            if (context != null) {
                SetPhrase(line.Phrase.InsertContext(context), hasMore);
            } else {
                SetPhrase(line.Phrase, hasMore);
            }

            var ac = GetAudioClip(line);
            if (ac) {
                PlayAudio(ac);
            }
        }
    }

    AudioClip GetAudioClip(DialogueActorLine line) {
        var ac = line.GetAudioClip();

        var overrideAudio = GetComponent<NPCDialogueOverrideAudio>();
        if (overrideAudio) {
            if (overrideAudio.GetAudioClip(line)) {
                ac = overrideAudio.GetAudioClip(line);
            }
        }

        return ac;
    }

    void PlayAudio(AudioClip clip) {
        if (!clip) {
            return;
        }

        if (!GetComponent<AudioSource>()) {
            gameObject.AddComponent<AudioSource>();
        }
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().time = 0;
        GetComponent<AudioSource>().Play();
    }

    public void HandleTriggerEntered(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            CrystallizeEventManager.Environment.RaiseActorApproached(this, EventArgs.Empty);
        }
    }

    public void HandleTriggerExited(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            SetPhrase(null);
            CrystallizeEventManager.Environment.RaiseActorDeparted(this, EventArgs.Empty);
        }
    }

    public bool GetInteractionEnabled() {
        if (InteractionManager.GetInteractionTarget() == gameObject) {
            return true;
        }

        return isRelevant;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public float GetRadius() {
        return 1f;
    }

    public void SetRelevant(bool relevant) {
        isRelevant = relevant;
    }
}
