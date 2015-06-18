using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WASDPanelUI : MonoBehaviour {

	public List<WASDButtonUI> buttons = new List<WASDButtonUI>();

	// Use this for initialization
	void Start () {
        transform.SetParent(MainCanvas.main.transform);
        transform.position = new Vector2(Screen.width * 0.5f, 300f);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var button in buttons) {
			button.SetState(false);
		}

		int activeButton = ((int)Time.time) % 4;
		buttons [activeButton].SetState (true);
	}
}
