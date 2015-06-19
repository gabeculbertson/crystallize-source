using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemResourceDictionary : ScriptableObject {

	public List<ItemResources> itemResources = new List<ItemResources>();

    public ItemResources GetItemResources(int itemID) {
        return (from ir in itemResources where ir.itemID == itemID select ir).FirstOrDefault();
    }

    public ItemResources GetOrCreateItemResources(int itemID) {
        var r = GetItemResources(itemID);
        if (r == null) {
            r = new ItemResources(itemID);
            itemResources.Add(r);
        }
        return r;
    }

    public void SetItemSprite(int itemID, Sprite sprite) {
        var r = GetOrCreateItemResources(itemID);
        r.icon = sprite;
    }

}
