public delegate void ProcessRequestHandler<I, O>(object sender, ProcessRequestEventArgs<I, O> args);
public delegate IProcess<I, O> GetProcessInstance<I, O>();
public delegate void ProcessExitCallback(object sender, ProcessExitEventArgs args);
public delegate void ProcessExitCallback<O>(object sender, ProcessExitEventArgs<O> args);