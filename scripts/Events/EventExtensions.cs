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

    public static void Raise<I, O>(this SequenceRequestHandler<I, O> eventHandler, object sender, SequenceRequestEventArgs<I, O> args) {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }

    public static void Raise<T>(this SequenceRequestCallback<T> eventHandler, object sender, SequenceCallbackEventArgs<T> args) {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }
    
    public static void Raise<T>(this SequenceCompleteCallback<T> eventHandler, object sender, SequenceCompleteEventArgs<T> args) {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }

}
