using UnityEngine;
using UnityEditor;
using System.Collections;

public class PhraseClassSetEditorWindow : EditorWindow {

	[MenuItem("Crystallize/Phrase class editor")]
	public static void Open(){
		var window = GetWindow<PhraseClassSetEditorWindow>();

		window.phraseClassData = GameData.Instance.PhraseClassData;
	}

	PhraseClassGameData phraseClassData;

	void OnGUI(){
		EditorGUILayout.BeginVertical ();

		foreach (var pc in phraseClassData.PhraseClasses) {
			EditorGUILayout.BeginHorizontal();

			pc.Description = EditorGUILayout.TextField("Class" + pc.ID, pc.Description);
			if(GUILayout.Button("Edit", GUILayout.Width(40f))){
				PhraseClassEditorWindow.Open(pc);
			}

			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button ("Add phrase class")) {
			phraseClassData.AddPhraseClass();
		}

		EditorGUILayout.EndVertical ();
	}

}
