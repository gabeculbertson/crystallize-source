using UnityEngine;
using System.Collections;

public class PhraseSetCollectionGameData :SerializableDictionary<string, PhraseSetGameData> {

    public PhraseSetGameData GetOrCreateItem(string key) {
        var item = GetItem(key);
        if (item == null) {
            item = new PhraseSetGameData();
            item.Name = key;
            AddItem(item);
        }
        return item;
    }

}