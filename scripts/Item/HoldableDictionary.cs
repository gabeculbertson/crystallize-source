using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HoldableDictionary : ScriptableObject {

	[System.Serializable]
	public class HoldableData {
		public string id;
		public GameObject prefab;

		public HoldableData(string id, GameObject prefab){
			this.id = id;
			this.prefab = prefab;
		}
	}

	public List<HoldableData> holdables = new List<HoldableData>();

	public HoldableData GetHoldable(string id){
		return (from h in holdables where h.id == id select h).FirstOrDefault();
	}

	public IEnumerable<HoldableData> GetAllHoldables(){
		return holdables;
	}

}
