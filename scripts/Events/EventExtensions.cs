﻿using UnityEngine;
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

    public static void Raise<I, O>(this ProcessExitCallback eventHandler, IProcess<I, O> sender, object args) {
        if (eventHandler != null) {
            eventHandler(sender, args);
        }
    }

}
