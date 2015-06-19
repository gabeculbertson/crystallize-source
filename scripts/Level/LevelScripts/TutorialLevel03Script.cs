using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialLevel03Script : LevelScript {

	public InteractiveDialogActor actor;
	public GameObject arrow;
    public int wordID;

	// Use this for initialization
	IEnumerator Start () {
		arrow.SetActive (false);

		while (!LevelSystemConstructor.main) {
			yield return null;
		}

		yield return null;

        SetMessage("Complete the conversation to continue");

		//actor.OnOpenDialog += Continue;
        CrystallizeEventManager.Environment.OnActorApproached += main_OnActorApproached;
		yield return StartCoroutine (WaitForEvent ());
        CrystallizeEventManager.Environment.OnActorApproached -= main_OnActorApproached;
        //actor.OnOpenDialog -= Continue;

        SetMessage("When a conversation requires additional words, these will be shown in the inventory. " +
        	"Leave the conversation to find them.");
        var e = TutorialCanvas.main.InventoryUI.GetEntry(new PhraseSequenceElement(wordID, 0));
        TutorialCanvas.main.CreateUIDragBox(e, "New objective word!");

        CrystallizeEventManager.Environment.OnActorDeparted += main_OnActorApproached;
        yield return StartCoroutine(WaitForEvent());
        CrystallizeEventManager.Environment.OnActorDeparted -= main_OnActorApproached;

        TutorialCanvas.main.ClearAllIndicators();
        SetMessage("Approach nearby characters to listen to their conversation and find new words");
		arrow.SetActive (true);

		var inventoryWords = PlayerData.Instance.WordStorage;
		while (true) {
			if(inventoryWords.ContainsFoundWord(wordID)){
				break;
			}

			yield return null;
		}

		arrow.SetActive (false);
	}

    void main_OnActorApproached(object sender, EventArgs e) {
        Continue();
    }


}
