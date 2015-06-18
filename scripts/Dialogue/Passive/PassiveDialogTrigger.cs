using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PassiveDialogTrigger : MonoBehaviour {

    public Texture2D backgroundTexture;

    //PassiveDialogData dialogData;
    Transform[] speakers;
    Dictionary<Transform, bool> flippedBubbles = new Dictionary<Transform, bool>();
    bool finished = false;
    bool logged = false;
    IEnumerator routine;

    public event EventHandler OnDialogOpened;

    // Use this for initialization
    void Start() {

    }

    public void Initialize(PassiveDialogActor actor1) {
        speakers = new Transform[1];
        speakers[0] = actor1.transform;
    }

    public void Initialize(PassiveDialogActor actor1, PassiveDialogActor actor2) {//, PassiveDialogData dialogData){
        speakers = new Transform[2];
        speakers[0] = actor1.transform;
        speakers[1] = actor2.transform;
        //this.dialogData = dialogData;
    }

    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody.tag == "Player") {
            //if(!logged){
            BeginDialog();

            if (Application.loadedLevelName == "Cards_Level01") {
                MainCanvas.main.OpenNotificationPanel("Better figure out what's going on... (drag words from conversations onto Objectives or the Inventory)");
            }
            //}
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody.tag == "Player") {
            if (finished) {
                if (!logged) {
                    //InteractiveDialogPanel.main.LogDialog();
                    logged = true;
                }
            } else {
                //InteractiveDialogPanel.main.Clear();
                StopCoroutine(routine);
            }

            foreach (var a in speakers) {
                a.GetComponent<PassiveDialogActor>().SetPhrase(null);
            }
        }
    }

    void BeginDialog() {
        //InteractiveDialogPanel.main.dialogTarget = this;
        if (OnDialogOpened != null) {
            OnDialogOpened(this, EventArgs.Empty);
        }

        routine = PlayDialog();
        StartCoroutine(routine);
    }

    /// <summary>
    /// Plays the dialog by adding phrases in 1 second intervals
    /// </summary>
    /// <returns>The sequence</returns>
    IEnumerator PlayDialog() {
        foreach (var a in speakers) {
            a.GetComponent<PassiveDialogActor>().PlayPhrase();
            yield return null;
        }
    }


    void SetPanelRegion() {
        var guiPoint1 = Camera.main.WorldToScreenPoint(speakers[0].transform.position + Vector3.up * 2f);
        var guiPoint2 = Camera.main.WorldToScreenPoint(speakers[1].transform.position + Vector3.up * 2f);
        var width = Mathf.Max(350f, Mathf.Abs(guiPoint1.x - guiPoint2.x));
        var centerX = (guiPoint1.x + guiPoint2.x) * 0.5f;
        var bottomY = Mathf.Max(guiPoint1.y, guiPoint2.y);

        var newRect = new Rect(0, 0, width, 650f);
        newRect.x = centerX - width * 0.5f;
        newRect.yMax = Screen.height - bottomY;

        if (guiPoint1.x < guiPoint2.x) {
            flippedBubbles[speakers[0].transform] = false;
            flippedBubbles[speakers[1].transform] = true;
        } else {
            flippedBubbles[speakers[0].transform] = true;
            flippedBubbles[speakers[1].transform] = false;
        }
    }

}