using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ContextData :ISerializableDictionaryItem<int> {

	static string[] personContextIDs = new string[]{ "Name", "Occupation", "Target" };

	public static ContextData GetPersonContextData(int worldID){
		return new ContextData (worldID, personContextIDs);
	}
	
	public int Key {
		get {
			return WorldID;
        }
        set {
            WorldID = value;
        }
	}

	public int WorldID { get; set; }
	public List<ContextDataElement> Elements { get; set; }

	public ContextData(){
		WorldID = -1;
		Elements = new List<ContextDataElement> ();
	}

	public ContextData(int worldID) : this(){
		WorldID = worldID;
	}

	ContextData(int worldID, IEnumerable<string> contextIDs) : this(worldID){
		WorldID = worldID;
		foreach (var e in contextIDs) {
			Elements.Add(new ContextDataElement(e));
			//UpdateElement(e, "NULL");
		}
	}

	public ContextDataElement GetElement(string id){
		return (from e in Elements where e.Name.ToLower() == id.ToLower() select e).FirstOrDefault ();
	}

	public void UpdateElement(string id, PhraseSequence value){
		var e = GetElement (id);
		if (e != null) {
			e.Data = value;
		} else {
			e = new ContextDataElement(id);
			e.Data = value;
			Elements.Add (e);
		}
	}

}
