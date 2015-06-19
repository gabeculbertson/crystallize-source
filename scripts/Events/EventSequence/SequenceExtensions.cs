using UnityEngine;
using System.Collections;

public static class SequenceExtensions {

    //public static void PipeThrough<T>(this SequenceRequestEventArgs<T, T> args) {
    //    var s = new PipeSelectionSequence<T>(args.Data);
    //    args.SequenceRequest.RaiseCallback(s);
    //    s.Continue();
    //}

    public static void SetAsChild<I1, O1, I2, O2>(this IProcess<I1,O1> c, IProcess<I2,O2> p) {
        ProcessExitCallback<O2> exitChild = (s, e) => OnParentExit(c);
        ProcessExitCallback<O1> exitParent = (s, e) => OnChildExit(p, exitChild);
        p.OnExit += exitChild;
        c.OnExit += exitParent;
    }

    static void OnParentExit<I, O>(IProcess<I, O> child) {
        child.ForceExit();
    }

    static void OnChildExit<I, O>(IProcess<I, O> parent, ProcessExitCallback<O> childEvent) {
        parent.OnExit -= childEvent;
    }


}