using UnityEngine;
using UnityEditor;
using System.Collections;

public class DictionaryDataEditor : UnityEditor.AssetModificationProcessor {
		
	static string[] OnWillSaveAssets(string[] assets){
		DictionaryData.SaveInstance ();
		
		return assets;
	}

}
