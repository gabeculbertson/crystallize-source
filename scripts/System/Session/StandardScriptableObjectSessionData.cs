using UnityEngine;
using System.Collections;

public class StandardScriptableObjectSessionData : ScriptableObjectSessionData {

    [SerializeField]
    string sessionArea = "";

    public string SessionArea
    {
        get
        {
            return sessionArea;
        }
    }

    public virtual TimeSessionArgs GetArgs() {
        return new TimeSessionArgs(sessionArea);
    }

}
