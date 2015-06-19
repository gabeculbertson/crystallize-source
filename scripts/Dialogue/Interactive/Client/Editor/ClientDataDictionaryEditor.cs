using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(ConversationClientDictionary))]
public class ClientDataDictionaryEditor : Editor {

	ConversationClientDictionary dictionary;
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		if(GUILayout.Button("Fill")){
			FillDictionary();
		}
	}
	
	void FillDictionary(){
		dictionary = (ConversationClientDictionary)target;
		
		var clientList = new List<ConversationClientData>();
		AddObjectsFromDirectoryRecursively (Application.dataPath + "/" + dictionary.path, "assets/" + dictionary.path, clientList);
		dictionary.conversationClients = clientList;
		
		EditorUtility.SetDirty(dictionary);
	}
	
	void AddObjectsFromDirectoryRecursively(string directory, string assetPath, List<ConversationClientData> clientList){
		var paths = Directory.GetFiles(directory);
		foreach(string p in paths){
			var obj = AssetDatabase.LoadAssetAtPath(assetPath + "/" + Path.GetFileNameWithoutExtension(p), typeof(Object));

			Debug.Log (obj);
			var client = obj as ConversationClientData;
			if(client){
				clientList.Add(client);
			}
		}
		
		var dirs = Directory.GetDirectories (directory);
		foreach (var dir in dirs) {
			AddObjectsFromDirectoryRecursively(dir, assetPath + "/" + Path.GetFileName(dir), clientList);
		}
	}

}
