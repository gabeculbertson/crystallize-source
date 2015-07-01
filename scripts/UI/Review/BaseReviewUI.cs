using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public abstract class BaseReviewUI : UIPanel {

    public event EventHandler<EventArgs<int>> Complete;

    public ItemReviewPlayerData ActiveReview { get; set; }

    int count = 0;

    protected virtual void Refresh() {    }
    protected virtual void OnSetResult() { }

    public void Skip() {
        Complete.Raise(this, new EventArgs<int>(count));
        Close();
    }

    public void SetResult(int result) {
        count++;
        if (ActiveReview != null) {
            ActiveReview.AddEntry(result);
            ActiveReview = null;
        }
        OnSetResult();
    }

    public void Exit() {
        Complete.Raise(this, new EventArgs<int>(count));
        Close();
    }

}
