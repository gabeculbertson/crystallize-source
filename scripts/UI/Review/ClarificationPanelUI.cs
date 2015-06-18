using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClarificationPanelUI : MonoBehaviour {

	public static ClarificationPanelUI main { get; set; }

	public GameObject clarificationButtonPrefab;

	List<GameObject> buttons = new List<GameObject>();

	public event EventHandler<PhraseEventArgs> OnWordSelected;

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}

	void Update(){
		
	}

	public void Open(){
		gameObject.SetActive (true);
	}

	public void Close(){
		if (OnWordSelected != null) {
			foreach (var d in OnWordSelected.GetInvocationList()) {
				OnWordSelected -= (EventHandler<PhraseEventArgs>)d;
			}
		}
		gameObject.SetActive (false);
	}
	
	public void Initialize(List<PhraseSegmentData> words){
		foreach (var b in buttons) {
			Destroy(b);
		}

		buttons.Clear ();

		foreach (var w in words) {
			var go = Instantiate(clarificationButtonPrefab) as GameObject;
			go.GetComponent<ClarificationButtonUI>().Initialize(this, w);
			buttons.Add(go);
		}

		transform.SetAsLastSibling ();
	}

	public void WordClicked(PhraseSegmentData word){
		Debug.Log ("Word clicked " + word.Text);
		if (OnWordSelected != null) {
			OnWordSelected(this, new PhraseEventArgs(word));
		}
		Close ();
	}

}
