using UnityEngine;
using System.Collections;

public class GardenObjectProducer : MonoBehaviour {

	public GameObject levelSourceObject;
	public GameObject[] levelObjects;
	public GameObject smokeEffect;

	ILevel levelSource;
	GameObject instance;

	int level = -1;

	// Use this for initialization
	void Start () {
		levelSource = levelSourceObject.GetInterface<ILevel> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (level != levelSource.Level) {
			SetInstance(levelSource.Level);
		}
	}

	void SetInstance(int level){
		if (instance) {
			Destroy(instance);
		}
		int target = Mathf.Clamp(level, 0, levelObjects.Length - 1);
		var rot = Quaternion.AngleAxis (Random.Range(0, 360f), Vector3.up);
		instance = (GameObject)Instantiate (levelObjects [target], transform.position, rot);
		instance.transform.parent = transform;
		if (instance.GetComponent<Renderer>()) {
			instance.GetComponent<Renderer>().material.color = GUIPallet.Instance.levelColors[level];
		}
		Instantiate(smokeEffect, transform.position + Vector3.up, Quaternion.identity);
		this.level = level;
	}

}
