using UnityEngine;
using System;
using System.Collections;

public class EnvironmentEvents {

    public event EventHandler OnActorApproached;
    public void RaiseActorApproached(object sender, EventArgs args) { OnActorApproached.Raise(sender, args); }
    public event EventHandler OnActorDeparted;
    public void RaiseActorDeparted(object sender, EventArgs args) { OnActorDeparted.Raise(sender, args); }

    public event EventHandler BeforeCameraMove;
    public void RaiseBeforeCameraMove(object sender, EventArgs args) { BeforeCameraMove.Raise(sender, args); }
    public event EventHandler AfterCameraMove;
    public void RaiseAfterCameraMove(object sender, EventArgs args) { AfterCameraMove.Raise(sender, args); }

    public event EventHandler BeforeSceneChange;
    public void RaiseBeforeSceneChange(object sender, EventArgs args) { BeforeSceneChange.Raise(sender, args); }

    public event EventHandler<PersonAnimationEventArgs> OnPersonAnimationRequested;
    public void RaisePersonAnimationRequested(object sender, PersonAnimationEventArgs args) { OnPersonAnimationRequested.Raise(sender, args); }

}
