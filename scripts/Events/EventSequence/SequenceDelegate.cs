public delegate void SequenceRequestHandler<I, O>(object sender, SequenceRequestEventArgs<I, O> args);
public delegate void SequenceRequestCallback<O>(object sender, SequenceCallbackEventArgs<O> args);
public delegate void SequenceCompleteCallback<O>(object sender, SequenceCompleteEventArgs<O> args);