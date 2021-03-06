using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreezeNextLevelSocialActor : MonoBehaviour {

    //public GameObject networkCardPrefab;
    //public GameObject friendRequestButtonsPrefab;
    //public GameObject setEffect;
    //public SocialData socialData;
    public string nextLevelName = "";
    bool unlocked = false;

    // Use this for initialization
    void Start() {
        GetComponent<InteractiveDialogActor>().OnDialogueSuccess += HandleOnUnlock;
    }

    void Update() {
        if (!unlocked) {
            PlayerData.Instance.LevelData.SetLevelState(nextLevelName, LevelState.Hidden);
        }
    }

    void HandleOnUnlock(object sender, PhraseEventArgs e) {
        unlocked = true;
        PlayerData.Instance.LevelData.SetLevelState(nextLevelName, LevelState.Locked);
        MainCanvas.main.OpenNotificationPanel("It looks like the girl is willing to show the way out. You may now progress to the next level (hit tab).");
    }

}
