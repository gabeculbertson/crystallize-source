using UnityEngine;
using System.Collections;

public class MultiplayerBranchesTutorialScript : LevelScript {

    public Transform questClient;

	// Use this for initialization
	IEnumerator Start () {
        ObjectiveManager.main.SetObjective(this, false);
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;

        while(GetQuestInstance(questClient).State != ObjectiveState.Active){
            yield return null;
        }

        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
        CrystallizeEventManager.UI.OnUIInteraction += HandleUIInteraction;
	}

    void HandleUIInteraction(object sender, System.EventArgs e) {
        if (e is DialogueBranchSelectedEventArgs) {
            TutorialCanvas.main.ClearAllIndicators();
        }
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        StartCoroutine(TryToProduceGuideBox());
    }

    IEnumerator TryToProduceGuideBox() {
        yield return null;
        if (DialogueSystemManager.main.InteractionTarget == questClient.gameObject) {
            yield break;
        }

        var choiceBox = GameObject.FindObjectOfType<BranchUI>();
        SetMessage("Choose how to intereact by clicking an option.");
        TutorialCanvas.main.CreateUIDragBox(choiceBox.GetComponent<RectTransform>(), "choose here");
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        TutorialCanvas.main.ClearAllIndicators();
    }

    void HandleQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
        if (GetQuestInstance(questClient).State == ObjectiveState.Complete) {
            ObjectiveManager.main.SetObjective(this, true);
        }
    }
}
