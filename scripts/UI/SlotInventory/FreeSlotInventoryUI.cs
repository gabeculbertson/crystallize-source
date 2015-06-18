using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreeSlotInventoryUI : MonoBehaviour {

	public GameObject slotPrefab;

	List<RectTransform> slotOrder = new List<RectTransform>();

	// Use this for initialization
	void Start () {
		RefreshPanelState ();
	}

	void RefreshPanelState(){
		slotOrder.Clear ();
		
		for (int i = 0; i < 5; i++) {
			AddSlot(null);
		}
	}

	RectTransform AddSlot(PhraseSegmentData phraseData){
		var go = Instantiate (slotPrefab) as GameObject;
		go.GetComponent<ExplicitInventorySlotUI> ().SetWord (null);
		go.GetComponent<ExplicitInventorySlotUI> ().EnableRankUp = false;
		go.GetComponent<ExplicitInventorySlotUI> ().AllowOverride = true;
		go.transform.SetParent (transform);	
		return go.GetComponent<RectTransform> ();
	}

}
