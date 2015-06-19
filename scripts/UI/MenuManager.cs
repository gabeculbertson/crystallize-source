using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public GameObject branchDialogueUIPrefab;
	public GameObject phraseEntryUIPrefab;
	public GameObject questConfirmationPrefab;
    public GameObject areaConfirmationPrefab;
    public GameObject areaTravelConfirmationPrefab;
    public GameObject wordSelectionPanelPrefab;
    public GameObject fullInventoryPrefab;
    public GameObject wordTranslationPrefab;
    public GameObject areaMenuPrefab;

	// Use this for initialization
	void Start () {
		CrystallizeEventManager.UI.OnUIRequested += HandleOnUIRequested;

        CrystallizeEventManager.UI.OnReplaceWordPhraseEditorRequested += HandleReplaceWordPhraseEditorRequested;
        CrystallizeEventManager.UI.OnWordSelectionRequested += HandleWordSelectionRequested;
	}

    void HandleReplaceWordPhraseEditorRequested(object sender, SequenceRequestEventArgs<PhraseSequence, PhraseSequence> e) {
        var instance = ReplaceWordPhraseEditorUI.GetInstance();
        instance.Initialize(e.Data);
        instance.transform.position = new Vector2(Screen.width * 0.5f, 300f);
        e.SequenceRequest.RaiseCallback(instance);
    }

    void HandleWordSelectionRequested(object sender, SequenceRequestEventArgs<int, PhraseSequenceElement> e) {
        var instance = WordSelectionPanelUI.GetInstance();
        e.SequenceRequest.RaiseCallback(instance);
    }

	void HandleOnUIRequested (object sender, UIRequestEventArgs e)
	{
		if (e is BranchUIRequestEventArgs) {
			var be = (BranchUIRequestEventArgs)e;
			var instance = GetMenuInstance(branchDialogueUIPrefab, e);
			instance.GetComponent<BranchUI>().Initialize(be.Branches);
		} else if (e is PhraseInputUIRequestEventArgs) {
			var pie = (PhraseInputUIRequestEventArgs)e;
			var instance = GetMenuInstance(phraseEntryUIPrefab, e);
            var missing = new List<int>();
            var useMessage = true;
            if (pie.PlayerLine != null) {
                //if (pie.PlayerLine.GiveObjectives) { 
                missing = pie.PlayerLine.GetMissingWords();
                useMessage = pie.PlayerLine.ProvideMissingWordsMessage;
            } else {
                //TODO: move this
                var mws = new List<int>();

                for (int i = 0; i < pie.PhraseSequence.PhraseElements.Count; i++) {
                    var ele = pie.PhraseSequence.PhraseElements[i];

                    if (ele.GetPhraseCategory() == PhraseCategory.Unknown) {
                        continue;
                    }

                    if (ele.GetPhraseCategory() == PhraseCategory.Particle) {
                        continue;
                    }

                    if (ele.GetPhraseCategory() == PhraseCategory.Punctuation) {
                        continue;
                    }

                    mws.Add(ele.WordID);
                }

                missing = mws;
            }
			instance.GetComponent<PhraseEntryPanelUI>().Initialize(pie.PhraseSequence, missing, useMessage);
		} else if (e is QuestConfirmationUIRequestEventArgs) {
			var qe = (QuestConfirmationUIRequestEventArgs)e;
			var instance = GetMenuInstance(questConfirmationPrefab, e);
			instance.GetComponent<QuestConfirmationUI>().Initialize(qe.Quest);
		} else if (e is AreaUnlockConfirmationUIRequestEventArgs) {
            var ae = (AreaUnlockConfirmationUIRequestEventArgs)e;
            var instance = GetMenuInstance(areaConfirmationPrefab, e);
            instance.GetComponentInChildren<UnlockConfirmationUI>().Initialize(ae.Area);
        } else if (e is AreaTravelConfirmationUIRequestEventArgs) {
            var ae = (AreaTravelConfirmationUIRequestEventArgs)e;
            var instance = GetMenuInstance(areaTravelConfirmationPrefab, e);
            instance.GetComponentInChildren<TravelConfirmationUI>().Initialize(ae.Area);
        }else if (e is WordSelectionUIRequestEventArgs) {
            var ws = (WordSelectionUIRequestEventArgs)e;
            var instance = GetMenuInstance(wordSelectionPanelPrefab, e);
            instance.GetComponent<WordSelectionPanelUI>().Initialize(ws.Container);
        } else if (e is FullInventoryUIRequestEventArgs) {
            GetMenuInstance(fullInventoryPrefab, e);
        } else if (e is WordTranslationUIRequestEventArgs) {
            var i = GetMenuInstance(wordTranslationPrefab, e);
            var wte = (WordTranslationUIRequestEventArgs)e;
            i.GetComponent<WordTranslationUI>().Initialize(wte.Word, wte.Target, wte.SuccessCallback, wte.FailureCallback);
        } else if (e is AreaMenuUIRequestEventArgs) {
            GetMenuInstance(areaMenuPrefab, e);
        } 
	}

	GameObject GetMenuInstance(GameObject menuPrefab, UIRequestEventArgs args){
		if (!menuPrefab) {
			Debug.LogError("Menu prefab cannot be null!");
			return null;
		}

		var i = Instantiate(menuPrefab) as GameObject;
		i.transform.SetParent (args.MenuParent.transform);
		return i;
	}

}
