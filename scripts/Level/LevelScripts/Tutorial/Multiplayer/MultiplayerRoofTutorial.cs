using UnityEngine;
using System.Collections;

public class MultiplayerRoofTutorial : LevelScript {

    public Transform questClient;

    void Start() {
        ObjectiveManager.main.SetObjective(this, false);
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += main_OnQuestStateChanged;

        //DialogueSystemManager
        CrystallizeEventManager.UI.OnUIRequested += HandleOnUIRequested;
        CrystallizeEventManager.UI.OnSpeechBubbleOpen += HandleSpeechBubbleOpen;
        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;

        SetMessage("Complete the quest.");
    }

    void main_OnQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
        if (GetQuestInstance(questClient).State == ObjectiveState.Complete) {
            ObjectiveManager.main.SetObjective(this, true);
        }
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        var c = (Component)sender;
        if (c.GetComponent<SpeechStatusEffect>()) {
            if (!c.GetComponent<SpeechStatusEffect>().yourState) {
                if (PlayerManager.main.PlayerID == 0) {
                    SetMessage("This person is too shy to talk to guys...");
                } else {
                    SetMessage("This person is too shy to talk to girls...");
                }
            }
        }
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        ClearMessages();
    }

    void HandleOnUIRequested(object sender, UIRequestEventArgs e) {
        if (e is BranchUIRequestEventArgs) {
            SetMessage("Click a choice to begin the interaction.", 10f);
        }
    }

    void HandleSpeechBubbleOpen(object sender, PhraseEventArgs e) {
        //if (e.Phrase == null) {
        //    return;
        //}

        

        //if (e.Phrase.PhraseElements[0].Text == "...") {
        //    if (PlayerManager.main.PlayerID == 0) {
        //        SetMessage("This person is too shy to talk to guys...");
        //    } else {
        //        SetMessage("This person is too shy to talk to girls...");
        //    }
        //}
    }

}
