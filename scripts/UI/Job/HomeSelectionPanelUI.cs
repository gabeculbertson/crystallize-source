﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeSelectionPanelUI : UIPanel, ITemporaryUI<object, HomeRef> {

    const string ResourcePath = "UI/HomeSelectionPanel";
    public static HomeSelectionPanelUI GetInstance() {
        return GameObjectUtil.GetResourceInstance<HomeSelectionPanelUI>(ResourcePath);
    }

    public GameObject buttonPrefab;
    public RectTransform buttonParent;

    List<GameObject> instances = new List<GameObject>();

    public event EventHandler<EventArgs<HomeRef>> Complete;

    public void Initialize(object param1) {
        transform.SetParent(MainCanvas.main.transform, false);
        CrystallizeEventManager.PlayerState.OnHomesChanged += PlayerState_OnHomesChanged;
        CrystallizeEventManager.PlayerState.OnMoneyChanged += PlayerState_OnHomesChanged;
        Refresh();
    }

    void OnDestroy() {
        CrystallizeEventManager.PlayerState.OnHomesChanged -= PlayerState_OnHomesChanged;
        CrystallizeEventManager.PlayerState.OnMoneyChanged -= PlayerState_OnHomesChanged;
    }

    void PlayerState_OnHomesChanged(object sender, EventArgs e) {
        Refresh();
    }

    void Refresh() {
        var children = new List<HomeRef>();
        foreach (var h in GameData.Instance.Homes.Items) {
            var hr = new HomeRef(h.ID);
            if (hr.PlayerDataInstance.Unlocked) {
                children.Add(hr);
            }
        }
        UIUtil.GenerateChildren(children, instances, buttonParent, GetChild);
    }

    GameObject GetChild(HomeRef home) {
        var i = Instantiate<GameObject>(buttonPrefab);
        var cost = home.GameDataInstance.DailyCost;
        i.GetComponentInChildren<Text>().text = string.Format("{0} ({1} ¥)", home.GameDataInstance.Name, cost);
        if (cost > PlayerData.Instance.Money) {
            i.GetComponent<Image>().color = Color.gray;
        } else {
            i.AddComponent<DataContainer>().Store(home);
            i.GetComponent<UIButton>().OnClicked += HomeSelectionPanelUI_OnClicked;
        }
        return i;
    }

    void HomeSelectionPanelUI_OnClicked(object sender, EventArgs e) {
        var h = ((Component)sender).GetComponent<DataContainer>().Retrieve<HomeRef>();
        PlayerDataConnector.AddMoney(-h.GameDataInstance.DailyCost);
        RaiseComplete(h);
    }

    void RaiseComplete(HomeRef home) {
        Complete.Raise(this, new EventArgs<HomeRef>(home));
        Close();
    }

}
