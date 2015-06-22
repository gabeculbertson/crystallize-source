using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobSelectionProcess : IProcess<object, JobRef> {

    public event ProcessExitCallback OnExit;

    IPanelItemSelector<JobRef> panel;

    public void Initialize(object data) {
        panel = JobSelectionPanelUI.GetInstance();
        panel.OnItemSelected += HandleItemSelected;
    }

    public void ForceExit() {
        Exit(null);
    }

    void HandleItemSelected(object sender, EventArgs<JobRef> e) {
        Exit(e.Data);
    }

    void Exit(JobRef args) {
        panel.Close();
        OnExit.Raise(this, args);
    }

}
