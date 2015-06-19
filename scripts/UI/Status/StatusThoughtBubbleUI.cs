using UnityEngine;
using System.Collections;

public class StatusThoughtBubbleUI : MonoBehaviour {

    public bool yourState = false;
    public bool partnerState = false;
    public ThoughtBubbleDialogueAvailableUI yourStateUI;
    public ThoughtBubbleDialogueAvailableUI partnerStateUI;

    public void Initialize(Transform target, bool yourState, bool partnerState) {
        transform.SetParent(FieldCanvas.main.transform);

        GetComponent<OverheadUI>().target = target;
        this.yourState = yourState;
        this.partnerState = partnerState;
        
        yourStateUI.SetState(yourState);
        partnerStateUI.SetState(partnerState);
    }

	// Use this for initialization
	void Start () {
        yourStateUI.SetState(yourState);
        partnerStateUI.SetState(partnerState);
	}
	
}
