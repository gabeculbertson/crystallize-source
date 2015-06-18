using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class BranchedDialogueEditorWindow : EditorWindow {

	public static void Open(BranchedDialogueHolder holder){ //BranchedDialogue dialogue){
		var window = GetWindow<BranchedDialogueEditorWindow> ();

		window.holder = holder;
		window.Initialize ();

		var strings = new List<string> ();
		var ids = new List<int> ();
		foreach (var bd in GameData.Instance.DialogueData.BranchedDialogues.Items) {
			strings.Add(bd.ID + "; " + bd.Elements.Count);
			ids.Add(bd.ID);
		}
		window.allBranchedDialogueNames = strings.ToArray ();
		window.allBranchedDialogueIDs = ids;
	}

	void Initialize(){
		targetDialogue = GameData.Instance.DialogueData.BranchedDialogues.GetItem (holder.BranchedDialogueID);

        behaviorTypes = (from t in Assembly.GetAssembly(typeof(ResponseBehaviorGameData)).GetTypes()
                         where typeof(ResponseBehaviorGameData).IsAssignableFrom(t)
                         select t).ToList();
        behaviorStrings = (from t in behaviorTypes select t.ToString()).ToArray();
	}

    string[] behaviorStrings;
    List<Type> behaviorTypes;

	string[] allBranchedDialogueNames;
	List<int> allBranchedDialogueIDs;

	BranchedDialogueHolder holder;
	BranchedDialogue targetDialogue;

	void OnGUI(){
		EditorGUILayout.BeginVertical ();

		var selected = allBranchedDialogueIDs.IndexOf (holder.BranchedDialogueID);
		var newSelected = EditorGUILayout.Popup (selected, allBranchedDialogueNames);
		if (newSelected != selected) {
			holder.BranchedDialogueID = allBranchedDialogueIDs[newSelected];
			Initialize();
		}

		var options = GetPhraseClassStrings ();
		foreach (var b in targetDialogue.Elements) {
			DrawBranch(options, b);
		}

		if (GUILayout.Button ("Add branch")) {
			targetDialogue.AddBranch();
		}

		EditorGUILayout.EndVertical ();
	}

	void DrawBranch(string[] options, BranchedDialogueElement element){
		EditorGUILayout.BeginVertical (GUI.skin.box);

		element.Description = EditorGUILayout.TextField (element.Description);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prompt", GUILayout.Width(100f));
        EditorUtilities.DrawPlayerLine(element.PromptPhrase);
        //if (GUILayout.Button(element.PromptPhrase.GetText())) {
        //    PhraseEditorWindow.Open(element.PromptPhrase);
        //}

        EditorGUILayout.EndHorizontal();

		SetSelected(element, EditorGUILayout.Popup (GetSelected (element), options));

        if (element.Responese == null) {
            element.Responese = new ResponseBehaviorGameData();
        }
        var oldS = behaviorTypes.IndexOf(element.Responese.GetType());
        var newS = EditorGUILayout.Popup(oldS, behaviorStrings);
        if (oldS != newS) {
            element.Responese = (ResponseBehaviorGameData)Activator.CreateInstance(behaviorTypes[newS]);
        }

        EditorUtilities.DrawNPCLine(element.ResponseLine);
        //if (GUILayout.Button (element.ResponsePhrase.GetText ())) {
        //    PhraseEditorWindow.Open(element.ResponsePhrase);
        //}

		EditorGUILayout.EndVertical ();
	}

	int GetSelected(BranchedDialogueElement element){
		for (int i = 0; i < GameData.Instance.PhraseClassData.PhraseClasses.Count; i++) {
			if(GameData.Instance.PhraseClassData.PhraseClasses[i].ID == element.PromptPhraseClassID){
				return i;
			}
		}
		return -1;
	}

	void SetSelected(BranchedDialogueElement element, int index){
		if (index >= 0) {
			element.PromptPhraseClassID = GameData.Instance.PhraseClassData.PhraseClasses [index].ID;
		}
	}

	string[] GetPhraseClassStrings(){
		return (from c in GameData.Instance.PhraseClassData.PhraseClasses select c.Description).ToArray ();
	}

}
