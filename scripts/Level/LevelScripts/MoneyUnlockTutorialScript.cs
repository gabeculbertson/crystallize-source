using UnityEngine;
using System.Collections;

public class MoneyUnlockTutorialScript : LevelScript {

	// Use this for initialization
	void Start () {
        SetMessage("Explore the area!", 5f);

        CrystallizeEventManager.UI.OnTutorialEvent += HandleTutorialEvent;
	}

    void HandleTutorialEvent(object sender, System.EventArgs e) {
        if (e is MoneyLockTutorialEventArgs) {
            StartCoroutine(WaitAndSet());
        }
    }

    IEnumerator WaitAndSet() {
        yield return new WaitForSeconds(1f);
        SetMessage("Complete quests to earn money", 5f);
    }
	
}
