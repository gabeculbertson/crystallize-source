using UnityEngine;
using System.Collections;

public class PersonColliderManager : MonoBehaviour {

	public GameObject colliderPrefab;

	// Use this for initialization
	void Start () {
		foreach (var t in GetComponentsInChildren<Transform>()) {
			var teh = t.gameObject.GetInterface<IPerson>();
			if(teh != null){
				var colliderInstance = Instantiate(colliderPrefab) as GameObject;
				colliderInstance.transform.SetParent(t);
				colliderInstance.transform.localPosition = Vector3.zero;
			}
		}
	}

}
