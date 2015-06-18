using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FloatingNameUI : MonoBehaviour {

	public static FloatingNameUI main { get; set; }

	public GameObject namePrefab;

	Dictionary<FloatingNameHolder, RectTransform> holderNames = new Dictionary<FloatingNameHolder, RectTransform>();

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!PlayerManager.main.PlayerGameObject) {
			return;
		}

        var player = PlayerManager.main.PlayerGameObject.transform;
		foreach(var h in holderNames.Keys){
			var d = Vector3.Distance(player.position, h.transform.position);
			if(d < 20f){
				holderNames[h].GetComponent<CanvasGroup>().alpha = 1f - ((d - 5f) / 15f);
				holderNames[h].position = Camera.main.WorldToScreenPoint(h.transform.position + Vector3.up * 2.5f);
			} else {
				holderNames[h].GetComponent<CanvasGroup>().alpha = 0;
			}
		}
	}

	public void SetName(FloatingNameHolder holder, string name){
		if (!holderNames.ContainsKey (holder)) {
			var instance = Instantiate (namePrefab) as GameObject;
			instance.transform.SetParent(transform);
			holderNames [holder] = instance.GetComponent<RectTransform>();
		} 
		holderNames[holder].GetComponentInChildren<Text> ().text = name;
	}

}
