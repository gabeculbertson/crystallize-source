using UnityEngine;
using System.Collections;

public class ConversationProcessPart {

    static IProcess cameraProcess;

    protected void StartCamera(GameObject obj) {
        if (cameraProcess != null) {
            cameraProcess.ForceExit();
        }

        cameraProcess = ConversationSequence.RequestConversationCamera.Get(obj, Callback, null);
    }

    protected void StopCamera() {
        if (cameraProcess != null) {
            cameraProcess.ForceExit();
            cameraProcess = null;
        }
    }

    void Callback(object sender, object output) {    }

}