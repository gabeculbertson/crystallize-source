using UnityEngine;
using System.Collections;

public class ConversationWordBoxUI : MonoBehaviour {
	
	public GameObject objective;
	public GameObject reward;

	public RectTransform rectTransform { get; set; }

	public Vector2 Anchor { get; set; }
	public float MoveSpeed { get; set; }

	public bool ObjectiveComplete { get; set; }

	public PhraseSegmentData phrase;
	public bool unlocked = false;

	bool initialized = false;

	void Awake(){
		MoveSpeed = 1000f;
		rectTransform = GetComponent<RectTransform> ();
	}

	public void Initialize(PhraseSegmentData phrase){
		this.phrase = phrase;
		GetComponentInChildren<ConversationObjectiveUI> ().phrase = phrase;
		GetComponentInChildren<ConversationRewardUI> ().phrase = phrase;
		objective.SetActive(false);
		reward.SetActive(false);

		initialized = true;
	}

	// Use this for initialization
	void Start () {
		//unlocked = PlayerManager.main.playerData.WordStorage.ContainsWord (phrase);
		if (!initialized) {
			Initialize(phrase);
		}

		if (!unlocked) {
			objective.SetActive(true);
			reward.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.position = Vector2.MoveTowards (rectTransform.position, Anchor, MoveSpeed * Time.deltaTime);
		ObjectiveComplete = objective.GetComponent<ConversationObjectiveUI> ().IsComplete;
	}

	public void Unlock(){
		StartCoroutine (UnlockSequence ());
	}
	
	IEnumerator UnlockSequence(){
		var go = Instantiate<GameObject> (EffectLibrary.Instance.uiEnergizeEffect);
		go.transform.SetParent (objective.transform);
		
		yield return new WaitForSeconds(0.25f);
		
		objective.SetActive (false);
		reward.SetActive (true);

        var set = Instantiate<GameObject>(EffectLibrary.Instance.uiSetEffect);
		set.transform.SetParent (reward.transform);

		unlocked = true;
	}

}
