using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkMenu : MonoBehaviour {

    public GameObject clientServerMenu;
    public GameObject serverMenu;
    public GameObject clientMenu;
    public GameObject startingText;
    public GameObject buttonPrefab;
    public RectTransform hostListParent;
    public NetworkInitializer networkInitializer;

    IEnumerable<GameObject> AllMenus {
        get {
            return new GameObject[] { clientServerMenu, serverMenu, clientMenu, startingText };
        }
    }

    List<GameObject> buttonInstances = new List<GameObject>();

    void Start() {
        networkInitializer.OnHostListReceived += HandleHostListReceived;
        networkInitializer.OnPlayerConnectedEvent += HandlePlayerConnectedEvent;
    }

    void HandlePlayerConnectedEvent(object sender, System.EventArgs e) {
        WaitForBegin();
        Begin();
    }

    void HandleHostListReceived(object sender, System.EventArgs e) {
        foreach (var i in buttonInstances) {
            Destroy(i);
        }
        buttonInstances.Clear();

        Debug.Log("Got hosts");
        var count = networkInitializer.hostList.Length;
        for(int index = 0; index < count; index++){
            var i = Instantiate(buttonPrefab) as GameObject;
            i.transform.SetParent(hostListParent);
            var c = index;
            i.GetComponent<Button>().onClick.AddListener(() => ChooseHost(c));
            i.GetComponentInChildren<Text>().text = "Game" + index;
        }
    }

    void HideAll() {
        foreach (var go in AllMenus) {
            go.SetActive(false);
        }
    }

    public void ChooseServer() {
        //HideAll();
        //serverMenu.SetActive(true);
        networkInitializer.StartServer();
        WaitForBegin();
        //Begin();
    }

    public void ChooseClient() {
        HideAll();
        clientMenu.SetActive(true);
        RefreshHostList();
    }

    public void WaitForBegin() {
        HideAll();
        startingText.SetActive(true);
    }

    public void Begin() {
        if (PlayerData.Instance.Location.AreaID == LocationPlayerData.DefaultAreaID) {
            PlayerData.Instance.Location.AreaID = AreaManager.GetFirstAreaID();
        }
        var aid = PlayerData.Instance.Location.AreaID;
        var area = GameData.Instance.NavigationData.Areas.GetItem(aid);
        AreaManager.TransitionToArea(area);
    }

    public void Cancel() {
        HideAll();
        clientServerMenu.SetActive(true);
    }

    public void ChooseHost(int index) {
        networkInitializer.JoinServer(index);
        WaitForBegin();
    }

    public void RefreshHostList() {
        networkInitializer.RefreshHostList();
    }

}
