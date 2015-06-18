using UnityEngine;
using System;
using System.Collections;

public partial class CrystallizeEventManager : MonoBehaviour {

	static bool quitting = false;
	static CrystallizeEventManager _main;

	public static CrystallizeEventManager main { 
		get {
			if(!_main && !quitting){
				UnityEngine.Debug.Log("Producing new event manager.");
				var go = new GameObject("CrystallizeEventSystem");
				_main = go.AddComponent<CrystallizeEventManager>();

                if (OnInitialized != null) {
                    OnInitialized(_main, EventArgs.Empty);
                }
			}
			return _main;
		}
	}

    public static PlayerStateEvents PlayerState { get { return main.playerState; } }
    public static EnvironmentEvents Environment { get { return main.environment; } }
    public static UIEvents UI { get { return main.ui; } }
    public static NetworkEvents Network { get { return main.network; } }
    public static DebugEvents Debug { get { return main.debug; } }

    public static EventHandler OnInitialized;
    public static EventHandler OnLoadComplete;
    public static EventHandler OnQuit;

    public event EventHandler<SpeechBubbleRequestedEventArgs> OnSpeechBubbleRequested;
    public void RaiseSpeechBubbleRequested(object sender, SpeechBubbleRequestedEventArgs args) { OnSpeechBubbleRequested.Raise(sender, args); }
	public event EventHandler<PhraseEventArgs> OnSpeechBubbleOpen;
    public void RaiseSpeechBubbleOpen(object sender, PhraseEventArgs args) { OnSpeechBubbleOpen(sender, args); }

    public event EventHandler<WordClickedEventArgs> OnWordClicked;
    public void RaiseWordClicked(object sender, WordClickedEventArgs args) { OnWordClicked(sender, args); }
	public event EventHandler<PhraseEventArgs> OnWordFound;
    public void RaiseOnWordFound(object sender, PhraseEventArgs args) { OnWordFound.Raise(sender, args); }

	public event EventHandler OnInteractiveDialogueOpened;
    public void RaiseInteractiveDialogueOpened(object sender, EventArgs args) { OnInteractiveDialogueOpened.Raise(sender, args); }
	public event EventHandler OnInteractiveDialogueClosed;
    public void RaiseInteractiveDialogueClosed(object sender, EventArgs args) { OnInteractiveDialogueClosed.Raise(sender, args); }

	public event EventHandler OnActorApproached;
    public void RaiseActorApproached(object sender, EventArgs args) { OnActorApproached.Raise(sender, args); }
	public event EventHandler OnActorDeparted;
    public void RaiseActorDeparted(object sender, EventArgs args) { OnActorDeparted.Raise(sender, args); }

	public event EventHandler<TextEventArgs> OnFlagChanged;
    public void RaiseFlagChanged(object sender, TextEventArgs args) { OnFlagChanged.Raise(sender, args); }

	public event EventHandler OnGameEvent;
    public void RaiseGameEvent(object sender, EventArgs args) { OnGameEvent.Raise(sender, args); }

	public event EventHandler<QuestStateChangedEventArgs> OnQuestStateChanged;
    public void RaiseQuestStateChanged(object sender, QuestStateChangedEventArgs args) { OnQuestStateChanged.Raise(sender, args); }
    public event EventHandler<QuestEventArgs> OnQuestStateRequested;
    public void RaiseQuestStateRequested(object sender, QuestEventArgs args) { OnQuestStateRequested.Raise(sender, args); }
	
    // replace this with a meta-event
	public event EventHandler OnUpdateUI;
    public void RaiseUpdateUI(object sender, EventArgs args) { OnUpdateUI.Raise(sender, args); }
	public event EventHandler OnProgressEvent;
    public void RaiseOnProgressEvent(object sender, EventArgs args) { OnProgressEvent.Raise(sender, args); }

	public event EventHandler<QuestEventArgs> OnActiveQuestChanged;
    public void RaiseActiveQuestChanged(object sender, QuestEventArgs args) { OnActiveQuestChanged.Raise(sender, args); }

	public event EventHandler<UIRequestEventArgs> OnUIRequested;
    public void RaiseUIRequest(object sender, UIRequestEventArgs args) { OnUIRequested.Raise(sender, args); }
	public event EventHandler OnUIInteraction;
    public void RaiseUIInteraction(object sender, EventArgs args) { OnUIInteraction.Raise(sender, args); }

    public event EventHandler<UIModeChangedEventArgs> OnUIModeRequested;
    public void RaiseUIModeRequested(object sender, UIModeChangedEventArgs args) { OnUIModeRequested.Raise(sender, args); }
    public event EventHandler<UIModeChangedEventArgs> OnUIModeChanged;
    public void RaiseUIModeChanged(object sender, UIModeChangedEventArgs args) { OnUIModeChanged.Raise(sender, args); }

    public event EventHandler<DialogueSequenceEventArgs> OnPromptLinearDialogueContinue;
    public void RaisePromptLinearDialogueContinue(object sender, DialogueSequenceEventArgs args) { OnPromptLinearDialogueContinue.Raise(sender, args); }
    public event EventHandler<DialogueSequenceEventArgs> OnPromptPromptDialogueContinue;
    public void RaisePromptPromptDialogueContinue(object sender, DialogueSequenceEventArgs args) { OnPromptPromptDialogueContinue.Raise(sender, args); }
    public event EventHandler<DialogueSequenceEventArgs> OnPromptEndDialogueContinue;
    public void RaisePromptEndDialogueContinue(object sender, DialogueSequenceEventArgs args) { OnPromptEndDialogueContinue.Raise(sender, args); }
    public event EventHandler<DialogueSequenceEventArgs> OnResolveLinearDialogueContinue;
    public void RaiseResolveLinearDialogueContinue(object sender, DialogueSequenceEventArgs args) { OnResolveLinearDialogueContinue.Raise(sender, args); }
    public event EventHandler<DialogueSequenceEventArgs> OnResolvePromptDialogueContinue;
    public void RaiseResolvePromptDialogueContinue(object sender, DialogueSequenceEventArgs args) { OnResolvePromptDialogueContinue.Raise(sender, args); }
    public event EventHandler<DialogueSequenceEventArgs> OnResolveEndDialogueContinue;
    public void RaiseResolveEndDialogueContinue(object sender, DialogueSequenceEventArgs args) { OnResolveEndDialogueContinue.Raise(sender, args); }

	public event EventHandler BeforeCameraMove;
    public void RaiseBeforeCameraMove(object sender, EventArgs args) { BeforeCameraMove.Raise(sender, args); }
	public event EventHandler AfterCameraMove;
    public void RaiseAfterCameraMove(object sender, EventArgs args) { AfterCameraMove.Raise(sender, args); }

    public event EventHandler OnMoneyChanged;
    public void RaiseMoneyChanged(object sender, EventArgs args) { OnMoneyChanged.Raise(sender, args); }
    public event EventHandler OnAreaUnlocked;
    public void RaiseAreaUnlocked(object sender, EventArgs args) { OnAreaUnlocked.Raise(sender, args); }

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

	// Scene events
	public event EventHandler BeforeSceneChange;
    public void RaiseBeforeSceneChange(object sender, EventArgs args) { BeforeSceneChange.Raise(sender, args); }

    // Tutorial
    public event EventHandler OnTutorialEvent;
    public void RaiseTutorialEvent(object sender, EventArgs args) { OnTutorialEvent.Raise(sender, args); }

    // Network stuff
    public event EventHandler OnConnectedToNetwork;
    public void RaiseConnectedToNetwork(object sender, System.EventArgs args) { OnConnectedToNetwork.Raise(sender, args); }
    public event EventHandler<NetworkSpeechBubbleRequestedEventArgs> OnNetworkSpeechBubbleRequested;
    public void RaiseNetworkSpeechBubbleRequested(object sender, NetworkSpeechBubbleRequestedEventArgs args) { OnNetworkSpeechBubbleRequested.Raise(sender, args); }
    public event EventHandler OnNetworkPlayerFeedbackRequested;
    public void RaiseNetworkPlayerFeedbackRequested(object sender, EventArgs args) { OnNetworkPlayerFeedbackRequested.Raise(sender, args); }
    public event EventHandler<PartnerObjectiveCompleteEventArgs> OnSendQuestStateRequested;
    public void RaiseSendQuestStateRequested(object sender, PartnerObjectiveCompleteEventArgs args) { OnSendQuestStateRequested.Raise(sender, args); }
	public event EventHandler<TextEventArgs> OnEnglishLineInput;
    public void RaiseEnglishLineInput(object sender, TextEventArgs args) { OnEnglishLineInput.Raise(sender, args); }

    public event EventHandler<PersonAnimationEventArgs> OnPersonAnimationRequested;
    public void RaisePersonAnimationRequested(object sender, PersonAnimationEventArgs args) { OnPersonAnimationRequested.Raise(sender, args); }

    // trying a new pattern here, may want to go back and change this later
    public event EventHandler OnLoad;
    public void RaiseLoad(object sender, EventArgs args) { OnLoad.Raise(sender, args); }
    public event EventHandler OnSave;
    public void RaiseSave(object sender, EventArgs args) { OnSave.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnBasePhraseSelected;
    public void RaiseBasePhraseSelected(object sender, PhraseEventArgs args) { OnBasePhraseSelected.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnModifiedPhraseSelected;
    public void RaiseModifiedPhraseSelected(object sender, PhraseEventArgs args) { OnModifiedPhraseSelected.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnAttemptCollectPhrase;
    public void RaiseAttemptCollectPhrase(object sender, PhraseEventArgs args) { OnAttemptCollectPhrase.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnSucceedCollectPhrase;
    public void RaiseSucceedCollectPhrase(object sender, PhraseEventArgs args) { OnSucceedCollectPhrase.Raise(sender, args); }

    PlayerStateEvents playerState;
    EnvironmentEvents environment;
    UIEvents ui;
    NetworkEvents network;
    DebugEvents debug;

    void Awake() {
        playerState = new PlayerStateEvents();
        environment = new EnvironmentEvents();
        ui = new UIEvents();
        network = new NetworkEvents();
        debug = new DebugEvents();
    }

	void OnApplicationQuit(){
		quitting = true;

        if (OnQuit != null) {
            OnQuit(this, EventArgs.Empty);
            OnQuit = null;
        }
	}

    IEnumerator Start()
    {
        yield return null;

        if (OnLoadComplete != null)
        {
            OnLoadComplete(this, EventArgs.Empty);
        }
    }

}
