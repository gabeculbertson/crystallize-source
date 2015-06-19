using UnityEngine;
using System.Collections;

public class PersonTriggerManager : MonoBehaviour {

	public GameObject triggerPrefab;

	// Use this for initialization
	void Start () {
		foreach (Transform t in transform) {
			var teh = t.gameObject.GetInterface<ITriggerEventHandler>();
			if(teh != null && t.HasWorldID()){
				var triggerInstance = Instantiate(triggerPrefab) as GameObject;
				triggerInstance.transform.SetParent(t);
				triggerInstance.transform.localPosition = Vector3.zero;
                //Debug.Log(triggerInstance.GetComponent<TriggerEventObject>());
                //Debug.Log(teh);
                //Debug.Log(triggerInstance.GetComponent<TriggerEventObject>());
				triggerInstance.GetComponent<TriggerEventObject>().OnTriggerEnterEvent += teh.HandleTriggerEntered;
				triggerInstance.GetComponent<TriggerEventObject>().OnTriggerExitEvent += teh.HandleTriggerExited;
			}
		}
	}

    void OnDrawGizmos() {
        var go = GameObject.FindGameObjectWithTag("PlayerOrigin");
        if (go) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(go.transform.position, 0.25f);
        }
    }

}
