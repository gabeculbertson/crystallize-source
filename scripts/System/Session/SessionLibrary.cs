using UnityEngine;
using System.Collections;

public class SessionLibrary : ScriptableObject {

    [SerializeField]
    ScriptableObjectSessionData morningSessionData;
    [SerializeField]
    ScriptableObjectSessionData eveningSessionData;
    [SerializeField]
    ScriptableObjectSessionData nightSessionData;

    public IDaySession GetSession(int time)
    {
        IDaySession session;
        switch (time % 3)
        {
            case 0:
                session = new DebugSession();
                morningSessionData.LoadTo(session);
                return session;
            case 1:
                session = new DebugSession();
                eveningSessionData.LoadTo(session);
                return session;
            case 2:
                session = new DebugSession();
                nightSessionData.LoadTo(session);
                return session;
        }
        return null;
    }

}
