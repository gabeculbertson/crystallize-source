using UnityEngine;
using System;
using System.Collections;

public class SequenceRequestEventArgs<I, O> : EventArgs {

    public I Data { get; set; }
    public SequenceRequest<O> SequenceRequest { get; set; }

    public SequenceRequestEventArgs(I data, SequenceRequest<O> seqReq){
        Data = data;
        SequenceRequest = seqReq;
    }

}