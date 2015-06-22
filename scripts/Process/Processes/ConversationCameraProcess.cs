using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ConversationCameraProcess : IProcess<GameObject, object> {

    public event ProcessExitCallback OnExit;

    ITemporaryUI<GameObject, object> instance;

    public void Initialize(GameObject param1) {
        instance = UILibrary.ConversationCamera.Get(param1);
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        instance.Close();
    }

}
