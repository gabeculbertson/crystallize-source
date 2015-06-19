using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemResources {

    public int itemID;
    public Sprite icon;

    public ItemResources(int itemID) {
        this.itemID = itemID;
    }

}
