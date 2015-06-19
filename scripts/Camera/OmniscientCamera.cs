using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Sequence;

public class OmniscientCamera : MonoBehaviour {

	public static OmniscientCamera main { get; set; }

    //List<GameObject> motionFilterObjects = new List<GameObject>();
    List<ICameraMotionFilter> motionFilters = new List<ICameraMotionFilter>();

	private Vector3 anchorMouse;
	private Vector3 anchorCamera;
	
	public Vector3 target;
	public Transform player;
	//private Transform player = null;
	public float distance = 10.0f;
	
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float keyMoveSpeed = 5f;
	public float keyZoomSpeed = 5f;
	public float keyRotateSpeed = 50f;
	
	float x = 0.0f;
	float y = 0.0f;
	
	public bool lockedOn = true;
	public bool enableKeyMovement = true;
	public Vector3 offset = new Vector3(0, 1.5f, 0);

	bool copyTransform = true;
	float minSpeed = 7f;
	Transform finalTransform;
	Sequence zoomSequence;

	bool suspended = false;
	Vector3 nextPosition;
	Quaternion nextRotation;

	HashSet<object> controlLocks = new HashSet<object>();

	public bool ControlLocked {
		get {
			return controlLocks.Count > 0;
		}
	}

	void Awake(){
		main = this;
	}

	void OnDestroy(){
		main = null;
	}

	// Use this for initialization
	void Start () {
        if (!player) {
            player = PlayerManager.main.PlayerGameObject.transform;
        }

		finalTransform = new GameObject ("OmniscientCameraTarget").transform;

		var angles = transform.eulerAngles;
	    x = angles.y;
	    y = angles.x;
		
		if (!player)
			Unlock ();
	}

    public void AddMotionFilter(ICameraMotionFilter filter) {
        if (!motionFilters.Contains(filter)) {
            motionFilters.Add(filter);
        }
    }

    public void RemoveMotionFilter(ICameraMotionFilter filter) {
        if (motionFilters.Contains(filter)) {
            motionFilters.Remove(filter);
        }
    }
	
	void Update () {	
		if (suspended) {
			return;
		}

		if(lockedOn){ target = player.position + offset; }

		if (ControlLocked) {
			return;
		}

		if (!Application.isEditor) {
			return;
			//Debug.Log("is editor");
		}

		if (!Input.GetKey (KeyCode.LeftControl)) {
			finalTransform.position += finalTransform.forward * Time.deltaTime * Input.GetAxis ("Mouse ScrollWheel") * 500;
			distance -= Time.deltaTime*Input.GetAxis("Mouse ScrollWheel")*500;
		}

		if (!player) {
			Debug.Log("No player!");
			Unlock();
		}

		
		if(Input.GetKeyDown(KeyCode.Escape) && player){
			lockedOn = true;
		}

		
		if(Input.GetKey(KeyCode.LeftShift)){
			if(lockedOn) {
				Unlock();
			}
			if(Input.GetMouseButtonDown(2)){
				anchorMouse = Input.mousePosition;
				anchorCamera = finalTransform.position;
			}
			if(Input.GetMouseButton(2)){
				finalTransform.position = anchorCamera;
				finalTransform.Translate(-1 * (Input.mousePosition - anchorMouse) * 0.05f);
			}
		}
		else{
			if(Input.GetMouseButtonDown(2)){
				target = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
			}
			if(Input.GetMouseButtonUp(2)){
				target = Vector3.zero;
			}
		}
	}
	
	void LateUpdate () {
		CrystallizeEventManager.Environment.RaiseBeforeCameraMove (this, System.EventArgs.Empty);

		if (suspended) {
			transform.position = nextPosition;
			transform.rotation = nextRotation;
		} else {
			UpdateMouseOrbit();
		} 
		nextPosition = transform.position;
		nextRotation = transform.rotation;

		CrystallizeEventManager.Environment.RaiseAfterCameraMove (this, System.EventArgs.Empty);
	}

	void UpdateMouseOrbit(){
		if (target != Vector3.zero) {
			if(Input.GetMouseButton(2)){
				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				
				y = ClampAngle(y, yMinLimit, yMaxLimit);
			}
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0, 0, -distance) + target;
			finalTransform.position = position;
			finalTransform.rotation = rotation;
		}
		
		if (copyTransform) {
			transform.position = finalTransform.position;
			transform.rotation = finalTransform.rotation;
			copyTransform = false;
		} else {
			var dist = Vector3.Distance (transform.position, finalTransform.position);
			var moveDist = (minSpeed + dist) * Time.deltaTime;

            var oldPos = transform.position;
            var newPos = Vector3.MoveTowards(transform.position, finalTransform.position, moveDist);
            foreach (var f in motionFilters) {
                newPos = f.UpdatePosition(transform, oldPos, newPos);
            }
            transform.position = newPos;

			transform.rotation = Quaternion.Lerp (transform.rotation, finalTransform.rotation, moveDist / dist);
		}
	}

	public void Unlock(){
		lockedOn = false;
		target = Vector3.zero;
	}
	
	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360f)
			angle += 360f;
		if (angle > 360f)
			angle -= 360f;
		return Mathf.Clamp (angle, min, max);
	}

	public void LockControl(object obj){
		if (!controlLocks.Contains (obj))
			controlLocks.Add (obj);
	}

	public void UnlockControl(object obj){
		if (controlLocks.Contains (obj)) {
			controlLocks.Remove(obj);
		}
	}
	
//	bool enabledOnSuspend = false;
	public void Suspend(){
		//enabledOnSuspend = enabled;
		//enabled = false;
		suspended = true;
	}
	
	public void Resume(){
		//enabled = true;//enabledOnSuspend;
		suspended = false;
	}

	Vector3 CalculatePoint(Transform t){
		var pos = t.position + offset;
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		return rotation * new Vector3(0, 0, -distance) + pos;
	}

	public void SetPosition(Vector3 position){
		nextPosition = position;
	}

	public void SetRotation(Quaternion rotation){
		nextRotation = rotation;
	}

    public void SetTargetAngles(Vector3 angles) {
        x = angles.y;
        y = angles.x;
    }

	/*public void ZoomTo(Transform newTarget, float duration = 1f){
		if (zoomSequence != null) {
			zoomSequence.Cancel();
		}
		zoomSequence = new Sequence (ZoomToSequence (newTarget, duration));
	}

	IEnumerator ZoomToSequence(Transform newTarget, float duration){
		Unlock ();
		if (duration > 0) {
			for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
				var pos = CalculatePoint (newTarget);
				transform.position = Vector3.Lerp (transform.position, pos, t * t);

				yield return null;
			}
		}
		transform.position = CalculatePoint (newTarget);
		player = newTarget;
		lockedOn = true;
	}

	public Sequence ZoomAndRotate(Transform cameraTarget, float duration = 1f){
		if (!cameraTarget) {
			throw new UnityException("Camera target cannot be null!");
		}

		if (zoomSequence != null) {
			zoomSequence.Cancel();
		}
		zoomSequence = new Sequence (ZoomAndRotateSequence (cameraTarget, duration));
		return zoomSequence;
	}

	IEnumerator ZoomAndRotateSequence(Transform newTarget, float duration){
		Debug.Log ("zooming to " + newTarget);
		Unlock ();
		if (duration > 0) {
			var originalRot = transform.rotation;
			var originalPos = transform.position;
			for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
				transform.position = Vector3.Lerp(originalPos, newTarget.position, t);
				transform.rotation = Quaternion.Lerp(originalRot, newTarget.rotation, t);
				
				yield return null;
			}
		}
		transform.position = newTarget.position;
		transform.rotation = newTarget.rotation;
	}

	/// <summary>
	/// Tracks the target.
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="rate">Rotation rate in degrees per second.</param>
	public void TrackTarget(Transform target, float rate = 30f){
		if (zoomSequence != null) {
			zoomSequence.Cancel();
		}

		zoomSequence = new Sequence (TrackTargetSequence(target, rate));
		zoomSequence.OnDestroy += (object sender, System.EventArgs e) => UnlockControl(this);
	}

	IEnumerator TrackTargetSequence(Transform target, float rate){
		Unlock ();
		LockControl (this);

		while (true) {
			var targetForward = target.position - transform.position;
			var angle = Vector3.Angle(targetForward, transform.forward);
			var targetRotation = Quaternion.LookRotation(targetForward);
			var thisRate = Mathf.Clamp(angle / rate, 0.5f, rate);
			thisRate += 0.1f;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, thisRate * Time.deltaTime);

			yield return null;
		}
	}*/
	
}
