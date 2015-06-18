using UnityEngine;
using System.Collections;

public static class SequenceExtensions {

    public static void PipeThrough<T>(this SequenceRequestEventArgs<T, T> args) {
        var s = new PipeSelectionSequence<T>(args.Data);
        args.SequenceRequest.RaiseCallback(s);
        s.Continue();
    }

}