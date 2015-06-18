using UnityEngine;
using System.Collections;

public class SpeechStatusEffect : MonoBehaviour {

    public bool yourState = false;
    public bool partnerState = false;

    GameObject instance;
    bool initialized = false;

    void Initialize() {
        var pdGID = GetComponent<PlayerDependentWorldObjectComponent>();
        bool interactsWithSelf = true;
        if (pdGID) {
            var p = GameData.Instance.DialogueData.PersonPhrases.GetItem(pdGID.GlobalID);
            //Debug.Log(p + ";" + pdGID.GlobalID + "; ");
            if (p != null) {
                interactsWithSelf = false;
            }
        }
        if (interactsWithSelf) {
            yourState = true;
            partnerState = false;
        } else {
            yourState = false;
            partnerState = true;
        }

        if (!instance) {
            instance = Instantiate<GameObject>(EffectLibrary.Instance.uiStatusThoughBubble);
        }
        instance.GetComponent<StatusThoughtBubbleUI>().Initialize(transform, yourState, partnerState);

        initialized = true;
    }

	// Use this for initialization
	IEnumerator Start () {
        yield return null;

        if (!initialized) {
            Initialize();
        }

        CrystallizeEventManager.Network.OnConnectedToNetwork += HandleConnectedToNetwork;
        CrystallizeEventManager.Environment.OnActorApproached += HandleActorApproached;
        CrystallizeEventManager.Environment.OnActorDeparted += HandleActorDeparted;
	}

    void HandleConnectedToNetwork(object sender, System.EventArgs e) {
        Initialize();
    }

    void HandleActorApproached(object sender, System.EventArgs e) {
        // TODO: need to stop this...a  
        //if (DialogueSystemManager.main.InteractionTarget) {
            instance.SetActive(false);
        //}
    }

    void HandleActorDeparted(object sender, System.EventArgs e) {
        //if (!DialogueSystemManager.main.InteractionTarget && enabled) {
        if (enabled) {
            instance.SetActive(true);
        }
        //}
    }

    /*void OnEnable() {
        if (!initialized) {
            Initialize();
        }
        instance.SetActive(true);
    }

    void OnDisable() {
        if (instance) {
            instance.SetActive(false);
        }
    }*/
	
}
