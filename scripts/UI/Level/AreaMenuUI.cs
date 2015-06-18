using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaMenuUI : MonoBehaviour {

    public GameObject buttonPrefab;
    public List<int> areaIDs = new List<int>();

	// Use this for initialization
	void Start () {
        if (UISystem.main.ContainsCenterPanel()) {
            Destroy(gameObject);
            return;
        }

        foreach (var areaID in areaIDs) {
            var buttonInstance = Instantiate<GameObject>(buttonPrefab);
            buttonInstance.GetComponent<AreaMenuButtonUI>().Initialize(areaID);
            buttonInstance.transform.SetParent(transform);
        }

        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        UISystem.main.AddCenterPanel(this);
	}

    public void Close() {
        if (UISystem.main) {
            UISystem.main.RemoveCenterPanel(this);
        }
        Destroy(gameObject);
    }

}
