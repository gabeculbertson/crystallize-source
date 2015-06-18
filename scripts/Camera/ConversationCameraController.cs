using UnityEngine;
using System.Collections;

public class ConversationCameraController : MonoBehaviour {

    public Transform cameraTransform;
    public Transform conversationTarget;

	// Use this for initialization
	void Start () {
        transform.SetParent(PlayerManager.main.PlayerGameObject.transform);
        transform.localPosition = Vector3.zero;
        OmniscientCamera.main.Suspend();
	}

    void OnDestroy()
    {
        if (Camera.main) {
            OmniscientCamera.main.Resume();
        }
    }
	
	// Update is called once per frame
	void Update () {
        var forward = conversationTarget.transform.position - transform.position;
        forward.y = 0;
        transform.forward = forward;
        OmniscientCamera.main.SetPosition( cameraTransform.position);
        OmniscientCamera.main.SetRotation( cameraTransform.rotation);
	}
}
