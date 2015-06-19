using UnityEngine;
using System;
using System.Collections;

public static class EventExtensions {

    public static void Raise(this EventHandler eventHandler, object sender, EventArgs args)
    {
        if (eventHandler != null)
        {
            eventHandler(sender, args);
        }
    }

    public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T args) where T : EventArgs
    {
        if (eventHandler != null)
        {
            eventHandler(sender, args);
        }
    }

    public static void Raise<T>(this ProcessExitCallback<T> eventHandler, object sender, ProcessExitEventArgs<T> args) {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }

    public static void SetHandler<I, O>(this ProcessRequestHandler<I, O> processRequestHandler, GetProcessInstance<I, O> getProcessInstance) {
        processRequestHandler = (s, e) => ProcessRequestHandler(getProcessInstance, s, e);
    }

    static void ProcessRequestHandler<I, O>(GetProcessInstance<I, O> getProcessInstance, object sender, ProcessRequestEventArgs<I, O> args) {
        var process = getProcessInstance(args.Data);
        //process.
        process.OnExit += args.Callback;
    }

}
