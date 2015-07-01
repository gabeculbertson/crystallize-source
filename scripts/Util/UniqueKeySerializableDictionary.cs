using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UniqueKeySerializableDictionary<V> : SerializableDictionary<int, V> where V : ISerializableDictionaryItem<int>, ISetableKey<int>, new() {

	const int StartConstant = 0;
	const int Increment = 1;

	public int CurrentKey { get; set; }

	public UniqueKeySerializableDictionary() : base(){
		CurrentKey = StartConstant;
	}

	public int GetNextKey(){
		var key = CurrentKey;
		CurrentKey += Increment;
		return key;
	}

    public V GetOrCreateItem(int id) {
        if (ContainsKey(id)) {
            return GetItem(id);
        }
        var i = new V();
        i.SetKey(id);
        AddItem(i);
        return i;
    }

}
