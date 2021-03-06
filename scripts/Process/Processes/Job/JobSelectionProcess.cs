﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobSelectionProcess : IProcess<object, DaySessionArgs> {

    public event ProcessExitCallback OnExit;

    ITemporaryUI<object, DaySessionArgs> panel;

    public void Initialize(object data) {
        PlayerDataConnector.UpdateShownJobs();
        panel = UILibrary.Jobs.Get(null);
        panel.Complete += HandleItemSelected;
    }

    public void ForceExit() {
        Exit(null);
    }

    void HandleItemSelected(object sender, EventArgs<DaySessionArgs> e) {
        Exit(e.Data);
    }

    void Exit(DaySessionArgs args) {
        panel.Close();
        OnExit.Raise(this, args);
    }

}
