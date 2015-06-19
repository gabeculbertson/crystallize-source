using UnityEngine;
using System;
using System.Collections;

public class AutomaticNetworkInitializer : MonoBehaviour {

    public const string TypeName = "CrystallizeMultiplayer";
    public const string GameName = "CrystallizeLevel";
    public const int PortNumber = 25002;

    public HostData[] hostList;

    public GameObject networkMessagerPrefab;

    bool hostListReceived = false;

    IEnumerator Start() {
        yield return null;

        RefreshHostList();
        while (!hostListReceived) {
            yield return null;
        }

        if (hostList.Length == 0) {
            StartServer();
        } else {
            JoinServer(hostList[0]);
        }
    }

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
            hostListReceived = true;
        }
    }

    void JoinServer(HostData hostData) {
        Network.Connect(hostData);
    }

    void OnServerInitialized() {
        Begin();
    }

    void OnConnectedToServer() {
        Begin();
    }

    public void Begin() {
        Instantiate(networkMessagerPrefab);

        if (PlayerData.Instance.Location.AreaID == LocationPlayerData.DefaultAreaID) {
            PlayerData.Instance.Location.AreaID = AreaManager.GetFirstAreaID();
        }
        var aid = PlayerData.Instance.Location.AreaID;
        var area = GameData.Instance.NavigationData.Areas.GetItem(aid);
        AreaManager.TransitionToArea(area);
    }

}
