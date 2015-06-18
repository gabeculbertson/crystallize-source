using UnityEngine;
using UnityEditor;
using System.Collections;

public class PhraseClassEditorWindow : EditorWindow {

	public static void Open(OpenDialoguePhraseClass phraseClass){
		var window = GetWindow<PhraseClassEditorWindow>();
		
		window.phraseClass = phraseClass;
	}

	OpenDialoguePhraseClass phraseClass;

	void OnGUI(){
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.LabelField (phraseClass.Description);

		foreach (var template in phraseClass.GetTemplates()) {
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField(template.Phrase.GetText());
			if(GUILayout.Button("Edit", GUILayout.Width(40f))){
				PhraseEditorWindow.Open(template.Phrase);
			}
			
			EditorGUILayout.EndHorizontal();
		}
		
		if (GUILayout.Button ("Add template")) {
			phraseClass.AddNewPhraseTemplate();
		}
		
		EditorGUILayout.EndVertical ();
	}

}
