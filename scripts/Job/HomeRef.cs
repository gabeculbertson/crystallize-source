using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeRef : IDRef<HomeGameData, HomePlayerData> {

    public override HomeGameData GameDataInstance {
        get {
            return GameData.Instance.Homes.GetItem(ID);
        }
    }

    public override HomePlayerData PlayerDataInstance {
        get {
            var pd = PlayerData.Instance.Homes.GetItem(ID);
            if (pd == null) {
                Debug.Log("no player data");
                pd = new HomePlayerData(ID);
                PlayerData.Instance.Homes.UpdateItem(pd);
                pd = PlayerData.Instance.Homes.GetItem(ID);
            }
            return pd;
        }
    }

    public HomeRef(int id) : base(id) { }

}
