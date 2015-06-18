using UnityEngine;
using System;
using System.Collections;

public interface ISelectionSequence<O> {

    event EventHandler OnCancel;
    event EventHandler OnExit;
    event SequenceCompleteCallback<O> OnSelection;

}
