using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crystallize;

[RequireComponent(typeof(InteractiveDialogActor))]
public class InteractiveDialogActorEffects : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
        effectSet = EffectLibrary.Instance.actorEffects;
            //EffectManager.main.actorEffects;
		var a = GetComponent<InteractiveDialogActor> ();
		a.OnDialogueSuccess += HandleOnDialogueSuccess;
		a.OnPhraseSuccess += HandleOnPhraseSuccess;
		a.OnReact += HandleOnReact;
		a.OnReject += HandleOnReject;

        lockEffect = Instantiate(EffectLibrary.Instance.uiClientLock) as GameObject;
		lockEffect.GetComponent<ClientLockUI> ().Initialize (transform);
        questionEffect = Instantiate(EffectLibrary.Instance.uiQuestionMark) as GameObject;
		questionEffect.GetComponent<QuestExclaimationPointUI> ().Initialize (transform);
        exclaimationEffect = Instantiate(EffectLibrary.Instance.uiExclaimationPoint) as GameObject;
		exclaimationEffect.GetComponent<QuestExclaimationPointUI> ().Initialize (transform);
        countEffect = Instantiate(EffectLibrary.Instance.uiWordCount) as GameObject;
		countEffect.GetComponent<QuestProgressCountUI> ().Initialize (transform);
        checkEffect = Instantiate(EffectLibrary.Instance.uiCheckMark) as GameObject;
		checkEffect.GetComponent<QuestCheckUI> ().Initialize (transform);

		RefreshOverheadIndicator ();

		GetComponent<ConversationClient> ().OnStateChanged += HandleOnStateChanged;
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

		var a = GetComponent<InteractiveDialogActor> ();
		var client = GetComponent<ConversationClient> ();
		//Debug.Log ("Refreshing." + a.IsOpen + "; " + exclaimationEffect);
		if (a.IsOpen) {
			return;
		}

		switch (client.State) {
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
