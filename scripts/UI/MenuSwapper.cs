using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSwapper : MonoBehaviour {

    public static MenuSwapper main { get; set; }

    public static void LockSwapping(object obj) {
        if (main) {
            if (!main.locks.Contains(obj)) {
                main.locks.Add(obj);
            }
        }
    }

    public static void UnlockSwapping(object obj) {
        if (main) {
            if (main.locks.Contains(obj)) {
                main.locks.Remove(obj);
            }
        }
    }

    HashSet<object> locks = new HashSet<object>();

    void Awake() {
        main = this;
    }

    // Use this for initialization
    IEnumerator Start() {
        if (!FieldCanvas.main || !SecondaryCanvas.main) {
            Debug.Log("Canvases not found.");
            Destroy(this);
            yield break;
        }

        yield return null;
        FieldCanvas.main.gameObject.SetActive(true);
        SecondaryCanvas.main.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (locks.Count > 0) {
            //Debug.Log("Locked: " );
            //foreach(var o in locks){
            //	Debug.Log(o);
            //}
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            SwapMenus();
        }
    }

    public void SwapMenus() {
        if (FieldCanvas.main.gameObject.activeSelf) {
            SwapToSecondary();
        } else {
            SwapToPrimary();
        }
    }

    public void SwapToPrimary() {
        FieldCanvas.main.gameObject.SetActive(true);
        SecondaryCanvas.main.gameObject.SetActive(false);
    }

    public void SwapToSecondary() {
        FieldCanvas.main.gameObject.SetActive(false);
        SecondaryCanvas.main.gameObject.SetActive(true);

        if (Application.loadedLevelName == "Cards_Level05" && PlayerData.Instance.LevelData.GetLevelStateData("Cards_Level05").LevelState == LevelState.Hidden) {
            MainCanvas.main.OpenNotificationPanel("You're not sure where to go now. Perhaps you should increase your fluency (right click on word-grid words) and talk to some people?");
        } else {
            MainCanvas.main.OpenNotificationPanel("Drag words onto the grid to begin gaining credit. Go to the next level when you are ready.");
        }
    }
}
