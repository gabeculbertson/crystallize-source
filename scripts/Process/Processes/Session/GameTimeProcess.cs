using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class GameTimeProcess : MonoBehaviour {

	public static GameTimeProcess GetTestInstance(){
		GameObject go = new GameObject ("TestTime");
		var gtp = go.AddComponent<GameTimeProcess>();
		go.AddComponent<DontDestroyOnLoad> ();
		gtp.isTesting = true;
		return gtp;
	}

	public static GameTimeProcess GetTestInstance(JobRef job){
		GameObject go = new GameObject ("TestTime");
		var gtp = go.AddComponent<GameTimeProcess>();
		go.AddComponent<DontDestroyOnLoad> ();
		gtp.isTesting = true;
		gtp.testJob = job;
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

	public JobRef testJob = null;
    int currentProcess;
	public bool isTesting;

    void Start() {
		Debug.Log ("Main process started");
		MainProcess.Initialize ();
		MainProcess.InstantiateNewSceneObjects ();
		if (isTesting) {
			MorningSessionCallback(null, testJob);
		}
		else {
			MorningFactory.Get (new MorningSessionArgs (morningSession.SessionArea, new HomeRef (0)), MorningSessionCallback, null);
		}
	}

    void OnLevelWasLoaded(int level) {
        MainProcess.InstantiateNewSceneObjects();
    }

    public void MorningSessionCallback(object sender, JobRef args) {
        Debug.Log("Morning exited:" + args);
        if (isTesting) {
			DayFactory.Get(new DaySessionArgs(daySession.SessionArea, args), DaySessionCallback, null);
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
