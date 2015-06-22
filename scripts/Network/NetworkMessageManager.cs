using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util.Serialization;
using System.Xml.Serialization;

[RequireComponent(typeof(NetworkView))]
public class NetworkMessageManager : MonoBehaviour {

    const float Delay = 0.1f;

    class TimePosition {

        public int Level { get; set; }
        public float Time { get; set; }
        public Vector3 Position { get; set; }

        public TimePosition(int level, float time, Vector3 position) {
            Level = level;
            Time = time;
            Position = position;
        }
    }

    static NetworkMessageManager _instance;

    public static bool IsConnected {
        get {
            return _instance;
        }
    }

    public static bool AllPlayersJoined {
        get {
            return IsConnected && _instance.networkPlayers.Count > 1;
        }
    }

    public GameObject player2Prefab;
    public GameObject networkResources;

    List<string> networkPlayers = new List<string>();
    Dictionary<string, TimePosition> lastPositionData = new Dictionary<string, TimePosition>();
    Dictionary<string, TimePosition> currentPositionData = new Dictionary<string, TimePosition>();

    PhraseSequence currentPhrase;

    void Awake() {
        if (_instance) {
            Destroy(gameObject);
            Debug.Log("Only one network message manager can exist!");
            return;
        }

        // TODO: this is bad
        if (Network.isServer) {
            networkPlayers.Add(Network.player.guid);
            DataLogger.LogTimestampedData("Connected", "Server");
        } else {
            DataLogger.LogTimestampedData("Connected", "Client");
        }

        _instance = this;
        DontDestroyOnLoad(this);
        CrystallizeEventManager.OnInitialized += OnEventManagerInitialized;
    }

    void Start() {
        OnEventManagerInitialized(null, System.EventArgs.Empty);
    }

    public void SetPlayer(int playerID) {
        var playerTransform = GetPlayerTransform(playerID);
        PlayerController.main.target = playerTransform;
        OmniscientCamera.main.player = playerTransform;
        PlayerManager.Instance.PlayerGameObject = playerTransform.gameObject;

        Debug.Log("Player ID: " + PlayerManager.Instance.PlayerID);
    }

    void OnEventManagerInitialized(object sender, System.EventArgs args) {
        CrystallizeEventManager.Environment.OnPersonAnimationRequested += HandleOnPersonAnimationRequested;
        CrystallizeEventManager.UI.OnTradeStateChanged += HandleTradeStateChanged;
        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
        CrystallizeEventManager.Network.OnSendQuestStateRequested += HandleSendQuestStateRequested;
        CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleQuestStateChanged;
        CrystallizeEventManager.PlayerState.OnQuestStateRequested += HandleQuestStateRequested;
        CrystallizeEventManager.Network.OnEnglishLineInput += HandleOnEnglishLineInput;
        CrystallizeEventManager.UI.OnCursor3DPositionChanged += HandleCursor3DPositionChanged;
        CrystallizeEventManager.Network.OnNetworkSpeechBubbleRequested += HandleNetworkSpeechBubbleRequested;
        CrystallizeEventManager.Network.OnNetworkPlayerFeedbackRequested += HandleNetworkPlayerFeedbackRequested;

        Instantiate(networkResources);

        Network.isMessageQueueRunning = false;
        StartCoroutine(WaitAndBeginSequence());

        Debug.Log("Network manager events initialized");
    }

    void HandleNetworkPlayerFeedbackRequested(object sender, System.EventArgs e) {
        if ((sender as NetworkMessageManager) == this) {
            return;
        }

        GetComponent<NetworkView>().RPC("SendPlayerFeedback",
                                        RPCMode.Others, Network.player.guid);
    }

    void HandleNetworkSpeechBubbleRequested(object sender, NetworkSpeechBubbleRequestedEventArgs e) {
        if (sender == this) {
            return;
        }

        GetComponent<NetworkView>().RPC("SendNetworkSpeechBubbleRequested",
                                        RPCMode.Others, Network.player.guid, Serializer.GetBytesForObject<NetworkSpeechBubbleRequestedEventArgs>(e));
    }

    void HandleOnEnglishLineInput(object sender, TextEventArgs e) {
        if (sender == this) {
            return;
        }

        if (GameSettings.GetFlag(NetworkFlags.LockEnglishText)) {
            return;
        }

        GetComponent<NetworkView>().RPC("SendChatText",
                                        RPCMode.Others, Network.player.guid, e.Text);
    }

    IEnumerator WaitAndBeginSequence() {
        yield return new WaitForSeconds(0.15f);

        var other = GameObject.FindGameObjectWithTag("OtherPlayer");
        if (!other) {
            var go = Instantiate(player2Prefab) as GameObject;

            var spawn = GameObject.FindGameObjectWithTag("PlayerOrigin");
            if (spawn) {
                go.transform.position = spawn.transform.position;
            } else {
                go.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.back + Vector3.up;
            }
        }

        if (Network.isServer) {
            SetPlayer(0);
        } else {
            SetPlayer(1);
        }

        Network.isMessageQueueRunning = true;

        CrystallizeEventManager.Network.RaiseConnectedToNetwork(this, System.EventArgs.Empty);
    }

    void HandleQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
        GetComponent<NetworkView>().RPC("SendQuestState", RPCMode.Others, Network.player.guid,
                                        Serializer.GetBytesForObject<QuestStateChangedEventArgs>(e));
    }

    void HandleQuestStateRequested(object sender, QuestEventArgs e) {
        GetComponent<NetworkView>().RPC("SendQuestStateRequest", RPCMode.Others, Network.player.guid, e.QuestID);
        Debug.Log("Requesting quest from others");
    }

    void BroadcastQuestState(int questID) {
        var q = PlayerData.Instance.QuestData.GetOrCreateQuestInstance(questID);
        HandleQuestStateChanged(this, new QuestStateChangedEventArgs(PlayerManager.Instance.PlayerID, q));
    }

    void HandleSendQuestStateRequested(object sender, PartnerObjectiveCompleteEventArgs e) {
        if (sender == this) {
            return;
        }

        GetComponent<NetworkView>().RPC("SendObjectiveState", RPCMode.Others, Network.player.guid,
                                        Serializer.GetBytesForObject<PartnerObjectiveCompleteEventArgs>(e));
    }

    void HandleCursor3DPositionChanged(object sender, Cursor3DPositionChangedEventArgs e) {
        //Debug.Log("cursor moved: " + e.PlayerID + "; " + (Vector3)e.Position);
        if (sender == this) {
            return;
        }

        GetComponent<NetworkView>().RPC("SendCursorPosition", RPCMode.Others, Network.player.guid, Serializer.GetBytesForObject<Cursor3DPositionChangedEventArgs>(e));
    }

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is SpeechPanelUIRequestEventArgs) {
            PlayerManager.Instance.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(null);
        }
    }

    void HandleOnPersonAnimationRequested(object sender, PersonAnimationEventArgs e) {
        if (sender == this) {
            return;
        }

        if (GameSettings.GetFlag(NetworkFlags.LockAnimation)) {
            return;
        }

        // TODO: make this more robust
        if (e.TargetObject != PlayerManager.Instance.PlayerGameObject) {
            return;
        }

        GetComponent<NetworkView>().RPC("UpdateAnimation", RPCMode.Others, Network.player.guid, (int)e.AnimationType);
    }

    void HandleTradeStateChanged(object sender, TradeStateEventArgs e) {
        if (sender == this) {
            return;
        }

        GetComponent<NetworkView>().RPC("UpdateInventory", RPCMode.Others, Network.player.guid, Serializer.GetBytesForObject<TradeState>(e.State));
    }

    void Update() {
        if (!Network.isClient && !Network.isServer) {
            Debug.Log("Not connected!");
            return;
        }

        if (!Network.isMessageQueueRunning) {
            return;
        }

        // TODO: for debug purposes only
        //if (Input.GetKeyDown(KeyCode.Tab)) {
        //    if (PlayerManager.main.PlayerID == 0) {
        //        SetPlayer(1);
        //    } else {
        //        SetPlayer(0);
        //    }
        //    // need to fake a connection in order to get related objects to change
        //    CrystallizeEventManager.main.RaiseConnectedToNetwork(this, System.EventArgs.Empty);
        //}

        foreach (var guid in networkPlayers) {
            if (guid == Network.player.guid) {
                continue;
            }

            //GetPlayerTransform(guid).gameObject.SetActive(true);
            UpdateForward(guid);
            GetPlayerTransform(guid).position = GetPosition(guid, Time.time);
            if (currentPositionData.ContainsKey(guid)) {
                PlayerManager.Instance.OtherPlayerLevelID = currentPositionData[guid].Level;
                if (currentPositionData[guid].Level != Application.loadedLevel) {
                    GetPlayerTransform(guid).position = new Vector3(0, -10000f, 0);
                }
            }
        }

        // TODO: get rid of this
        if (networkPlayers.Count == 1 || LevelSettings.main.hidePartner) {
            PlayerManager.Instance.OtherGameObject.transform.position = new Vector3(0, -10000f, 0);
        }

        var da = GetPlayerTransform(Network.player.guid).GetComponent<DialogueActor>();
        if (da.CurrentPhrase != currentPhrase) {
            CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new PartnerSaidPhraseEventArgs(Network.player.guid, da.CurrentPhrase));
            GetComponent<NetworkView>().RPC("UpdateSpeech", RPCMode.Others, Network.player.guid, GetXml(da.CurrentPhrase));
            currentPhrase = da.CurrentPhrase;
        }

        Vector3 p = GetPlayerTransform(Network.player.guid).position;
        GetComponent<NetworkView>().RPC("UpdatePlayerMovement", RPCMode.Others, Network.player.guid, Application.loadedLevel, p);
    }

    string GetXml(PhraseSequence phrase) {
        return Serializer.SaveToXmlString<PhraseSequence>(phrase);
    }

    void OnPlayerConnected(NetworkPlayer player) {
        Debug.Log("Player connected: " + player.guid);
        networkPlayers.Add(player.guid);
        SendNetworkPlayers();
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("Player disconnected: " + player.guid);
        networkPlayers.Remove(player.guid);
        SendNetworkPlayers();
        //Destroy(gameObject);
    }

    void SendNetworkPlayers() {
        var bytes = Serializer.GetBytesForObject<List<string>>(networkPlayers);
        GetComponent<NetworkView>().RPC("SetNetworkPlayers", RPCMode.All, bytes);
    }

    Vector3 GetPosition(string guid, float time) {
        time -= Delay;
        if (currentPositionData.ContainsKey(guid) && lastPositionData.ContainsKey(guid)) {
            return Vector3.Lerp(lastPositionData[guid].Position, currentPositionData[guid].Position,
                (time - lastPositionData[guid].Time) / (currentPositionData[guid].Time - lastPositionData[guid].Time));
        }
        return Vector3.zero;
    }

    void UpdateForward(string guid) {
        if (currentPositionData.ContainsKey(guid) && lastPositionData.ContainsKey(guid)) {
            var f = currentPositionData[guid].Position - lastPositionData[guid].Position;
            f.y = 0;
            if (f.sqrMagnitude > 0.001f) {
                GetPlayerTransform(guid).forward = f;
            }
        }
    }

    [RPC]
    void UpdatePlayerMovement(string playerGuid, int level, Vector3 position) {
        if (currentPositionData.ContainsKey(playerGuid)) {
            lastPositionData[playerGuid] = currentPositionData[playerGuid];
        }
        currentPositionData[playerGuid] = new TimePosition(level, Time.time, position);
    }

    [RPC]
    void UpdateSpeech(string playerGuid, string phraseXml) {
        var pObj = Serializer.LoadFromXmlString<PhraseSequence>(phraseXml);
        GetPlayerTransform(playerGuid).GetComponent<DialogueActor>().SetPhrase(pObj);
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new PartnerSaidPhraseEventArgs(playerGuid, pObj));
    }

    [RPC]
    void UpdateAnimation(string playerGuid, int animationType) {
        var at = (PersonAnimationType)animationType;
        var obj = GetPlayerTransform(playerGuid);
        CrystallizeEventManager.Environment.RaisePersonAnimationRequested(this, new PersonAnimationEventArgs(obj.gameObject, at));
    }

    [RPC]
    void UpdateInventory(string playerGuid, string bytes) {
        var state = Serializer.GetObjectForBytes<TradeState>(bytes);
        CrystallizeEventManager.UI.RaiseTradeStateChanged(this, new TradeStateEventArgs(state));
    }

    [RPC]
    void SendObjectiveState(string playerGuid, string bytes) {
        var state = Serializer.GetObjectForBytes<PartnerObjectiveCompleteEventArgs>(bytes);
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, state);
    }

    [RPC]
    void SendQuestState(string playerGuid, string bytes) {
        var state = Serializer.GetObjectForBytes<QuestStateChangedEventArgs>(bytes);
        //Debug.Log("Got state for quest: " + state.QuestID + "; " + state.QuestState);
        //Debug.Log("Got quest state: " + state.PlayerID + "; " + state.GetQuestInstance().ObjectiveStates.Count);
        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, state);
    }

    [RPC]
    void SendCursorPosition(string playerGuid, string bytes) {
        var args = Serializer.GetObjectForBytes<Cursor3DPositionChangedEventArgs>(bytes);
        CrystallizeEventManager.UI.RaiseCursor3DPositionChanged(this, args);
    }

    [RPC]
    void SendQuestStateRequest(string playerGuid, int questID) {
        BroadcastQuestState(questID);
        //Debug.Log("Requested quest: " + questID);
    }

    [RPC]
    void SendChatText(string playerGuid, string text) {
        CrystallizeEventManager.Network.RaiseEnglishLineInput(this, new TextEventArgs(text));
    }

    [RPC]
    void SendNetworkSpeechBubbleRequested(string playerGuid, string data) {
        var e = Serializer.GetObjectForBytes<NetworkSpeechBubbleRequestedEventArgs>(data);
        CrystallizeEventManager.UI.RaiseSpeechBubbleRequested(this,
            new SpeechBubbleRequestedEventArgs(e.GetTarget(), e.GetPhraseSequence(), false, false, false));
    }

    [RPC]
    void SendPlayerFeedback(string playerGuid) {
        EffectManager.main.PlayPositiveFeedback();
    }

    [RPC]
    void SetNetworkPlayers(string bytes) {
        networkPlayers = Serializer.GetObjectForBytes<List<string>>(bytes);
        Debug.Log("Have players: " + networkPlayers.Count);
        PlayerManager.Instance.PlayerCount = networkPlayers.Count;
    }

    public Transform GetPlayerTransform(string guid) {
        var index = networkPlayers.IndexOf(guid);
        if (index == 0) {
            return GameObject.FindGameObjectWithTag("Player").transform;
        } else {
            return GameObject.FindGameObjectWithTag("OtherPlayer").transform;
        }
    }

    public Transform GetPlayerTransform(int id) {
        if (id == 0) {
            return GameObject.FindGameObjectWithTag("Player").transform;
        } else {
            return GameObject.FindGameObjectWithTag("OtherPlayer").transform;
        }
    }



}
