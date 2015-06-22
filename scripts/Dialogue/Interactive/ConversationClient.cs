using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationClient : MonoBehaviour {

    ConversationClientState state;

    public ConversationClientData clientData;
    public List<PhraseSegmentData> missingWords = new List<PhraseSegmentData>();
    //public int clientID = -1;

    public bool IsConversationOpen {
        get;
        private set;
    }

    public ConversationClientState State {
        get {
            return state;
        }
    }

    public event EventHandler OnStateChanged;

    // Use this for initialization
    void Start() {
        GetComponent<InteractiveDialogActor>().OnOpenDialog += HandleOnOpenDialog;
        GetComponent<InteractiveDialogActor>().OnDialogueSuccess += HandleOnUnlock;

        ObjectiveManager.main.SetObjective(this, false);

        PlayerManager.Instance.OnLevelChanged += HandleOnStateChanged;
        InteractiveDialogManager.main.OnDialogSuccess += HandleOnStateChanged;

        CrystallizeEventManager.PlayerState.OnWordFound += HandleOnStateChanged;
        CrystallizeEventManager.UI.OnInteractiveDialogueOpened += HandleOnStateChanged;
        CrystallizeEventManager.UI.OnInteractiveDialogueClosed += HandleOnStateChanged;
        CrystallizeEventManager.UI.OnProgressEvent += HandleOnStateChanged;
    }

    void Update() {
        GetClientState();
    }

    void OnDisable() {
        PlayerManager.Instance.OnLevelChanged -= HandleOnStateChanged;
        InteractiveDialogManager.main.OnDialogSuccess -= HandleOnStateChanged;

        CrystallizeEventManager.PlayerState.OnWordFound -= HandleOnStateChanged;
        CrystallizeEventManager.UI.OnInteractiveDialogueOpened -= HandleOnStateChanged;
        CrystallizeEventManager.UI.OnInteractiveDialogueClosed -= HandleOnStateChanged;
        CrystallizeEventManager.UI.OnProgressEvent -= HandleOnStateChanged;
    }

    void HandleOnOpenDialog(object sender, PhraseEventArgs e) {
        PlayerDialogActor.main.currentClient = this;
    }

    void HandleOnUnlock(object sender, PhraseEventArgs e) {
        PlayerData.Instance.FriendData.SetFriendState(clientData.ID, 1);

        ObjectiveManager.main.SetObjective(this, true);
    }

    public int GetRemainingWordCount() {
        int count = 0;
        foreach (var word in GetObjectiveWords()) {
            if (!ObjectiveManager.main.IsWordFound(word)) {
                count++;
            }
        }
        return count;
    }

    public List<PhraseSegmentData> GetObjectiveWords() {
        if (missingWords.Count > 0) {
            return missingWords;
        } else {
            return clientData.GetMissingWords();
        }
    }

    public List<PhraseSegmentData> GetPlayerSuppliedWords() {
        var words = new List<PhraseSegmentData>();
        if (missingWords.Count > 0) {
            foreach (var word in missingWords) {
                words.Add(word);
            }
            return words;
        } else {
            return clientData.GetAllWords();
        }
    }

    void HandleOnStateChanged(object sender, EventArgs args) {
        GetClientState();
    }

    void GetClientState() {
        var a = GetComponent<InteractiveDialogActor>();
        var lastState = state;
        if (a.IsComplete) {
            state = ConversationClientState.Completed;
        } else if (PlayerData.Instance.InventoryState.Level < a.minimumLevel) {
            state = ConversationClientState.Locked;
        } else if (GetRemainingWordCount() == 0) {
            state = ConversationClientState.Available;
        } else if (!a.HasBeenVisited) {
            state = ConversationClientState.SeekingClient;
        } else {
            state = ConversationClientState.SeekingWords;
        }

        if (state != lastState || IsConversationOpen != a.IsOpen) {
            if (OnStateChanged != null) {
                OnStateChanged(this, EventArgs.Empty);
            }
        }
        IsConversationOpen = a.IsOpen;
    }

}
