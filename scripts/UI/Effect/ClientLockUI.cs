using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crystallize;

public class ClientLockUI : MonoBehaviour {

	const float JumpTime = 0.8f;
	const float CompressTime = 0.2f;
	const float JumpHeight = 0.5f;
	const float CompressAmount = 0.25f;

	public Transform target;

	enum JumpPhase {
		Rising,
		Falling,
		Compressing,
	}

	public void Initialize(Transform target){
		this.target = target;
        if (target.GetComponent<InteractiveDialogActor>()) {
            GetComponentInChildren<Text>().text = target.GetComponent<InteractiveDialogActor>().minimumLevel.ToString();
        }
	}

	void Start(){
		if (target == null) {
			Destroy(gameObject);
		}

		transform.SetParent(WorldCanvas.main.transform);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.forward = -Camera.main.transform.forward;
		transform.LookAt (Camera.main.transform);
		transform.forward = -transform.forward;
		//transform.position = target.position + Vector3.up * (3f + offset);

		/*var f = Mathf.Repeat (Time.time, JumpTime + CompressTime);
		if (f < JumpTime) {
			transform.localScale = Vector3.one;
			offset = JumpHeight * curve.Evaluate (f / JumpTime);
		} else {
			var c = curve.Evaluate ((f - JumpTime)/ CompressTime);
			transform.localScale = new Vector3(1f + CompressAmount * c, 1f - CompressAmount * c, 1f);
			offset = 0;
		}*/
	}
}
