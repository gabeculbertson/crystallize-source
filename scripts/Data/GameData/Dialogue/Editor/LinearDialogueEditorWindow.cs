using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

public class LinearDialogueEditorWindow : EditorWindow {

    public static void Open(LinearDialogueHolder holder) {
        var window = GetWindow<LinearDialogueEditorWindow>();
        window.Initialize(holder);
    }


    LinearDialogueHolder holder;
    LinearDialogueSection dialogue;
    //PhraseAudioResources audioResources;

    void Initialize(LinearDialogueHolder holder) {
        this.holder = holder;
        this.dialogue = GameData.Instance.DialogueData.LinearDialogues.GetItem(holder.DialogueID);
    }

    void OnGUI() {
        if (GUILayout.Button("Choose existing...")) {
            LinearDialogueSelectorWindow.Open(SetDialogue);
        }

        dialogue.AvailableByDefault = EditorGUILayout.Toggle("Available by default", dialogue.AvailableByDefault);
        dialogue.GiveObjectives = EditorGUILayout.Toggle("Give objectives", dialogue.GiveObjectives);

        foreach (var l in dialogue.Lines) {
            if (!DrawLine(l)) {
                break;
            }
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add NPC line")) {
            dialogue.AddNewNPCLine();
        }

        if (GUILayout.Button("Add player line")) {
            dialogue.AddNewPlayerLine();
        }

        EditorGUILayout.EndHorizontal();
    }

    bool DrawLine(DialogueActorLine line) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.BeginHorizontal();
        //if (GUILayout.Button(line.Phrase.GetText())) {
        //    PhraseEditorWindow.Open(line.Phrase);
        //}

        if (GUILayout.Button("x", GUILayout.Width(24f))) {
            dialogue.Lines.Remove(line);
            return false;
        }
        EditorGUILayout.EndHorizontal();

        if (line is PlayerActorLine) {
            EditorUtilities.DrawPlayerLine((PlayerActorLine)line);
            //DrawPlayerLine((PlayerActorLine)line);
        } else {
            EditorUtilities.DrawNPCLine((NPCActorLine)line);
        }

        EditorGUILayout.EndVertical();
        return true;
    }

    void DrawPlayerLine(PlayerActorLine line) {
        var c = GUI.color;
        if (line.Phrase.Translation == "") {
            GUI.color = Color.red;
        }
        EditorGUILayout.LabelField(line.Phrase.Translation, GUI.skin.box);
        
        GUI.color = c;

        line.SetOverrideGivenWords(EditorGUILayout.Toggle("Override given words", line.OverrideGivenWords));

         if (line.OverrideGivenWords) {

             GUILayout.BeginHorizontal();

             for (int i = 0; i < line.Phrase.PhraseElements.Count; i++) {
                 var t = line.Phrase.PhraseElements[i].GetText();
                 if(!line.GetWordGiven(i)){
                     t = "{" + t + "}";
                 }

                 if (GUILayout.Button(t)) {
                     //Debug.Log("given: " + line.GetWordGiven(i));
                     line.SetWordGiven(i, !line.GetWordGiven(i));
                 }
             }

             GUILayout.EndHorizontal();
         }
    }

    void SetDialogue(int dialogueID) {
        holder.DialogueID = dialogueID;
        Initialize(holder);
    }

}
