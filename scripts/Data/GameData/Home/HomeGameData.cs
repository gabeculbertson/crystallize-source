using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class HomeGameData : ISerializableDictionaryItem<int>, IHasID {

    public string Name { get; set; }
    public string AreaName { get; set; }
    public int DailyCost { get; set; }
    public int InitialCost { get; set; }
    public float Quality { get; set; }
    public int ID { get; set; }

    public int Key {
        get { return ID; }
    }

    public HomeGameData() {
        Name = "";
        AreaName = "";
        DailyCost = 10000;
        InitialCost = 100000;
        Quality = 1f;
        ID = -1;
    }

}
