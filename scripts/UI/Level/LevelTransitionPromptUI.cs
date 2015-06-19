using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTransitionPromptUI : MonoBehaviour {

	public static LevelTransitionPromptUI current { get; set; }

	public Text promptText;

	string targetLevel;

	// Use this for initialization
	void Start () {
		transform.SetParent (MainCanvas.main.transform);
		transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * .5f);
		current = this;
	}

	void OnDestroy () {
		current = null;
	}

	public void Initiallize(string targetLevel, string levelString){
		this.targetLevel = targetLevel;
		promptText.text = string.Format ("Go to {0}?", levelString); 
	}

	public void GoToNextLevel(){
		if (targetLevel != null) {
			PlayerManager.main.Save ();
			Application.LoadLevel(targetLevel);
		}
	}

	public void Close(){
		Destroy (gameObject);
	} 

}
