using UnityEngine;
using System.Collections;

public class NotificationDisplayer : MonoBehaviour {

	public string text;

	// Use this for initialization
	IEnumerator Start () {
		while(!MainCanvas.main){
			yield return null;
		}

		MainCanvas.main.OpenNotificationPanel (text);
	}

}
