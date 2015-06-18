using UnityEngine;
using System.Collections;

public class MultiplayerPointTutorialScript : LevelScript {

    enum TutorialObjective {
        AcceptQuest,
        WaitForPartnerAcceptQuest,
        Point,
        GoToPartnerCursor,
        WaitForPartnerComplete,
        Complete
    }

    public Transform questClient;

	// Use this for initialization
	IEnumerator Start () {
        ObjectiveManager.main.SetObjective(this, false);

        SetMessage("Approach the teacher to start the quest.");

        yield return null;

        yield return StartCoroutine(RunStateMachine<TutorialObjective>(GetObjective, SetObjective, TutorialObjective.Complete));

        ObjectiveManager.main.SetObjective(this, true);
	}

    TutorialObjective GetObjective() {
        var qi = GetQuestInstance(questClient);
        if (qi.State == ObjectiveState.Complete) {
            return TutorialObjective.Complete;
        }
        
        if (qi.State != ObjectiveState.Active) {
            return TutorialObjective.AcceptQuest;
        }

        if (!qi.GetObjectiveState(0).IsComplete) {
            return TutorialObjective.WaitForPartnerAcceptQuest;
        }

        if (!qi.GetObjectiveState(1).IsComplete) {
            return TutorialObjective.Point;
        }

        if (qi.GetObjectiveState(2).IsComplete) {
            return TutorialObjective.WaitForPartnerComplete;
        }

        return TutorialObjective.GoToPartnerCursor;
    }

    void SetObjective(TutorialObjective objective) {
        TutorialCanvas.main.ClearAllIndicators();
        switch (objective) {
            case TutorialObjective.AcceptQuest:
                SetMessage("Accept the quest.");
                break;

            case TutorialObjective.WaitForPartnerAcceptQuest:
                SetMessage("Wait for your partner to accept the quest.");
                break;

            case TutorialObjective.Point:
                SetMessage("Point anywhere in the environment with the mouse by clicking the <b>right mouse button</b>.");
                break;

            case TutorialObjective.GoToPartnerCursor:
                SetMessage("Go to where your partner is pointing to. The location is shown by a blue target.");
                break;

            case TutorialObjective.WaitForPartnerComplete:
                SetMessage("Wait for your partner to go where you are pointing. Make sure they can reach the spot you are pointing to.");
                break;
        }
    }
	
}
