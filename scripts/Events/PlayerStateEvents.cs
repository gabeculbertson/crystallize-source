using UnityEngine;
using System;
using System.Collections;

public class PlayerStateEvents : GameEvents {

    public event EventHandler OnGameEvent;
    public void RaiseGameEvent(object sender, EventArgs args) { OnGameEvent.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnWordFound;
    public void RaiseOnWordFound(object sender, PhraseEventArgs args) { OnWordFound.Raise(sender, args); }

    public event EventHandler<TextEventArgs> OnFlagChanged;
    public void RaiseFlagChanged(object sender, TextEventArgs args) { OnFlagChanged.Raise(sender, args); }

    public event EventHandler<QuestStateChangedEventArgs> OnQuestStateChanged;
    public void RaiseQuestStateChanged(object sender, QuestStateChangedEventArgs args) { OnQuestStateChanged.Raise(sender, args); }
    public event EventHandler<QuestEventArgs> OnQuestStateRequested;
    public void RaiseQuestStateRequested(object sender, QuestEventArgs args) { OnQuestStateRequested.Raise(sender, args); }
    public event EventHandler<QuestEventArgs> OnActiveQuestChanged;
    public void RaiseActiveQuestChanged(object sender, QuestEventArgs args) { OnActiveQuestChanged.Raise(sender, args); }

    public event EventHandler OnMoneyChanged;
    public void RaiseMoneyChanged(object sender, EventArgs args) { OnMoneyChanged.Raise(sender, args); }
    public event EventHandler OnAreaUnlocked;
    public void RaiseAreaUnlocked(object sender, EventArgs args) { OnAreaUnlocked.Raise(sender, args); }
    public event EventHandler OnHomesChanged;
    public void RaiseHomesChanged(object sender, EventArgs args) { OnHomesChanged(sender, args); }

    public event EventHandler<PhraseEventArgs> OnCollectPhraseRequested;
    public void RaiseCollectPhraseRequested(object sender, PhraseEventArgs args) { OnCollectPhraseRequested.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnPhraseCollected;
    public void RaisePhraseCollected(object sender, PhraseEventArgs args) { OnPhraseCollected.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnCollectWordRequested;
    public void RaiseCollectWordRequested(object sender, PhraseEventArgs args) { OnCollectWordRequested.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnWordCollected;
    public void RaiseWordCollected(object sender, PhraseEventArgs args) { OnWordCollected.Raise(sender, args); }

}
