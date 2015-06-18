using UnityEngine;
using System;
using System.Collections;

public class NetworkInitializer : MonoBehaviour {

    public const string TypeName = "CrystallizeMultiplayer";
    public const string GameName = "CrystallizeLevel";
    public const int PortNumber = 25002;

    public HostData[] hostList;

    public GameObject networkMessagerPrefab;

    public event EventHandler OnHostListReceived;
    public event EventHandler OnPlayerConnectedEvent;

    public void StartServer() {
        Network.InitializeServer(4, PortNumber, !Network.HavePublicAddress());
        MasterServer.RegisterHost(TypeName, GameName);
    }

    public void RefreshHostList() {
        MasterServer.RequestHostList(TypeName);
    }

    public void JoinServer(int hostIndex) {
        if (hostIndex >= 0 && hostIndex < hostList.Length) {
            Debug.Log("Joining server...");
            JoinServer(hostList[hostIndex]);
        } else {
            Debug.Log("Index out of range:" + hostIndex);
        }
    }

    void OnMasterServerEvent(MasterServerEvent msEvent) {
        if (msEvent == MasterServerEvent.HostListReceived) {
            hostList = MasterServer.PollHostList();
            if (OnHostListReceived != null) {
                OnHostListReceived(this, EventArgs.Empty);
            }
        }
    }

    void JoinServer(HostData hostData) {
        Network.Connect(hostData);
    }

    void OnServerInitialized() {
        Debug.Log("Server initialized.");

        Instantiate(networkMessagerPrefab);

        if (OnPlayerConnectedEvent != null) {
            OnPlayerConnectedEvent(this, EventArgs.Empty);
        }

        Destroy(gameObject);
    }

    void OnConnectedToServer() {
        Instantiate(networkMessagerPrefab);
        if (OnPlayerConnectedEvent != null) {
            OnPlayerConnectedEvent(this, EventArgs.Empty);
        }
        Debug.Log("Connected to server.");
    }

}
