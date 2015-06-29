using UnityEngine;
using System;
using System.Collections;

public class UIEvents : GameEvents{

    public event EventHandler<PhraseEventArgs> OnBeginDragWord;
    public void RaiseBeginDragWord(object sender, PhraseEventArgs args) { OnBeginDragWord.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnDropWord;
    public void RaiseOnDropWord(object sender, PhraseEventArgs args) { OnDropWord.Raise(sender, args); }

    public event EventHandler<UIRequestEventArgs> OnUIRequested;
    public void RaiseUIRequest(object sender, UIRequestEventArgs args) { OnUIRequested.Raise(sender, args); }
    public event EventHandler OnUIInteraction;
    public void RaiseUIInteraction(object sender, EventArgs args) { OnUIInteraction.Raise(sender, args); }

    public event EventHandler<UIModeChangedEventArgs> OnUIModeRequested;
    public void RaiseUIModeRequested(object sender, UIModeChangedEventArgs args) { OnUIModeRequested.Raise(sender, args); }
    public event EventHandler<UIModeChangedEventArgs> OnUIModeChanged;
    public void RaiseUIModeChanged(object sender, UIModeChangedEventArgs args) { OnUIModeChanged.Raise(sender, args); }

    public event EventHandler<SpeechBubbleRequestedEventArgs> OnSpeechBubbleRequested;
	public void RaiseSpeechBubbleRequested(object sender, SpeechBubbleRequestedEventArgs args) { OnSpeechBubbleRequested.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnSpeechBubbleOpen;
    public void RaiseSpeechBubbleOpen(object sender, PhraseEventArgs args) { OnSpeechBubbleOpen.Raise(sender, args); }

    public event EventHandler<WordClickedEventArgs> OnWordClicked;
    public void RaiseWordClicked(object sender, WordClickedEventArgs args) { OnWordClicked.Raise(sender, args); }

    public event EventHandler OnInteractiveDialogueOpened;
    public void RaiseInteractiveDialogueOpened(object sender, EventArgs args) { OnInteractiveDialogueOpened.Raise(sender, args); }
    public event EventHandler OnInteractiveDialogueClosed;
    public void RaiseInteractiveDialogueClosed(object sender, EventArgs args) { OnInteractiveDialogueClosed.Raise(sender, args); }

    public event EventHandler OnUpdateUI;
    public void RaiseUpdateUI(object sender, EventArgs args) { OnUpdateUI.Raise(sender, args); }
    public event EventHandler OnProgressEvent;
    public void RaiseOnProgressEvent(object sender, EventArgs args) { OnProgressEvent.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnBasePhraseSelected;
    public void RaiseBasePhraseSelected(object sender, PhraseEventArgs args) { OnBasePhraseSelected.Raise(sender, args); }
    
    public event EventHandler<PhraseEventArgs> OnWordSelected;
    public void RaiseWordSelected(object sender, PhraseEventArgs args) { OnWordSelected.Raise(sender, args); }

    public event EventHandler<ItemHoverEventArgs> OnHoverOverItem;
    public void RaiseHoverOverItem(object sender, ItemHoverEventArgs args) { OnHoverOverItem.Raise(sender, args); }
    public event EventHandler<ItemDragEventArgs> OnItemDragStarted;
    public void RaiseItemDragStarted(object sender, ItemDragEventArgs args) { OnItemDragStarted.Raise(sender, args); }
    public event EventHandler<ItemDragEventArgs> OnItemDropped;
    public void RaiseItemDropped(object sender, ItemDragEventArgs args) { OnItemDropped.Raise(sender, args); }
    public event EventHandler<ItemDragEventArgs> OnItemAcquired;
    public void RaiseItemAcquired(object sender, ItemDragEventArgs args) { OnItemAcquired.Raise(sender, args); }
    public event EventHandler<ItemDragEventArgs> OnItemDiscarded;
    public void RaiseItemDiscarded(object sender, ItemDragEventArgs args) { OnItemDiscarded.Raise(sender, args); }
    public event EventHandler<StringEventArgs> OnItemChanged;
    public void RaiseItemChanged(object sender, StringEventArgs args) { OnItemChanged.Raise(sender, args); }
    public event EventHandler<TradeStateEventArgs> OnTradeStateChanged;
    public void RaiseTradeStateChanged(object sender, TradeStateEventArgs args) { OnTradeStateChanged.Raise(sender, args); }

    public event EventHandler<BookmarkChangedEventArgs> OnBookmarkChanged;
    public void RaiseBookmarkChanged(object sender, BookmarkChangedEventArgs args) { OnBookmarkChanged.Raise(sender, args); }
    public event EventHandler<Cursor3DPositionChangedEventArgs> OnCursor3DPositionChanged;
    public void RaiseCursor3DPositionChanged(object sender, Cursor3DPositionChangedEventArgs args) { OnCursor3DPositionChanged.Raise(sender, args); }

    public event EventHandler OnTutorialEvent;
    public void RaiseTutorialEvent(object sender, EventArgs args) { OnTutorialEvent.Raise(sender, args); }

    //public event SequenceRequestHandler<GameObject, object> OnDialogueRequested;
    //public void RequestDialogue(GameObject inputPhrase, SequenceRequestCallback<object> callback) { RequestSequence(inputPhrase, OnDialogueRequested, callback); }

    //public event SequenceRequestHandler<DialogueState, DialogueState> OnLinearDialogueTurnRequested;
    //public void RequestLinearDialogueTurn(DialogueState inputPhrase, SequenceRequestCallback<DialogueState> callback) { RequestSequence(inputPhrase, OnLinearDialogueTurnRequested, callback); }
    //public event SequenceRequestHandler<DialogueState, DialogueState> OnPromptDialogueTurnRequested;
    //public void RequestPromptDialogueTurn(DialogueState inputPhrase, SequenceRequestCallback<DialogueState> callback) { RequestSequence(inputPhrase, OnPromptDialogueTurnRequested, callback); }

    //public event SequenceRequestHandler<object, PhraseSequence> OnPhrasePanelRequested;
    //public void RequestPhrasePanel(object input, SequenceRequestCallback<PhraseSequence> callback) { RequestSequence(input, OnPhrasePanelRequested, callback); }
    //public event SequenceRequestHandler<PhraseSequence, PhraseSequence> OnReplaceWordPhraseEditorRequested;
    //public void RequestReplaceWordPhraseEditor(PhraseSequence input, SequenceRequestCallback<PhraseSequence> callback) { RequestSequence(input, OnReplaceWordPhraseEditorRequested, callback); }
    //public event SequenceRequestHandler<int, PhraseSequenceElement> OnWordSelectionRequested;
    //public void RequestWordSelectionRequested(int input, SequenceRequestCallback<PhraseSequenceElement> callback) { RequestSequence(input, OnWordSelectionRequested, callback); }

}
