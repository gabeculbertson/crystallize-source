using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerEnglishTutorialScript : LevelScript {

    enum TutorialObjective {
        Empty,
        ApproachTeacher,
        PressQuestConfirm,
        WaitForPartnerJoin,
        PressSpace,
        TypeMessage,
        PressMessageConfirm,
		WaitForPartnerComplete,
        Complete
    }

    public GameObject movementTutorialPrefab;

    public Transform questClient;
    public Transform movementTrigger;

	bool partnerJoined = false;

	IEnumerator Start () {
		ObjectiveManager.main.SetObjective(this, false);
		StartCoroutine (WaitAndActivateQuest ());
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;

        yield return null;

        // Teach the player to move
        SetMessage("Move with WASD.");
        var movementTutorialInstance = Instantiate<GameObject>(movementTutorialPrefab);
        TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.GetRegisteredGameObject("HelpBox").GetComponent<RectTransform>(), "Help is shown here");

        movementTrigger.GetComponent<TriggerEventObject>().OnTriggerExitEvent += HandleTriggerExit;
        yield return StartCoroutine(WaitForEvent());
        movementTrigger.GetComponent<TriggerEventObject>().OnTriggerExitEvent -= HandleTriggerExit;

        Destroy(movementTutorialInstance);
        
		SetMessage ("Wait for your partner to join the game.");
		while (!partnerJoined) {
			yield return null;
		}

		// Figure out what state we are in and display the appropriate messages
        yield return StartCoroutine(RunStateMachine<TutorialObjective>(GetTutorialObjective, SetTutorialState, TutorialObjective.Complete));

		ObjectiveManager.main.SetObjective (this, true);
	}

	IEnumerator WaitAndActivateQuest(){
		partnerJoined = false;
		questClient.gameObject.SetActive (false);
		while (PlayerManager.Instance.PlayerCount < 2) {
			yield return null;
		}
		questClient.gameObject.SetActive (true);
		partnerJoined = true;
	}

    void HandleTriggerExit(object sender, TriggerEventArgs args) {
        if (args.Collider.IsPlayer()) {
            Continue();
        }
    }

    void HandleQuestStateChanged(object sender, QuestStateChangedEventArgs args) {
        var i = GetQuestInstance(questClient);
        if (i.State == ObjectiveState.Complete) {
            ObjectiveManager.main.SetObjective(this, true);
        }
    }

    TutorialObjective GetTutorialObjective() {
        var i = GetQuestInstance(questClient);
        switch (i.State) {
            case ObjectiveState.Active:
				if(i.GetObjectiveState(1).IsComplete){
					return TutorialObjective.WaitForPartnerComplete;
				} else if(i.GetObjectiveState(0).IsComplete) {
					var englishInput = TutorialCanvas.main.GetRegisteredGameObject("EnglishInput");
					if (!englishInput.activeInHierarchy) {
						return TutorialObjective.PressSpace;
					}

	                if (englishInput.GetComponentInChildren<InputField>().text == "") {
	                    return TutorialObjective.TypeMessage;
	                } else {
	                    return TutorialObjective.PressMessageConfirm;
	                }
				} else {
					return TutorialObjective.WaitForPartnerJoin;
				}

            case ObjectiveState.Complete:
                return TutorialObjective.Complete;

            default:
                if (DialogueSystemManager.main.InteractionTarget) {
                    return TutorialObjective.PressQuestConfirm;
                } else {
                    return TutorialObjective.ApproachTeacher;
                }
        }
    }

    void SetTutorialState(TutorialObjective objective) {
        TutorialCanvas.main.ClearAllIndicators();
        switch (objective) {
            case TutorialObjective.ApproachTeacher:
                SetMessage("Approach the teacher.");
                break;

            case TutorialObjective.PressQuestConfirm:
                SetMessage("Accept the quest.");
                var qcb = TutorialCanvas.main.GetRegisteredGameObject("QuestConfirmButton");
                TutorialCanvas.main.CreateUIDragBox(qcb.GetComponent<RectTransform>(), "click here");
                break;

            case TutorialObjective.WaitForPartnerJoin:
                SetMessage("Wait for your partner to accept the quest.");
                break;

            case TutorialObjective.PressSpace:
                SetMessage("Press the space bar to begin entering text.");
                break;

            case TutorialObjective.TypeMessage:
                SetMessage("Type a message.");
                break;

            case TutorialObjective.PressMessageConfirm:
                SetMessage("Press the confirm button or hit enter to say your message.");
                break;

			case TutorialObjective.WaitForPartnerComplete:
				SetMessage("Wait for your partner to finish the task.");
				break;
        }
    }

}
