using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(SocialDataDictionary))]
public class SocialDataDictionaryEditor : Editor {

	SocialDataDictionary dictionary;
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		if(GUILayout.Button("Fill")){
			FillDictionary();
		}
	}
	
	void FillDictionary(){
		dictionary = (SocialDataDictionary)target;
		
		var socialDataList = new List<SocialData>();
		AddObjectsFromDirectoryRecursively (Application.dataPath + "/" + dictionary.path, "assets/" + dictionary.path, socialDataList);
		dictionary.socialData = socialDataList;
		
		EditorUtility.SetDirty(dictionary);
	}
	
	void AddObjectsFromDirectoryRecursively(string directory, string assetPath, List<SocialData> socialList){
		var paths = Directory.GetFiles(directory);
		//Debug.Log(paths.Length);
		foreach(string p in paths){
			var obj = AssetDatabase.LoadAssetAtPath(assetPath + "/" + Path.GetFileNameWithoutExtension(p), typeof(Object));
			
			//Debug.Log(assetPath + "/" + Path.GetFileNameWithoutExtension(p) + ";;" + obj);
			var socialData = obj as SocialData;
			if(socialData){
				socialList.Add(socialData);
			}
		}
		
		var dirs = Directory.GetDirectories (directory);
		foreach (var dir in dirs) {
			AddObjectsFromDirectoryRecursively(dir, assetPath + "/" + Path.GetFileName(dir), socialList);
		}
	}
}
