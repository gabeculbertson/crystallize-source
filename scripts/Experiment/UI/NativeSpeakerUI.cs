using UnityEngine;
using System.Collections;
using Crystallize;

public class NativeSpeakerUI : MonoBehaviour {

    DialogueActor targetActor;

	// Use this for initialization
	void Start () {
        transform.SetParent(MainCanvas.main.transform);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            if (!targetActor) {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit)) {
                    var da = hit.transform.parent.GetComponent<DialogueActor>();
                    if (da) {
                        targetActor = da;
                        var ps = new PhraseSequence();
                        ps.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, ":)"));
                        targetActor.SetPhrase(ps);
                        CrystallizeEventManager.Network.RaiseNetworkSpeechBubbleRequested(this, 
                            new NetworkSpeechBubbleRequestedEventArgs(targetActor.transform, ps, false, false, false));
                    }
                }
            } else {
                targetActor.SetPhrase(null);
                targetActor = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            GivePlayerPositiveFeedback();
        }
	}

    public void GivePlayerPositiveFeedback() {
        EffectManager.main.PlayPositiveFeedback();
        CrystallizeEventManager.Network.RaiseNetworkPlayerFeedbackRequested(this, System.EventArgs.Empty);
    }

}
