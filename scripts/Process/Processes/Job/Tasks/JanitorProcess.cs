using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[JobProcessType]
public class JanitorProcess : IProcess<JobTaskRef, object> {

    public event ProcessExitCallback OnExit;

    GameObject person;
    JobTaskRef task;

    ITemporaryUI<string, string> ui;
    string targetRegion;
    int remainingCount = 0;
    int correctCount = 0;
    float score = 0;

    public void Initialize(JobTaskRef param1) {
        task = param1;
        remainingCount = GetTaskCount();
        person = new SceneObjectRef(task.Data.Actor).GetSceneObject();
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, task.Data.Dialogue), HandleConversationExit, this);
    }

    void HandleConversationExit(object sender, object obj) {
        ProcessLibrary.MessageBox.Get("Go clean the specified area.", HandleMessageBoxExit, this);
    }

    void HandleMessageBoxExit(object sender, object obj) {
        GetNewTargetRegion();
        person.GetComponent<DialogueActor>().SetLine(task.Data.Line, GetNewContext());
        ui = UILibrary.ContextActionButton.Get("Mop the floor");
        ui.Complete += ui_Complete;
    }

    void ui_Complete(object sender, EventArgs<string> e) {
        remainingCount--;
        if (e.Data == targetRegion) {
            correctCount++;
            var ui = UILibrary.PositiveFeedback.Get("");
            ui.Complete += Feedback_Complete;
        } else {
            var ui = UILibrary.NegativeFeedback.Get("");
            ui.Complete += Feedback_Complete;
        }
    }

    void Feedback_Complete(object sender, EventArgs<object> e) {
        if (remainingCount <= 0) {
            PlayExitDialogue();
        } else {
            HandleMessageBoxExit(null, null);
        }
    }

    void PlayExitDialogue() {
        var s = "";
        score = (float)correctCount / GetTaskCount();
        if (score >= 0.99f) {
            s = "Amazing! Nice work.";
        } else if( score >= 0.75f){
            s = "Hm... Not bad.";
        } else if(score >= 0.25f){
            s = "You need to work harder.";
        } else {
            s = "What are you doing!? Keep this up and I'll fire you.";
        }

        var d = new DialogueSequence();
        var de = d.GetNewDialogueElement<LineDialogueElement>();
        de.Line = new DialogueActorLine();
        de.Line.Phrase = new PhraseSequence(s);
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, d), HandleExitConversationExit, this);
    }

    void HandleExitConversationExit(object sender, object obj) {
        int money = (int)(10000 * score * PlayerData.Instance.RestQuality);
        PlayerDataConnector.AddMoney(money);
        string moneyString = string.Format("You made {0} yen today.", money);
        var ui = UILibrary.MessageBox.Get(moneyString);
        ui.Complete += ui_Complete;
    }

    void ui_Complete(object sender, EventArgs<object> e) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        if (ui != null) {
            ui.Close();
        }

        OnExit.Raise(this, null);
    }

    void GetNewTargetRegion() {
        var regions = GameObject.FindObjectsOfType<Region>();
        if(regions.Length == 0){
            Debug.LogError("No regions!");
        } else {
            Debug.Log("Found " + regions.Length + " regions");
            var r = regions[UnityEngine.Random.Range(0, regions.Length)];
            Debug.Log("Region is: " + r);
            targetRegion = r.name;
        }
    }

    ContextData GetNewContext() {
        var c = new ContextData();
        c.UpdateElement("region", new PhraseSequence(targetRegion));
        return c;
    }

    int GetTaskCount() {
        return 5;
    }

}
