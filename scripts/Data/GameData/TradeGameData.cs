﻿using UnityEngine;
using System.Collections;

public class TradeGameData  {

    public UniqueKeySerializableDictionary<ItemGameData> Items { get; set; }

    public TradeGameData() {
        Items = new UniqueKeySerializableDictionary<ItemGameData>();
    }

    public void AddNewItem() {
        Items.AddItem(new ItemGameData(Items.GetNextKey()));
    }

}
