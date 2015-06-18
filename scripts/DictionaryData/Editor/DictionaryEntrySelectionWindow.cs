using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DictionaryEntrySelectionWindow : EditorWindow {
	
	public static void Open(List<DictionaryDataEntry> entries){
		var window = GetWindow<DictionaryEntrySelectionWindow>();

		window.entries = entries;
		foreach (var e in entries) {
			window.entryStrings[e] = string.Format("[{0}] {1} ({2}) {3}", e.ID, e.Kanji, e.Kana, e.EnglishSummary);
		}
	}

	Vector2 scrollPosition;
	List<DictionaryDataEntry> entries = new List<DictionaryDataEntry>();
	Dictionary<DictionaryDataEntry, string> entryStrings = new Dictionary<DictionaryDataEntry, string>();

	void OnGUI(){
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
		EditorGUILayout.BeginVertical ();

		foreach (var entry in entries) {
			if(GUILayout.Button(entryStrings[entry])){
				DictionaryData.Instance.UpdateEntry(entry);
			}
		}

		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndScrollView ();
	}

}
