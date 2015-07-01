using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[JobProcessType]
public class StandardConversationProcess : IProcess<JobTaskRef, object> {

    public event ProcessExitCallback OnExit;

    GameObject person;
    JobTaskRef task;

    public void Initialize(JobTaskRef param1) {
        task = param1;
        person = new SceneObjectRef(task.Data.Actor).GetSceneObject();
        ProcessLibrary.Conversation.Get(new ConversationArgs(person, task.Data.Dialogue), HandleConversationExit, this);
    }

    void HandleConversationExit(object sender, object obj) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        OnExit.Raise(this, null);
    }

}
