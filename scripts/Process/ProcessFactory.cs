using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;

public class ProcessFactory<I, O>  {
    public virtual IProcess<I, O> GetInstance() {
        return null; 
    }
}

public class ProcessFactory<T, I, O> : ProcessFactory<I, O> where T : IProcess<I, O>, new() {
    public override IProcess<I, O> GetInstance() {
        return new T();
    }
}

public class UIProcessFactory<T, I, O> : ProcessFactory<I, O> where T : IUIProcess<I, O>, new() {

    Func<ITemporaryUI<I, O>> getter;

    public UIProcessFactory(Func<ITemporaryUI<I, O>> getter) {
        this.getter = getter;
    }

    public override IProcess<I, O> GetInstance() {
        var t = new T();
        t.SetUIInstance(getter());
        return t;
    }

}

public class DynamicProcessFactory<I, O> : ProcessFactory<I, O> {

    Type t;

    public DynamicProcessFactory(Type t){
        this.t = t;
    }

    public override IProcess<I, O> GetInstance() {
        return (IProcess<I, O>)Activator.CreateInstance(t);
    }

}

public class ProcessFactoryRef<I, O> {
    
    public ProcessFactory<I, O> Factory { get; set; }

    public void Set<T>() where T : IProcess<I, O>, new() {
        Factory = new ProcessFactory<T, I, O>();
    }

    public void Set(Type t) {
        Factory = new DynamicProcessFactory<I, O>(t);
    }

}

public class UIFactory<I, O> {

    Func<ITemporaryUI<I, O>> getter;

    public UIFactory(Func<ITemporaryUI<I, O>> getter) {
        this.getter = getter;
    }

    public ITemporaryUI<I, O> GetInstance() {
        return getter();
    }

}

public class UIFactoryRef<I, O> {

    public UIFactory<I, O> Factory { get; set; }

    public ITemporaryUI<I, O> Get(I args) {
        if (Factory == null) {
            Debug.LogError(this + " has not been set!");
        }
        var i = Factory.GetInstance();
        i.Initialize(args);
        return i;
    }

    public void Set(Func<ITemporaryUI<I, O>> getter) {
        Factory = new UIFactory<I, O>(getter);
    }

}

public static class ProcessFactoryExtentions {

    public static IProcess Get<I, O>(this ProcessFactoryRef<I, O> fact, I input, ProcessExitCallback<O> callback, IProcess parent) {
        if (fact == null) {
            Debug.LogError(string.Format("Factory reference has not been intialized. ({0})", fact));
            return null;
        }

        IProcess<I, O> process = null;
        if (fact == null) {
            Debug.LogWarning(string.Format("Factory instance has not been intialized. Using temporary process. ({0})", fact));
            process = new TempProcess<I, O>();
        } else {
            process = fact.Factory.GetInstance();
        }

        if (parent != null) {
            ProcessExitCallback forceChildExit = (s, e) => process.ForceExit();
            ProcessExitCallback detachChild = (s, e) => parent.OnExit -= forceChildExit;
            process.OnExit += detachChild;
            parent.OnExit += forceChildExit;
        }

        ProcessExitCallback castCallback = (s, e) => callback(s, (O)e);
        process.OnExit += castCallback;

        process.Initialize(input);
        return process;
    }

}