using UnityEngine;
using System.Collections;

public static class Process {

    public static void Connect<I,O>(ref ProcessRequestHandler<I, O> requester, GetProcessInstance<I, O> getProcess) {
        requester = (s, e) => ProcessRequestHandler(getProcess, s, e);
    }

    static void ProcessRequestHandler<I, O>(GetProcessInstance<I, O> getProcessInstance, object sender, ProcessRequestEventArgs<I, O> args) {
        var process = getProcessInstance();

        if (args.Parent != null) {
            ProcessExitCallback forceChildExit = (s, e) => process.ForceExit();
            ProcessExitCallback detachChild = (s, e) => {
                args.Parent.OnReturn -= forceChildExit;
                Debug.Log("Detached child: " + process);
            };
            process.OnReturn += detachChild;
            args.Parent.OnReturn += forceChildExit;
        }

        ProcessExitCallback castCallback = (s, e) => args.Callback(s, (ProcessExitEventArgs<O>)e);
        process.OnReturn += castCallback;

        Debug.Log("Started: " + process);
        process.OnReturn += (s, e) => Debug.Log("Ended: " + process);

        process.Initialize(args.Data);
    }

}