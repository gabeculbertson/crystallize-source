using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class MorningProcess : TimeSessionProcess<MorningSessionArgs, DaySessionArgs>, IProcess<MorningSessionArgs, DaySessionArgs> {

    public static readonly ProcessFactoryRef<object, DaySessionArgs> RequestPlanSelection = new ProcessFactoryRef<object,DaySessionArgs>();

    MorningSessionArgs args;

    protected override void BeforeInitialize(MorningSessionArgs input) {
        args = input;
    }

    protected override void AfterLoad() {
        if (BedSelector.Instance) {
            BedSelector.Instance.SetBed(args.Home.ID);
        }
        if (SunInstance.Instance) {
            SunInstance.Instance.SetMorning();
        }

        string s = "";
        var quality = args.Home.GameDataInstance.Quality;
        PlayerData.Instance.RestQuality = quality;
        if (quality < 0.95f) {
            s = "You didn't sleep so well... You'll probably have a hard time working today.";
        } else if (quality < 1.05f) {
            s = "You slept alright. You feel ready for work.";
        } else {
            s = "You slept great! You should be able to work hard today.";
        }
        ProcessLibrary.MessageBox.Get(s, MessageBoxCallback, this);
    }

    void MessageBoxCallback(object sender, object args) {
        RequestPlanSelection.Get(null, PlanSelectionCallback, this);
    }

    void PlanSelectionCallback(object sender, DaySessionArgs args) {
        Exit(args);
    }

}
