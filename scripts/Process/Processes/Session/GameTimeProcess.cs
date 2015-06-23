using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class GameTimeProcess : MonoBehaviour {

	public static GameTimeProcess GetTestInstance(){
		var gtp = new GameObject ("TestTime").AddComponent<GameTimeProcess>();
		gtp.isTesting = true;
		return gtp;
	}

    public static readonly ProcessFactoryRef<MorningSessionArgs, JobRef> MorningFactory = new ProcessFactoryRef<MorningSessionArgs, JobRef>();
    public static readonly ProcessFactoryRef<DaySessionArgs, object> DayFactory = new ProcessFactoryRef<DaySessionArgs, object>();
    public static readonly ProcessFactoryRef<EveningSessionArgs, HomeRef> EveningFactory = new ProcessFactoryRef<EveningSessionArgs, HomeRef>();
    public static readonly ProcessFactoryRef<NightSessionArgs, MorningSessionArgs> NightFactory = new ProcessFactoryRef<NightSessionArgs, MorningSessionArgs>();

    public StandardScriptableObjectSessionData morningSession;
    public StandardScriptableObjectSessionData daySession;
    public StandardScriptableObjectSessionData eveningSession;
    public StandardScriptableObjectSessionData nightSession;

    int currentProcess;
	bool isTesting = false;

    void Start() {
		Debug.Log ("Main process started");
		MainProcess.Initialize ();
		MainProcess.InstantiateNewSceneObjects ();

		if (!isTesting) {
			MorningFactory.Get (new MorningSessionArgs (morningSession.SessionArea, new HomeRef (0)), MorningSessionCallback, null);
		}
	}

    void OnLevelWasLoaded(int level) {
        MainProcess.InstantiateNewSceneObjects();
    }

    public void MorningSessionCallback(object sender, JobRef args) {
        Debug.Log("Morning exited:" + args);
        if (isTesting) {
			return;
		}

		if (args == null) {
            PlayerData.Instance.Time.Session = (int)TimeSessionType.Evening;
            EveningFactory.Get(new EveningSessionArgs(eveningSession.SessionArea), EveningSessionCallback, null); 
        } else {
            PlayerData.Instance.Time.Session = (int)TimeSessionType.Day;
            DayFactory.Get(new DaySessionArgs(daySession.SessionArea, args), DaySessionCallback, null);
        }
    }

    void DaySessionCallback(object sender, object args) {
        PlayerData.Instance.Time.Session = (int)TimeSessionType.Evening;
        EveningFactory.Get(new EveningSessionArgs(eveningSession.SessionArea), EveningSessionCallback, null); 
    }

    void EveningSessionCallback(object sender, HomeRef args) {
        PlayerData.Instance.Time.Session = (int)TimeSessionType.Night;
        NightFactory.Get(new NightSessionArgs(nightSession.SessionArea, args), NightSessionCallback, null);
    }

    void NightSessionCallback(object sender, MorningSessionArgs args) {
        PlayerData.Instance.Time.Day++;
        PlayerData.Instance.Time.Session = (int)TimeSessionType.Morning;
        MorningFactory.Get(args, MorningSessionCallback, null);
    }

}
