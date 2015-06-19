using UnityEngine;
using System.Collections;

public class UIEffectProducer : MonoBehaviour {

	public static UIEffectProducer main { get; set; }

	public GameObject notEnoughMoneyEffect;
	public GameObject expandedEffect;

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void CreateEffect(GameObject prefab, Vector2 position){
		var go = (GameObject)Instantiate (prefab);
		go.transform.SetParent(transform.parent);
		go.GetComponent<RectTransform> ().position = position;
	}

}
