using UnityEngine;
using System.Collections;

public class Cursor3DBehavior : MonoBehaviour {

    int playerID = -1;

    public void Initialize(int playerID) {
        this.playerID = playerID;
    }

	// Use this for initialization
	void Start () {
		GetComponentInChildren<TriggerEventObject>().OnTriggerEnterEvent += HandleOnTriggerEnterEvent;
	}

	void HandleOnTriggerEnterEvent (object sender, TriggerEventArgs e)
	{
		if (e.Collider.IsHumanControlled ()) {
			var args = new CursorApproachedEventArgs(PlayerManager.main.GetPlayerID(e.Collider.attachedRigidbody.gameObject), playerID);
			//Debug.Log("Approached: " + args.ActorPlayerID + "; " + args.CursorPlayerID);
            CrystallizeEventManager.PlayerState.RaiseGameEvent(this, args);
		}
	}

}
