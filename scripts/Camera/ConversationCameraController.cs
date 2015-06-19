using UnityEngine;
using System.Collections;

public class ConversationCameraController : MonoBehaviour, IProcess<GameObject, object> {

    const string ResourcePath = "Camera/ConversationCameraController";

    public static ConversationCameraController GetInstance(GameObject target) {
        var i = GameObjectUtil.GetResourceInstance<ConversationCameraController>(ResourcePath);
        i.Initialize(target);
        return i;
    }

    public Transform cameraTransform;
    public Transform conversationTarget;

    public event ProcessExitCallback<object> OnExit;

    public void Initialize(ProcessRequestEventArgs<GameObject, object> args) {
        Initialize(args.Data);
    }

    public void Initialize(GameObject target) {
        conversationTarget = target.transform;
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        if (gameObject) {
            Destroy(gameObject);
        }
        OnExit.Raise(this, null);
    }

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
