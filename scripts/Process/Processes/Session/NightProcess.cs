﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NightProcess : TimeSessionProcess<NightSessionArgs, MorningSessionArgs>, IProcess<NightSessionArgs, MorningSessionArgs> {

    public static readonly ProcessFactoryRef<object, int> RequestReviews = new ProcessFactoryRef<object,int>();

    NightSessionArgs nightArgs;

    protected override void BeforeInitialize(NightSessionArgs input) {
        nightArgs = input;
    }

    protected override void AfterLoad() {
        RequestReviews.Get(null, ReviewsCompleteCallback, this);
        if (BedSelector.Instance) {
            BedSelector.Instance.SetBed(nightArgs.Home.ID);
        }
    }

    void ReviewsCompleteCallback(object sender, int args) {
        var result = PlayerDataConnector.AddReviewExperience(args);
        ProcessLibrary.MessageBox.Get(result.ToMessageBoxString(), MessageBoxCallback, this);
    }

    void MessageBoxCallback(object sender, object args) {
        Exit(new MorningSessionArgs(nightArgs.LevelName, nightArgs.Home));
    }

}
