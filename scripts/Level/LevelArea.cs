using UnityEngine;
using System.Collections;

public class LevelArea : MonoBehaviour {

	public Transform[] areaObjects;
	public Transform cameraController;

	ICameraController cameraControllerInterface;

	public virtual void Start(){
		if (cameraController) {
			cameraControllerInterface = cameraController.gameObject.GetInterface<ICameraController>();
		}
	}

	public bool ContainsPlayer(Vector3 position){
		return GetComponent<BoxCollider>().bounds.Contains(position);
	}
	
	public void SetObjectsActive(bool isActive){
		//Debug.Log ("SetObjectsActive (" + isActive + ") " + transform);
		foreach (var obj in areaObjects) {
			obj.gameObject.SetActive(isActive);
		}

		if (cameraControllerInterface != null) {
			//Debug.Log("CameraActive " + transform + " " + isActive);
			cameraControllerInterface.SetAsController(isActive);
		}
	}

}
