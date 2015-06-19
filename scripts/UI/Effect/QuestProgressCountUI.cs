using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestProgressCountUI : MonoBehaviour {

    public Transform _target;
	public ConversationClient target;
	public Text countText;

    List<int> neededWords;// = new List<int>();
	List<PhraseSegmentData> phraseData = new List<PhraseSegmentData>();

	enum JumpPhase {
		Rising,
		Falling,
		Compressing,
	}

	public void Initialize(Transform target){
		this.target = target.GetComponent<ConversationClient>();

		phraseData = this.target.GetObjectiveWords ();
	}

    public void Initialize(Transform target, List<int> neededWords) {
        this._target = target;
        this.neededWords = new List<int>(neededWords);
    }

	void Start(){
		transform.SetParent(WorldCanvas.main.transform);
	}
	
	// Update is called once per frame
	void Update () {
        if (neededWords == null) {
            transform.LookAt(Camera.main.transform);
            transform.forward = -transform.forward;
            transform.position = target.transform.position + Vector3.up * 2f;

            int completed = 0;
            foreach (var word in phraseData) {
                if (PlayerManager.main.playerData.WordStorage.ContainsFoundWord(word)) {
                    completed++;
                }
            }
            countText.text = string.Format("{0}/{1} words", completed, phraseData.Count);
        } else {
            transform.LookAt(Camera.main.transform);
            transform.forward = -transform.forward;
            transform.position = _target.position + Vector3.up * 2f;

            var count = 0;
            foreach (var n in neededWords) {
                if (PlayerManager.main.playerData.WordStorage.ContainsFoundWord(n)) {
                    count++;
                }
            }
            countText.text = string.Format("{0}/{1} words", count, neededWords.Count);
        }
	}
}
