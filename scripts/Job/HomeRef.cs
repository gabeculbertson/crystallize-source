using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeRef : IDRef<HomeGameData, HomePlayerData> {

    public override HomeGameData GameDataInstance {
        get {
            return GameData.Instance.Homes.GetItem(ID);
        }
		set{}
    }

    public override HomePlayerData PlayerDataInstance {
        get {
            return PlayerData.Instance.Homes.GetOrCreateItem(ID);
        }
		set{}
    }

    public HomeRef(int id) : base(id) { }

}
