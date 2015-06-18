using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is used for triggering actions when it might be difficult or annoying to trigger them through normal game means.
/// </summary>
public class DebugSystem : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Insert)) {
            CrystallizeEventManager.Debug.RaiseDebugTextRequested(this, System.EventArgs.Empty);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            PlayerManager.main.Save();
            //SavePlayerData(playerData, Application.dataPath + "TempPlayerData.xml");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            Application.LoadLevel(LevelSettings.main.nextLevel);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            PlayerManager.main.playerData.InventoryState.Level = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            PlayerManager.main.playerData.InventoryState.Level = 100;
        }
#endif
    }
}
