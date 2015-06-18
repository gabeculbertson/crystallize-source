using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crystallize;

public class TutorialLevel08Script : LevelScript {

    enum TutorialObjective {
        ApproachPerson,
        LookUpWord,
        Complete
    }

    //public int wordID;

	// Use this for initialization
	IEnumerator Start () {
		while (!LevelSystemConstructor.main) {
			yield return null;
		}

		yield return null;

        PlayerData.Instance.Flags.SetFlag(FlagPlayerData.DictionaryUnlocked, true);
        TutorialCanvas.main.GetRegisteredGameObject("QuickDictionary").GetComponent<QuickDictionaryUI>().Initialize();
        TutorialCanvas.main.GetRegisteredGameObject("QuickDictionary").GetComponent<QuickDictionaryUI>().ForceOpen = true;

        ObjectiveManager.main.SetObjective(this, false);

        SetMessage("Approach the people.");

        yield return StartCoroutine(RunStateMachine<TutorialObjective>(GetObjective, SetObjective, TutorialObjective.Complete));

        TutorialCanvas.main.ClearAllIndicators();

        ObjectiveManager.main.SetObjective(this, true);
	}

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is FullInventoryUIRequestEventArgs) {
            Continue(sender, e);
        }
    }

    TutorialObjective GetObjective() {
        var qd = TutorialCanvas.main.GetRegisteredGameObject("QuickDictionary");
        if (qd.GetComponent<QuickDictionaryUI>().Word != null) {
            //if (qd.GetComponent<QuickDictionaryUI>().Word.WordID == wordID) {
                return TutorialObjective.Complete;
            //}
        }

        if (!DialogueSystemManager.main.InteractionTarget) {
            return TutorialObjective.ApproachPerson;
        }

        return TutorialObjective.LookUpWord;
    }

    void SetObjective(TutorialObjective obj) {
        var inventory = TutorialCanvas.main.InventoryUI;
        inventory.gameObject.SetActive(true);
        TutorialCanvas.main.ClearAllIndicators();
        switch (obj) {
            case TutorialObjective.ApproachPerson:
                SetMessage("Approach the people.");
                break;

            case TutorialObjective.LookUpWord:
                SetMessage("Drag a word do the dictionary or right-click to continue.");
                TutorialCanvas.main.CreateUIDragBox(TutorialCanvas.main.GetRegisteredGameObject("QuickDictionary").GetComponent<RectTransform>(), "drag here to here");
                break;
        }
    }


}
