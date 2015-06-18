using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

public class NPCDialogueEditorWindow : EditorWindow {

    public static void Open(NPCDialogueHolder holder){ //NPCDialogue dialogue) {
        var window = GetWindow<NPCDialogueEditorWindow>();
        window.Initialize(holder);
    }

    public static void PlayClip(AudioClip clip) {
        Assembly assembly = typeof(AudioImporter).Assembly;
        Type audioUtilType = assembly.GetType("UnityEditor.AudioUtil");

        Type[] typeParams = { typeof(AudioClip), typeof(int), typeof(bool) };
        object[] objParams = { clip, 0, false };

        MethodInfo method = audioUtilType.GetMethod("PlayClip", typeParams);
        method.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, objParams, null);
    }

    public static PhraseAudioResources GetAudioResources() {
        var assets = AssetDatabase.FindAssets("AudioResources");
        foreach (var asset in assets) {
            var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof(PhraseAudioResources));
            if (obj) {
                return (PhraseAudioResources)obj;
            }
        }
        return null;
    }

    NPCDialogueHolder holder;
    NPCDialogue dialogue;
    PhraseAudioResources audioResources;

    void Initialize(NPCDialogueHolder holder) {
        this.holder = holder;
        this.dialogue = GameData.Instance.DialogueData.NPCDialogues.GetItem(holder.DialogueID);

        audioResources = GetAudioResources();
    }

    void OnGUI() {
        if (GUILayout.Button("Choose existing...")) {
            NPCDialogueSelectorWindow.Open(SetDialogue);
        }

        foreach (var l in dialogue.Lines) {
            if (!DrawLine(l)) {
                break;
            }
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add line")) {
            dialogue.AddLine();
        }

        EditorGUILayout.EndHorizontal();
    }

    bool DrawLine(DialogueActorLine line) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(line.Phrase.GetText())) {
            PhraseEditorWindow.Open(line.Phrase);
        }

        if (GUILayout.Button("x", GUILayout.Width(24f))) {
            dialogue.Lines.Remove(line);
            return false;
        }
        EditorGUILayout.EndHorizontal();

        if (line is PlayerActorLine) {
            DrawPlayerLine((PlayerActorLine)line);
        } else {
            DrawNPCLine((NPCActorLine)line);
        }

        EditorGUILayout.EndVertical();
        return true;
    }

    void DrawPlayerLine(PlayerActorLine line) {
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

    void DrawNPCLine(NPCActorLine line) {
        dialogue.SetActorIndex(line, EditorGUILayout.IntField("Actor index", dialogue.GetActorIndex(line)));

        EditorGUILayout.BeginHorizontal();
        
        var a1 = audioResources.GetAudioResource(line.AudioClipID);
        var a2 = (AudioClip)EditorGUILayout.ObjectField("Audio clip", a1, typeof(AudioClip), false);
        if (a1 != a2) {
            var id =  audioResources.GetAudioResourceID(a2);
            Debug.Log("Changing to " + id);
            EditorUtility.SetDirty(audioResources);
            PlayClip(a2);
            line.AudioClipID = id;
        }

        if (GUILayout.Button("Play")) {
            PlayClip(a2);
        }

        EditorGUILayout.EndHorizontal();
    }

    void SetDialogue(int dialogueID) {
        holder.DialogueID = dialogueID;
        Initialize(holder);
    }

}
