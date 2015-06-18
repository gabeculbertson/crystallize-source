using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crystallize;

public class TutorialLevel07Script : LevelScript {

    enum TutorialObjective {
        ApproachPerson,
        ClickSlot,
        ChooseWord,
        Complete
    }

    public int clientID;

    bool complete = false;

	// Use this for initialization
	IEnumerator Start () {
		while (!LevelSystemConstructor.main) {
			yield return null;
		}

        PlayerData.Instance.Flags.SetFlag(FlagPlayerData.ClickWordSlotMessage, true);

		yield return null;

        ObjectiveManager.main.SetObjective(this, false);

        PlayerController.LockMovement(this);

		SetMessage ("Access the full inventory by pressing the the button in the lower-right of the screen.");

        var rt = TutorialCanvas.main.FullInventoryButton.GetRectTransform();
        //Debug.Log(rt);
        TutorialCanvas.main.CreateUIDragBox(rt, "Click here!");

        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.UI.OnUIRequested -= HandleUIRequested;

        TutorialCanvas.main.ClearAllIndicators();

        SetMessage("Organize to help yourself access important words quickly. Close the by pressing the red 'x' when you are done.");

        while (UISystem.main.ContainsCenterPanel()) {
            yield return null;
        }

        PlayerController.UnlockMovement(this);
        SetMessage("Finish the conversation to continue.");

        yield return StartCoroutine(RunStateMachine<TutorialObjective>(GetObjective, SetObjective, TutorialObjective.Complete));

        TutorialCanvas.main.ClearAllIndicators();

        while (!PlayerData.Instance.Conversation.GetConversationComplete(clientID)) {
            yield return null;
        }

        ObjectiveManager.main.SetObjective(this, true);
	}

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is FullInventoryUIRequestEventArgs) {
            Continue(sender, e);
        }
    }

    TutorialObjective GetObjective() {
        if (complete) {
            return TutorialObjective.Complete;
        }

        if (PlayerData.Instance.Conversation.GetConversationComplete(clientID)) {
            return TutorialObjective.Complete;
        }

        if (DialogueSystemManager.main.InteractionTarget) {
            if (!DialogueSystemManager.main.InteractionTarget.GetComponent<DialogueActor>()) {
                return TutorialObjective.ApproachPerson;
            }
        } else {
            return TutorialObjective.ApproachPerson;
        }

        if (TutorialCanvas.main.GetRegisteredGameObject("WordSelector")) {
            return TutorialObjective.ChooseWord;
        }

        return TutorialObjective.ClickSlot;
    }

    void SetObjective(TutorialObjective obj) {
        var inventory = TutorialCanvas.main.InventoryUI;
        inventory.gameObject.SetActive(true);
        TutorialCanvas.main.ClearAllIndicators();
        switch (obj) {
            case TutorialObjective.ApproachPerson:
                SetMessage("Finish the conversation to continue.");
                break;

            case TutorialObjective.ClickSlot:
                inventory.gameObject.SetActive(false);
                SetMessage("Click an empty word slot to open the word selection menu.");
                var slot = GameObject.FindObjectOfType<PlayerSpeechBubbleDropAreaUI>();
                if (!slot) {
                    Debug.Log("No slot!");
                } else {
                    TutorialCanvas.main.CreateUIDragBox(slot.GetComponent<RectTransform>(), "Click here");
                }
                break;

            case TutorialObjective.ChooseWord:
                inventory.gameObject.SetActive(false);
                SetMessage("Click a word to choose it.");
                complete = true;
                break;
        }
    }


}
