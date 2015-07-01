using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class TimeSessionProcess {
    public static readonly ProcessFactoryRef<string, object> TransitionFactory = new ProcessFactoryRef<string,object>();
}

public abstract class TimeSessionProcess<I, O> : IProcess<I, O> where I : TimeSessionArgs {

    public event ProcessExitCallback OnExit;

    string nextLevel = "";

    public virtual void Initialize(I input) {
        BeforeInitialize(input);
        Run(input);
    }

    public void ForceExit() {
        Exit();
    }

    protected void Run(I args) {
        nextLevel = SelectNextLevel(args);
        TimeSessionProcess.TransitionFactory.Get(PlayerData.Instance.Time.GetFormattedString(), TransitionCompleteCallback, this);
    }

    protected virtual string SelectNextLevel(I args) {
        return args.LevelName;
    }

    protected virtual void BeforeInitialize(I input) { }

    protected abstract void AfterLoad();

    protected void Exit() {
        Exit(default(O));
    }

    protected void Exit(O args) {
        CrystallizeEventManager.OnLoadComplete -= HandleLoadComplete;
        OnExit(this, args);
    }

    void TransitionCompleteCallback(object sender, object args) {
        CrystallizeEventManager.OnLoadComplete += HandleLoadComplete;
        Application.LoadLevel(nextLevel);
    }

    void HandleLoadComplete(object sender, System.EventArgs args) {
        CrystallizeEventManager.OnLoadComplete -= HandleLoadComplete;
        AfterLoad();
    }

}
