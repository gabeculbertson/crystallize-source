﻿using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TempProcess<I, O> : IProcess<I, O> {

    public event ProcessExitCallback OnExit;

    NotImplementedYetUI panel;

    public void Initialize(I data) {
        panel = NotImplementedYetUI.GetInstance();
        panel.SetProcessText(data);
        CrystallizeEventManager.Input.OnLeftClick += HandleLeftClick;
    }

    void HandleLeftClick(object sender, EventArgs e) {
        Exit();
    }

    public void ForceExit() {
        Exit();
    }

    void Exit() {
        panel.Close();
        CrystallizeEventManager.Input.OnLeftClick -= HandleLeftClick;
        OnExit(this, null);
    }

}
