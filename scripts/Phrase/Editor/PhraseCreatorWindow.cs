using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JapaneseTools;

public class PhraseCreatorWindow : EditorWindow {

	public static PhraseDictionaryData GetPhraseDictionary(){
		var assets = AssetDatabase.FindAssets ("PhraseDictionary");
		foreach (var asset in assets) {
			var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof(PhraseDictionaryData));
			if(obj){
				return (PhraseDictionaryData)obj;
			}
		}
		return null;
	}

	string phraseText = "";
	string overrideFilename = "";
	PhraseDictionaryData phraseDictionary;

	Dictionary<string, PhraseSegmentData> tempPhraseData = new Dictionary<string, PhraseSegmentData>();
	Dictionary<PhraseSegmentData, string> overrideFilenames = new Dictionary<PhraseSegmentData, string>();
	Dictionary<PhraseSegmentData, Editor> editors = new Dictionary<PhraseSegmentData, Editor>();

	[MenuItem("Crystallize/Phrase creator")]
	public static void Open(){
		var window = GetWindow<PhraseCreatorWindow>();

		window.phraseDictionary = GetPhraseDictionary ();
	}

	void OnGUI(){
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Phrase text", GUILayout.Width(120f));
		phraseText = EditorGUILayout.TextField (phraseText);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Override file", GUILayout.Width(120f));
		overrideFilename = EditorGUILayout.TextField (overrideFilename);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Romaji", GUILayout.Width(120f));
		EditorGUILayout.TextField (KanaConverter.Instance.ConvertToRomaji(phraseText));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Hiragana", GUILayout.Width(120f));
		EditorGUILayout.TextField (KanaConverter.Instance.ConvertToHiragana(phraseText));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Katakana", GUILayout.Width(120f));
		EditorGUILayout.TextField (KanaConverter.Instance.ConvertToKatakana(phraseText));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Save segments")) {
			SaveAllSegments();
		}

		if (GUILayout.Button ("Save all")) {
			SaveAllSegments();
			SavePhraseAsset(GetPhrase());
		}

		if (GUILayout.Button ("Reorganize all")) {
			RelocateAllPhraseDataAssets();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		var segments = phraseText.Split (new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (var segment in segments) {
			DrawPhraseSegment(segment);
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical ();
	}

	void SaveAllSegments(){
		var segments = phraseText.Split (new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (var segment in segments) {
			if(tempPhraseData.ContainsKey(segment)){
				SavePhraseAsset(tempPhraseData[segment]);
				tempPhraseData.Remove(segment);
			}
		}
	}

	Phrase GetPhrase(){
		var phrase = ScriptableObject.CreateInstance<Phrase> ();
		var segments = phraseText.Split (new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (var segment in segments) {
			string jpString = KanaConverter.Instance.ConvertToHiragana (segment);
			Debug.Log("Segment: " + jpString);
			var phraseData = phraseDictionary.GetPhraseData (jpString);
			Debug.Log("PhraseData: " + phraseData.Text);
			phrase.phraseSegments.Add(phraseData);
		}
		phrase.SetCategory (PhraseCategory.Sentence);
		phrase.Initialize (phrase.Text);
		overrideFilenames [phrase] = overrideFilename;
		return phrase;
	}

	void DrawPhraseSegment(string phrase){
		bool isCreated = phraseDictionary.Contains (KanaConverter.Instance.ConvertToHiragana (phrase));
		string jpString = KanaConverter.Instance.ConvertToHiragana (phrase);

		GUILayout.BeginVertical (GUI.skin.box, GUILayout.Width(300f));

		PhraseSegmentData phraseData = null;
		if (isCreated) {
			phraseData = phraseDictionary.GetPhraseData (jpString);
		} else {
			if(!tempPhraseData.ContainsKey(phrase)){
				tempPhraseData[phrase] = ScriptableObject.CreateInstance<PhraseSegmentData>();
				tempPhraseData[phrase].Initialize(jpString);
			}
			phraseData = tempPhraseData[phrase];
			GUI.color = Color.yellow;
		}

		if (!overrideFilenames.ContainsKey (phraseData)) {
			overrideFilenames[phraseData] = "";
		}
		overrideFilenames [phraseData] = EditorGUILayout.TextField ("Filename", overrideFilenames [phraseData]);

		if (GUILayout.Button (phrase, GUI.skin.box, GUILayout.ExpandWidth (true)) && !isCreated) {
			SavePhraseAsset(phraseData);
			tempPhraseData.Remove(phrase);
		}

		GetEditor(phraseData).DrawDefaultInspector();

		GUI.color = Color.white;
		GUILayout.EndVertical ();
	}

	Editor GetEditor(PhraseSegmentData phrase){
		if (!editors.ContainsKey (phrase)) {
			editors[phrase] = Editor.CreateEditor (phrase);
		}
		return editors [phrase];
	}

	void SavePhraseAsset(PhraseSegmentData phrase){
		var assetName = phrase.ConvertedText;
		//Debug.Log (phrase.ConvertedText);
		if (overrideFilenames.ContainsKey (phrase)) {
			if (overrideFilenames [phrase] != "") {
				assetName = overrideFilenames [phrase];
			}
		}

		var subpath = Path.GetDirectoryName (AssetDatabase.GetAssetPath (phraseDictionary)) + "/Phrases/" + phrase.Category.ToString() + "/";
		var absolutePath = Path.GetDirectoryName (Application.dataPath) + "/" + subpath;
		var filename = assetName + ".asset";
		
		if (!Directory.Exists (absolutePath)) {
			Directory.CreateDirectory(absolutePath);
		}
		
		Debug.Log ("Saving: " + subpath + filename);
		AssetDatabase.CreateAsset (phrase, subpath + filename);
		phraseDictionary.AddPhrase (phrase);
		EditorUtility.SetDirty (phraseDictionary);
		AssetDatabase.SaveAssets ();
	}

	void RelocateAllPhraseDataAssets(){
		var assets = AssetDatabase.GetAllAssetPaths ();
		//var s = "";
		foreach (var asset in assets) {
			var obj = AssetDatabase.LoadAssetAtPath(asset, typeof(PhraseSegmentData)) as PhraseSegmentData;
			if(obj){
				var subpath = Path.GetDirectoryName (AssetDatabase.GetAssetPath (phraseDictionary)) + "/Phrases/" + obj.Category.ToString() + "/";
				var absolutePath = Path.GetDirectoryName (Application.dataPath) + "/" + subpath;
				var filename = KanaConverter.Instance.ConvertToRomaji(obj.Text) + ".asset";
				
				if (!Directory.Exists (absolutePath)) {
					Directory.CreateDirectory(absolutePath);
				}
				
				//Debug.Log ("Saving: " + subpath + filename);
				AssetDatabase.MoveAsset(asset, subpath + filename);
				//s += obj.name + obj.Text + "\t";
			}
		}
		AssetDatabase.Refresh ();
		//Debug.Log (s);
	}

}
