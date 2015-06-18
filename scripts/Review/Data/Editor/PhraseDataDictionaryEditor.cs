using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(PhraseDictionaryData))]
public class PhraseDataDictionaryEditor : Editor {

	PhraseDictionaryData dictionary;

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		if(GUILayout.Button("Fill")){
			FillDictionary();
		}
	}



	void FillDictionary(){
		dictionary = (PhraseDictionaryData)target;

		var phrases = new List<PhraseSegmentData>();
		AddObjectsFromDirectoryRecursively (Application.dataPath + "/" + dictionary.path, "assets/" + dictionary.path, phrases);
		dictionary.SetPhrases (phrases);

		Debug.Log ("Not found: " + count);

		EditorUtility.SetDirty(dictionary);
	}

	int count = 0;
	void AddObjectsFromDirectoryRecursively(string directory, string assetPath, List<PhraseSegmentData> phrases){
		var paths = Directory.GetFiles(directory);
		//Debug.Log(paths.Length);
		foreach(string p in paths){
			var obj = AssetDatabase.LoadAssetAtPath(assetPath + "/" + Path.GetFileNameWithoutExtension(p), typeof(Object));

			//Debug.Log(assetPath + "/" + Path.GetFileNameWithoutExtension(p) + ";;" + obj);
			var phrase = obj as PhraseSegmentData;
			if(phrase){
				if(phrase.ChildPhrases.Length == 1){
					var w = DictionaryData.Instance.GetLocalEntryFromKana(phrase.Text);
					if(w != null){
						phrase.wordID = w.ID;
						EditorUtility.SetDirty(phrase);
					} else {
						if(phrase.WordID != -1){
							Debug.Log("No ID found for " + phrase.Text + "; " + phrase.name);
							count++;
						}
					}
				}
				phrases.Add(phrase);
			}
		}

		var dirs = Directory.GetDirectories (directory);
		foreach (var dir in dirs) {
			AddObjectsFromDirectoryRecursively(dir, assetPath + "/" + Path.GetFileName(dir), phrases);
		}
	}

}
