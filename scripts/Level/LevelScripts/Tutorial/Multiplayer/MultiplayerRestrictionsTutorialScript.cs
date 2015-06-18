using UnityEngine;
using System.Collections;

public class MultiplayerRestrictionsTutorialScript : LevelScript {

    public Transform questClient;

	IEnumerator Start () {
        ObjectiveManager.main.SetObjective(this, false);
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;

        SetMessage("Complete the quest.");

        while (GetQuestInstance(questClient).State != ObjectiveState.Active) {
            yield return new WaitForSeconds(0.2f);
        }

        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        TutorialCanvas.main.ClearAllIndicators();
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        var statusBubbles = GameObject.FindObjectsOfType<StatusThoughtBubbleUI>();
        foreach (var sb in statusBubbles) {
            TutorialCanvas.main.CreateUIDragBox(sb.GetComponent<RectTransform>(), "look here");
        }
        SetMessage("Look at the thought bubbles to see who someone will talk to.");
    }

    

    void HandleQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
        if (GetQuestInstance(questClient).State == ObjectiveState.Complete) {
            ObjectiveManager.main.SetObjective(this, true);
        }
    }
	
}
