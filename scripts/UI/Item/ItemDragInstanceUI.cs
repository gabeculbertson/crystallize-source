using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemDragInstanceUI : UIMonoBehaviour {

    public Image itemIconImage;

    public void Initialize(int itemID) {
        var res = ScriptableObjectDictionaries.main.itemResourceDictionary.GetOrCreateItemResources(itemID);
        itemIconImage.sprite = res.icon;
    }

}
