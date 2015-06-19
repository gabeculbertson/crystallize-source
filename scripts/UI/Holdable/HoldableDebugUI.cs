using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoldableDebugUI : MonoBehaviour {
	
	public GameObject holdableItemButtonPrefab;

	// Use this for initialization
	void Start () {
		transform.SetParent (MainCanvas.main.transform);
		transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * .5f);

		foreach (var h in ScriptableObjectDictionaries.main.holdableDictionary.GetAllHoldables()) {
			var instance = Instantiate(holdableItemButtonPrefab) as GameObject;
			instance.transform.SetParent(transform);
			instance.GetComponent<HoldableItemButton>().Initiallize(gameObject, h.id);
		}
	}

	public void Close(){
		Destroy (gameObject);
	} 

}
