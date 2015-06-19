using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectiveSlotColoring : MonoBehaviour {

    PhraseSequenceElement word;
    bool isHighlighted = false;

    public void Initialize(PhraseSequenceElement word) {
        this.word = word;
        GetComponentInChildren<Text>().text = word.GetTranslation();
        GetComponent<Image>().color = GUIPallet.main.darkGray;

        if (DialogueSystemManager.main) {
            if (DialogueSystemManager.main.Mode == InteractionType.LinearDialogue
                && DialogueSystemManager.main.InteractionTarget) {
                var worldID = DialogueSystemManager.main.InteractionTarget.transform.GetWorldID();
                var ld = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(worldID);
                if (ld != null) {
                    var words = ld.GetNeededWords();
                    foreach (var w in words) {
                        if (PhraseSequenceElement.IsEqual(w, word)) {
                            isHighlighted = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    void Start() {
        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
    }

    void Update() {
        if (isHighlighted) {
            GetComponent<Image>().color = Color.Lerp(Color.yellow, GUIPallet.main.darkGray, Mathf.PingPong(Time.time, 1f));
        }
    }

    void OnDestroy() {
        CrystallizeEventManager.Environment.OnActorApproached -= HandleActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted -= HandleActorDeparted;
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        var worldID = ((Component)sender).GetWorldID();
        var ld = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(worldID);
        if (ld != null) {
            var words = ld.GetNeededWords();
            foreach (var w in words) {
                if (PhraseSequenceElement.IsEqual(w, word)) {
                    isHighlighted = true;
                    break;
                }
            }
        }
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        isHighlighted = false;
    }

}
