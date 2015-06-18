using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(ConversationDictionary))]
public class ConversationDictionaryEditor : Editor {

	ConversationDictionary dictionary;

	PhraseDictionaryData phraseDictionary;
	ReorderableList list;

	void OnEnable(){
		dictionary = (ConversationDictionary)target;

		var progression = GameData.Instance.ProgressionData;
		foreach (var challenge in progression.Challenges.Items) {
			if(!progression.ChallengeOrder.Contains(challenge.ChallengeID)){
				progression.ChallengeOrder.Add(challenge.ChallengeID);
			}
		}

		foreach (var assetPath in AssetDatabase.FindAssets("PhraseDictionary")) {
			//Debug.Log(AssetDatabase.GUIDToAssetPath(assetPath));
			var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetPath), typeof(Object));
			if(obj != null){
				phraseDictionary = (PhraseDictionaryData)obj;
				break;
			}
		}
		//Debug.Log (phraseDictionary);

		list = new ReorderableList (progression.ChallengeOrder, typeof(string));
		list.drawElementCallback = DrawElementCallback;
	}

	public override void OnInspectorGUI ()
	{
		DrawChallengeEditor ();

		DrawDefaultInspector();
		
		if(GUILayout.Button("Fill")){
			FillDictionary();
		}
	}

	void DrawChallengeEditor(){
		list.DoLayoutList ();
	}

	void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused){
		var progression = GameData.Instance.ProgressionData;
		var id = progression.ChallengeOrder [index];
		var chal = progression.Challenges.GetItem (id);
		var convID = chal.ConversationID;
		var conv = dictionary.GetConversationForID (convID);
		var missingWordsString = "";
		foreach(var mw in chal.MissingWordIDs){
			missingWordsString += phraseDictionary.GetPhraseForID(mw).Text + ", ";
		}
		if (missingWordsString.Length > 2) {
			missingWordsString = missingWordsString.Substring(0, missingWordsString.Length - 2);
		}
		var label = string.Format ("{0}...{1} ({2})", conv.dialogPhrases [1].Text, conv.dialogPhrases.Count, missingWordsString);
		EditorGUI.LabelField (rect, label);
	}
	
	void FillDictionary(){
		var path = Path.GetDirectoryName (AssetDatabase.GetAssetPath (dictionary));
		var convs = ScriptableObjectUtility.GetAllAssets<FixedPlayerDialog> (path);
		dictionary.SetConversations (convs);
		int id = 0;
		foreach (var conv in convs) {
			if(conv.id < 0){
				while(dictionary.ContainsID(id)){
					id++;
				}
				conv.id = id;
				EditorUtility.SetDirty(conv);
				id++;
			}
		}
		dictionary.SetConversations(convs);
		
		EditorUtility.SetDirty(dictionary);
	}

}
