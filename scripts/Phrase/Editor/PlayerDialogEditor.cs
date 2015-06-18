using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseTools;

[CustomEditor(typeof(FixedPlayerDialog))]
public class PlayerDialogEditor : Editor {

	FixedPlayerDialog dialog;
	PhraseDictionaryData phraseDictionary;

	List<string> titleStrings = new List<string>();
	List<string> inputStrings = new List<string>();

	Dictionary<string, string> kanaStrings = new Dictionary<string, string>();
	//Dictionary<int, PhraseSegmentData> phrases = new Dictionary<string, PhraseSegmentData>();

	Queue<int> toBeRemoved = new Queue<int>();

	void OnEnable(){
		phraseDictionary = PhraseCreatorWindow.GetPhraseDictionary ();
	}

	public override void OnInspectorGUI ()
	{
		dialog = (FixedPlayerDialog)target;

		dialog.id = EditorGUILayout.IntField ("ID", dialog.id);

		var op = DrawPhrase (0, dialog.successPhrase);
		if (op) {
			dialog.successPhrase = op;
		}

		op = DrawPhrase (1, dialog.failurePhrase);
		if (op) {
			dialog.failurePhrase = op;
		}

		op = DrawPhrase (2, dialog.unsurePhrase);
		if (op) {
			dialog.unsurePhrase = op;
		}

		int count = 3;
		foreach (var phrase in dialog.dialogPhrases) {
			op = DrawPhrase(count, phrase);
			if(op){
				dialog.dialogPhrases[count-3] = op;
			}
			count++;
		}

		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Add phrase")) {
			dialog.dialogPhrases.Add(null);
		}

		if (GUILayout.Button ("Save")) {
			EditorUtility.SetDirty(dialog.successPhrase);
			EditorUtility.SetDirty(dialog.failurePhrase);
			EditorUtility.SetDirty(dialog.unsurePhrase);
			foreach(var p in dialog.dialogPhrases){
				EditorUtility.SetDirty(p);
			}
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
		}

		EditorGUILayout.EndHorizontal ();

		titleStrings [0] = "Success Phrase";
		titleStrings [1] = "Failure Phrase";
		titleStrings [2] = "Unsure Phrase";

		while (toBeRemoved.Count > 0) {
			dialog.dialogPhrases.RemoveAt(toBeRemoved.Dequeue());
		}
	}

//	List<PhraseSegmentData> GetAllPhrases(){
//		var list = new List<PhraseSegmentData> ();
//		list.Add (dialog.successPhrase);
//
//	}

	PhraseSegmentData DrawPhrase(int index, PhraseSegmentData phrase){
		EditorGUILayout.BeginVertical (GUI.skin.box);

		while (inputStrings.Count <= index) {
			if(phrase){
				inputStrings.Add(KanaConverter.Instance.ConvertToRomaji(phrase.Text));
			} else {
				inputStrings.Add("");
			}
			titleStrings.Add("Phrase " + (index-3).ToString());
		}

		EditorGUILayout.BeginHorizontal();

		var c = GUI.color;
		GUI.color = new Color(0.6f, 0.6f, 0.6f);
		EditorGUILayout.LabelField(titleStrings[index], GUI.skin.box, GUILayout.ExpandWidth(true));
		GUI.color = c;

		EditorGUILayout.EndHorizontal ();

		EditorGUI.indentLevel++;
		EditorGUILayout.BeginHorizontal();

		GUI.SetNextControlName (index.ToString ());
		inputStrings[index] = EditorGUILayout.TextField (inputStrings[index]);
		//EditorGUILayout.LabelField("Kana:");
		EditorGUILayout.LabelField (GetKana(inputStrings[index]));
		
		EditorGUILayout.EndHorizontal ();

		if (phrase) {
			EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.LabelField (phrase.Text);
			phrase.SetTranslation(EditorGUILayout.TextField (phrase.Translation));

			EditorGUILayout.EndHorizontal ();
		} else {
			EditorGUILayout.LabelField ("NULL", GUI.skin.box);
		}

		if(index >= 3){
			if (GUILayout.Button ("Remove")) {
				toBeRemoved.Enqueue(index - 3);
			}
		}

		PhraseSegmentData outputPhraseSegmentData = null;
		if (GUI.GetNameOfFocusedControl () == index.ToString ()) {
			foreach(var p in GetPossibilities(inputStrings[index])){
				if(GUILayout.Button(string.Format("{0} ({1})", p.Text, p.Translation))){
					outputPhraseSegmentData = p;
				}
			}
		}

		EditorGUI.indentLevel--;

		EditorGUILayout.EndVertical ();

		return outputPhraseSegmentData;
	}

	bool printed = false;
	IEnumerable<PhraseSegmentData> GetPossibilities(string enteredString){
		if (enteredString == "") {
			return new PhraseSegmentData[0];
		}

		if(!printed){
			foreach (var p in phraseDictionary.Phrases) {
				if(p == null){
					Debug.Log("Null in dictionary");
					continue;
				}

				if (p.Text == null) {
					Debug.Log(p.name);
				}
			}
			printed = true;
		}

		var kanaString = GetKana (enteredString);
		try {
			return (from p in phraseDictionary.Phrases where p.Text.Contains(kanaString) || p.Translation.Contains(enteredString) select p);
		} catch (System.Exception e){
			Debug.Log(e.Message);
		}
		return new List<PhraseSegmentData>();
	}

	string GetKana(string input){
		if(!kanaStrings.ContainsKey(input)){
			kanaStrings[input] =  KanaConverter.Instance.ConvertToHiragana(input);
		}
		return kanaStrings[input];
	}

}
