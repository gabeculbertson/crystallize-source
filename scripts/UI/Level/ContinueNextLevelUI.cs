using UnityEngine;
using System.Collections;

public class ContinueNextLevelUI : UIMonoBehaviour {

	public GameObject fireworksPrefab;

	float time = 0;
	string levelString;

    bool transitionStarted = false;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);

		ObjectiveManager.main.OnLevelComplete += HandleOnLevelComplete;
		MainCanvas.main.NextLevelForced += HandleOnLevelComplete;
	}

	void Update(){
		time += Time.deltaTime;

        if (LevelSettings.AutomaticallyTransition()) {
            if (time <= 3f) {
                canvasGroup.alpha = 0;
            } else if (time <= 4f) {
                if (!transitionStarted) {
                    Instantiate(EffectLibrary.Instance.uiFadeOutEffect);
                    transitionStarted = true;
                }
            } else {
                LoadNextLevel();
            }
        }

		if (Input.GetMouseButtonDown (0)) {
			time = 10f;
		}
	}

	void HandleOnLevelComplete (object sender, System.EventArgs e)
	{
		EffectManager.main.EnqueueEffect (Open, 0);
	}
	
	// Update is called once per frame
	void Open () {
		var instance = Instantiate (fireworksPrefab) as GameObject;
        instance.transform.position = PlayerManager.Instance.PlayerGameObject.transform.position;
		time = 0;
		canvasGroup.alpha = 0;
		gameObject.SetActive (true);
	}

	public void LoadNextLevel(){
		PlayerDataLoader.Save ();
		Application.LoadLevel (LevelSettings.main.nextLevel);
	}


}
