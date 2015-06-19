using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class DialogueActorStateEffects : MonoBehaviour {

	InteractiveDialogActorEffectSet effectSet;
	
	GameObject lockEffect;
	GameObject questionEffect;
	GameObject exclaimationEffect;
	GameObject countEffect;
	GameObject checkEffect;

	GameObject[] Effects { 
		get {
			return new GameObject[]{lockEffect, exclaimationEffect, questionEffect, countEffect, checkEffect};
		}
	}

    int convID;
    ConversationClientState lastState = ConversationClientState.Hidden;

	// Use this for initialization
	void Start () {
        effectSet = EffectLibrary.Instance.actorEffects;


        var id = transform.GetWorldID();
        var c = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(id);
        convID = c.ID;
        var nws = new List<int>();
        foreach (var n in c.GetNeededWords()) {
            nws.Add(n.WordID);
        }

		lockEffect = Instantiate (EffectLibrary.Instance.uiClientLock) as GameObject;
		lockEffect.GetComponent<ClientLockUI> ().Initialize (transform);
        questionEffect = Instantiate(EffectLibrary.Instance.uiQuestionMark) as GameObject;
		questionEffect.GetComponent<QuestExclaimationPointUI> ().Initialize (transform);
        exclaimationEffect = Instantiate(EffectLibrary.Instance.uiExclaimationPoint) as GameObject;
		exclaimationEffect.GetComponent<QuestExclaimationPointUI> ().Initialize (transform);
        countEffect = Instantiate(EffectLibrary.Instance.uiWordCount) as GameObject;
		countEffect.GetComponent<QuestProgressCountUI> ().Initialize (transform, nws);
        checkEffect = Instantiate(EffectLibrary.Instance.uiCheckMark) as GameObject;
		checkEffect.GetComponent<QuestCheckUI> ().Initialize (transform);

        RefreshOverheadIndicator();
	}

    void Update() {
        var s = GetState();
        
        //Debug.Log(s + "; " + UISystem.main.Mode);
        if (s != lastState) {
            lastState = s;
            RefreshOverheadIndicator();
        }
    }

	void HandleOnPhraseSuccess (object sender, PhraseEventArgs e)
	{
		AudioManager.main.PlayPhraseSuccess ();
	}

	void HandleOnDialogueSuccess (object sender, PhraseEventArgs e)
	{
		EffectManager.main.EnqueueEffect (DialogSuccessEffect, 3f);
	}

	void DialogSuccessEffect(){
		AudioManager.main.PlayDialogueSuccess ();
        var effect = Instantiate(EffectLibrary.Instance.conversationCompleteEffect) as GameObject;
		effect.transform.SetParent (MainCanvas.main.transform);
		effect.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}

	void RefreshOverheadIndicator(){
		foreach (var eff in Effects) {
			eff.SetActive(false);
		}

		switch (lastState) {
		case ConversationClientState.Locked:
			lockEffect.SetActive (true);
			return;

		case ConversationClientState.SeekingClient:
			questionEffect.SetActive (true);
			return;

		case ConversationClientState.SeekingWords:
			countEffect.SetActive(true);
			return;

		case ConversationClientState.Available:
			exclaimationEffect.SetActive (true);
			return;

		case ConversationClientState.Completed:
			checkEffect.SetActive (true);
			return;
		}
	}

    ConversationClientState GetState() {
        var id = transform.GetWorldID();
        var c = GameData.Instance.DialogueData.GetLinearDialogueForWorldObject(id);
        if (c == null) {
            return ConversationClientState.Hidden;
        }

        if (UISystem.main.Mode == UIMode.FixedPhraseInput) {
            return ConversationClientState.Hidden;
        }

        if (!PlayerManager.main.playerData.Conversation.IsAvailable(convID)) {
            return ConversationClientState.Hidden;
        }

        var objs = c.GetNeededWords();
        var haveObjs = true;
        foreach(var o in objs){
            if(!PlayerManager.main.playerData.WordStorage.ObjectiveWords.Contains(o.WordID)
                && !PlayerManager.main.playerData.WordStorage.FoundWords.Contains(o.WordID)) {
                haveObjs = false;
                break;
            }
        }

        if(!haveObjs){
            return ConversationClientState.SeekingClient;
        } 

        var haveWords = true;
        foreach (var o in objs) {
            if (!PlayerManager.main.playerData.WordStorage.FoundWords.Contains(o.WordID)) {
                haveWords = false;
                break;
            }
        }

        if (!haveWords) {
            return ConversationClientState.SeekingWords;
        }

        var completed = PlayerManager.main.playerData.Conversation.GetConversationComplete(id);//c.ID);
        if (!completed) {
            return ConversationClientState.Available;
        }

        return ConversationClientState.Completed;
    }

	void HandleOnStateChanged (object sender, System.EventArgs e)
	{
		RefreshOverheadIndicator ();
	}

	void HandleOnReject (object sender, PhraseEventArgs e)
	{
		AudioManager.main.PlayDialogueFailure ();
		if (effectSet.rejectEffect) {
			var go = Instantiate(effectSet.rejectEffect, transform.position, Quaternion.identity) as GameObject;
			go.transform.parent = transform;
		}
	}

	void HandleOnReact (object sender, PhraseEventArgs e)
	{
		if (effectSet.reactEffect) {
			var go = Instantiate(effectSet.reactEffect, transform.position, Quaternion.identity) as GameObject;
			go.transform.parent = transform;
		}
	}

}
