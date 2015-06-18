using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SerializableDictionary<K, V> where V : ISerializableDictionaryItem<K> {

    bool changed = true;
    Dictionary<K, V> dictionary = new Dictionary<K, V>();

	public List<V> Items { get; set; }

	public SerializableDictionary(){
		Items = new List<V> ();
	}

	public V GetItem(K key){
        if (changed) {
            dictionary = new Dictionary<K, V>();
            foreach (var item in Items) {
                dictionary.Add(item.Key, item);
            }
            changed = false;
            //Debug.Log("Constructed dictionary: " + this);
        }

        if (dictionary.ContainsKey(key)) {
            return dictionary[key];
        }
        return default(V);
        //return (from v in Items where v.Key.Equals(key) select v).FirstOrDefault ();
	}
	
	public void RemoveItem(K key){
		var i = GetItem (key);
		if (i != null) {
			Items.Remove(i);
            changed = true;
		}
	}
	
	public void AddItem(V item){
		var i = GetItem (item.Key);
		if (i != null) {
			Debug.LogWarning("Item ID: " + item.Key + " already exists.");
			return;
		}
		Items.Add (item);
        changed = true;
	}
	
	public void UpdateItem(V item){
		RemoveItem (item.Key);
		AddItem (item);
        changed = true;
	}

}
