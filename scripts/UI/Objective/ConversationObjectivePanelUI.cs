using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationObjectivePanelUI : MonoBehaviour {

	const float Padding = 10f;

	public GameObject objectivePrefab;

	List<ConversationWordBoxUI> objectiveInstances = new List<ConversationWordBoxUI>();
	Dictionary<ConversationWordBoxUI, bool> completedObjectives = new Dictionary<ConversationWordBoxUI, bool>();

	public RectTransform rectTransform { get; private set; }

	public event EventHandler<PhraseEventArgs> OnObjectiveCompleted;
	public event EventHandler<PhraseEventArgs> OnWordUnlocked;

	public bool Unlocked { get; set; }

	public bool IsComplete { 
		get {
			if(completedObjectives.Count == 0){
				return false;
			}

			foreach (var v in completedObjectives.Values) {
				if (!v) {
					return false;
				}
			}
			return true;
		}
	}

	// Use this for initialization
	void Awake () {
		Unlocked = false;
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		var pos = new Vector2 (Padding, Padding) + (Vector2)rectTransform.position;
		foreach (var objInstance in objectiveInstances) {
			objInstance.Anchor = pos;

			pos.x += objInstance.rectTransform.rect.width + Padding;

			CheckObjectiveComplete(objInstance);
		}
	}

	public void Set(List<PhraseSegmentData> phrases, bool unlocked){
		foreach (var obj in objectiveInstances) {
			Destroy(obj.gameObject);
		}

		objectiveInstances.Clear ();

		int index = 0;
		//Debug.Log ("Setting phrases: " + phrases);
		foreach (var p in phrases) {
			var go = Instantiate(objectivePrefab) as GameObject;
			go.transform.SetParent(transform);
			go.GetComponent<RectTransform>().position = new Vector2 (Padding - 300f, Padding);

			var obj = go.GetComponent<ConversationWordBoxUI>();
			obj.Initialize(p);
			//obj.phrase = p;
			obj.MoveSpeed = 1000f + (100f * index);
			objectiveInstances.Add(obj);
			obj.unlocked = unlocked;

			index++;
		}
	}

	public void UnlockWords(){
		Unlocked = true;
		//Debug.Log ("Unlocking words.");

		StartCoroutine (UnlockWordsSequence ());
	}

	IEnumerator UnlockWordsSequence(){
		foreach (var obj in objectiveInstances) {
			obj.Unlock();

			yield return new WaitForSeconds(0.25f);
		}

		if (OnWordUnlocked != null) {
			OnWordUnlocked(this, PhraseEventArgs.Empty);
		}
	}

	void CheckObjectiveComplete(ConversationWordBoxUI wordBox){
		if(!completedObjectives.ContainsKey(wordBox)){
			completedObjectives[wordBox] = false;
		}

		if(!completedObjectives[wordBox]){
			if(wordBox.ObjectiveComplete){
				if(OnObjectiveCompleted != null){
					OnObjectiveCompleted(this, new PhraseEventArgs(wordBox.phrase));
				}

				Debug.Log(wordBox.phrase + " objective complete.");
				completedObjectives[wordBox] = true;
			}
		}
	}

}
