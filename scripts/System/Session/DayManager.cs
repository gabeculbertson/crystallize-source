using UnityEngine;
using System.Collections;

public class DayManager {

    static DayManager _instance;

    public static DayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DayManager();
            }
            return _instance;
        }
    }

    public static void Begin()
    {
        Instance.BeginTransition();
    }

    SessionLibrary sessionLibrary;
    // this should be in player data
    int currentTime = -1;

    public DayManager()
    {
        // may want to load this as a singleton if we need it elsewhere
        sessionLibrary = Resources.Load<SessionLibrary>("SessionLibrary");
    }

    void BeginTransition()
    {
        Debug.Log("Transition starting");
        var transitionGO = GameObject.Instantiate(EffectLibrary.Instance.uiTimeTransition);
        transitionGO.GetInterface<IInitializable<string>>().Initialize(GetTimeString(currentTime + 1));
        var transition = transitionGO.GetInterface<ILockEvent>();
        transition.OnUnlock += OnTransitionComplete;
    }

    void OnTransitionComplete(object sender, System.EventArgs e)
    {
        MoveNext();
    }

    public void MoveNext()
    {
        currentTime++; //= (currentSession + 1) % sessionFactories.Length;
        var session = sessionLibrary.GetSession(currentTime); //sessionFactories[currentSession].GetNewSession();
        session.OnComplete += HandleSessionComplete;
        session.Begin();
    }

    string GetTimeString(int time)
    {
        var timeSlot = "";
        switch (time % 3)
        {
            case 0:
                timeSlot = "Morning";
                break;
            case 1:
                timeSlot = "Evening";
                break;
            case 2:
                timeSlot = "Night";
                break;
            default:
                timeSlot = "NULL";
                break;
        }
        var day = (int)(time / 3);
        return string.Format("Day {0}: {1}", day, timeSlot);
    }

    void HandleSessionComplete(object sender, System.EventArgs args)
    {
        BeginTransition();
    }

}
