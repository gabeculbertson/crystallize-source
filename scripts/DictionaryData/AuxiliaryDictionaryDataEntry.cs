using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuxiliaryDictionaryDataEntry {

	public int WordID { get; set; }
    public string PreferredTranslation { get; set; }
    public int ItemID { get; set; }
	public List<int> TagIDs { get; set; }

	public AuxiliaryDictionaryDataEntry () {
		TagIDs = new List<int> ();
	}

	public AuxiliaryDictionaryDataEntry(int wordID) : this() {
		WordID = wordID;
	}

	public void AddTag(int tagID){
		if (!TagIDs.Contains (tagID)) {
			TagIDs.Add (tagID);
		}
	}

}
