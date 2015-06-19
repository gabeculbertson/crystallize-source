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

}
