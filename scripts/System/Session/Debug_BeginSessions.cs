using UnityEngine;
using System.Collections;

public class Debug_BeginSessions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    GetComponent<UIButton>().OnClicked += Debug_BeginSessions_OnClicked;
	}

    void Debug_BeginSessions_OnClicked(object sender, System.EventArgs args)
    {
        DayManager.Begin();
    }
	
}
