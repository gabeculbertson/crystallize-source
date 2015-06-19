using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionLog : MonoBehaviour {

    public static InteractionLog main { get; set; }

    public static void LogInteraction(GameObject player, PhraseSequence phrase) {
        if (main) {
            var id = -1;
            if (player.CompareTag("Player")) {
                id = 0;
            } else if(player.CompareTag("OtherPlayer")){
                id = 1;
            }
            if (id != -1) {
                main._AddEntry(id, phrase);
            }
        }
    }

    List<InteractionLogEntry> entries = new List<InteractionLogEntry>();

    public List<InteractionLogEntry> Entries {
        get {
            return entries;
        }
    }

    void Awake() {
        main = this;
    }

	// Use this for initialization
	void Start () {
        entries.Add(InteractionLogEntry.Break);
	}
	
	// Update is called once per frame
	void Update () {
        if (!InteractionManager.IsInteractingWithOtherPlayer()) {
            if (entries.Last() != InteractionLogEntry.Break) {
                entries.Add(InteractionLogEntry.Break);
            }
        }
	}

    void _AddEntry(int player, PhraseSequence phrase) {
        entries.Add(new InteractionLogEntry(player, phrase));
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new InteractionLoggedEventArgs());
    }

}
