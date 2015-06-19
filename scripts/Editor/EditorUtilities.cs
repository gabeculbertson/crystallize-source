﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EditorUtilities {

    static PhraseAudioResources _audioResources;

    static Dictionary<Type, Action<object, PropertyInfo>> propertyDrawers = new Dictionary<Type, Action<object, PropertyInfo>>();

    static Dictionary<PhraseSequence, string> translations;

    static PhraseAudioResources audioResources {
        get {
            if (_audioResources == null) {
                _audioResources = EditorUtilities.GetAsset<PhraseAudioResources>("AudioResources");
            }
            return _audioResources;
        }
    }

    static EditorUtilities() {
        propertyDrawers = new Dictionary<Type, Action<object, PropertyInfo>>();
        propertyDrawers[typeof(int)] = DrawIntValue;
        propertyDrawers[typeof(string)] = DrawStringValue;
        propertyDrawers[typeof(PhraseSequence)] = DrawPhraseSequenceValue;
    }

	public static object GetInstance(Type t){
		return Activator.CreateInstance (t.GetType ());
	}

    public static Dictionary<DialogueActorLine, List<DialogueActorLine>> GetAggregatedPlayerLines() {
        var playerLines = new Dictionary<DialogueActorLine, List<DialogueActorLine>>();
        foreach (var d in GameData.Instance.DialogueData.LinearDialogues.Items) {
            foreach (var l in d.Lines) {
                if (l is PlayerActorLine) {
                    AddActorLine(playerLines, (PlayerActorLine)l);
                }
            }
        }

        foreach (var d in GameData.Instance.DialogueData.BranchedDialogues.Items) {
            foreach (var b in d.Elements) {
                if (b.PromptPhrase.Phrase.PhraseElements.Count > 0) {
                    AddActorLine(playerLines, b.PromptPhrase);
                }
            }
        }
        return playerLines;
    }

    public static Dictionary<DialogueActorLine, List<DialogueActorLine>> GetAggregatedNPCLines() {
        var npcLines = new Dictionary<DialogueActorLine, List<DialogueActorLine>>();
        foreach (var d in GameData.Instance.DialogueData.LinearDialogues.Items) {
            foreach (var l in d.Lines) {
                if (l is NPCActorLine) {
                    AddActorLine(npcLines, l);
                }
            }
        }

        foreach (var d in GameData.Instance.DialogueData.BranchedDialogues.Items) {
            foreach (var b in d.Elements) {
                AddActorLine(npcLines, b.ResponseLine);
            }
        }

        foreach (var d in GameData.Instance.DialogueData.NPCDialogues.Items) {
            foreach (var l in d.Lines) {
                AddActorLine(npcLines, l);
            }
        }

        foreach (var q in GameData.Instance.QuestData.Quests.Items) {
            AddActorLine(npcLines, q.QuestPromptLine);
        }

        return npcLines;
    }

    //public static List<PlayerActorLine> GetPlayerLines() {
    //    var playerLines = new List<PlayerActorLine>();
    //    foreach (var d in GameData.Instance.DialogueData.LinearDialogues.Items) {
    //        foreach (var l in d.Lines) {
    //            if (l is PlayerActorLine) {
    //                AddPlayerLine(playerLines, (PlayerActorLine)l);
    //            }
    //        }
    //    }

    //    foreach (var d in GameData.Instance.DialogueData.BranchedDialogues.Items) {
    //        foreach (var b in d.Elements) {
    //            if (b.PromptPhrase.Phrase.PhraseElements.Count > 0) {
    //                AddPlayerLine(playerLines, b.PromptPhrase);
    //            }
    //        }
    //    }
    //    return playerLines;
    //}

    //static bool AddPlayerLine(List<PlayerActorLine> playerLines, DialogueActorLine line) {
    //    foreach (var l in playerLines) {
    //        if (PhraseSequence.IsPhraseEquivalent(line.Phrase, l.Phrase)) {
    //            return false;
    //        }
    //    }
    //    playerLines.Add(line);
    //    return true;
    //}

    static void AddActorLine(Dictionary<DialogueActorLine, List<DialogueActorLine>> playerLines, DialogueActorLine line) {
        foreach (var l in playerLines.Keys) {
            if (PhraseSequence.IsPhraseEquivalent(line.Phrase, l.Phrase)) {
                playerLines[l].Add(line);
                return;
            }
        }
        var list = new List<DialogueActorLine>();
        list.Add(line);
        playerLines.Add(line, list);
    }

    static string GetTranslation(PhraseSequence phrase) {
        if (translations == null) {
            translations = new Dictionary<PhraseSequence, string>();
            var lines = GetAggregatedPlayerLines();
                //GetPlayerLines();
            foreach (var l in lines.Keys) {
                var simPhr = GetSimilarPhrase(translations.Keys, l.Phrase);
                if (simPhr == null) {
                    if (l.Phrase.Translation != "" && l.Phrase.Translation != null) {
                        translations[l.Phrase] = l.Phrase.Translation;
                        Debug.Log("Adding: " + l.Phrase.GetText() + l.Phrase.Translation);
                    }
                }
            }
        }
            
        var key = GetSimilarPhrase(translations.Keys, phrase);
        if (key != null) {
            return translations[key];
        }
        return "";
    }

    public static PhraseSequence GetSimilarPhrase(IEnumerable<PhraseSequence> phraseSet, PhraseSequence phrase) {
        foreach (var p in phraseSet) {
            if (PhraseSequence.IsPhraseEquivalent(p, phrase)) {
                return p;
            }
        }
        return null;
    }

    public static void DrawPlayerLine(PlayerActorLine line) {
        var c = GUI.color;
        if (line.Phrase.Translation == null || line.Phrase.Translation == "") {
            GUI.color = Color.red;
        }
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUI.color = c;

        if (GUILayout.Button(line.Phrase.GetText())) {
            PhraseEditorWindow.Open(line.Phrase);
        }

        line.Phrase.Translation = EditorGUILayout.TextField("Translation", line.Phrase.Translation);
        if (line.Phrase.Translation == null || line.Phrase.Translation == "") {
            line.Phrase.Translation = GetTranslation(line.Phrase);
        }

        line.SetUseAutomaticProgression(EditorGUILayout.Toggle("Use automatic progression", line.UseAutomaticProgression));
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

         line.ProvideMissingWordsMessage = EditorGUILayout.Toggle("Provide missing words message", line.ProvideMissingWordsMessage);

         EditorGUILayout.EndVertical();
    }

    public static void DrawNPCLine(NPCActorLine line) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (GUILayout.Button(line.Phrase.GetText())) {
            PhraseEditorWindow.Open(line.Phrase);
        }

        EditorGUILayout.BeginHorizontal();

        if (audioResources != null) {
            var a1 = audioResources.GetAudioResource(line.AudioClipID);
            var a2 = (AudioClip)EditorGUILayout.ObjectField("Audio clip", a1, typeof(AudioClip), false);
            if (a1 != a2) {
                var id = audioResources.GetAudioResourceID(a2);
                Debug.Log("Changing to " + id);
                EditorUtility.SetDirty(audioResources);
                if (a2 != null) {
                    EditorUtilities.PlayClip(a2);
                }
                line.AudioClipID = id;
            }

            if (GUILayout.Button("Play")) {
                EditorUtilities.PlayClip(a2);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    public static void DrawPhraseSequence(PhraseSequence phrase) {
        if (GUILayout.Button(phrase.GetText())) {
            PhraseEditorWindow.Open(phrase);
        }
    }

    public static void DrawObject(object obj) {
        foreach (var p in obj.GetType().GetProperties()) {
            if (!p.CanWrite) {
                continue;
            }
            
            DrawProperty(obj, p);
        }
    }

    public static void DrawProperty(object obj, PropertyInfo p) {
		if (propertyDrawers.ContainsKey(p.PropertyType)) {
            propertyDrawers[p.PropertyType](obj, p);
        } else if (typeof(IList).IsAssignableFrom(p.PropertyType)) {
            DrawListValue(obj, p);
        } else if(obj.GetType().IsSerializable){
			EditorGUI.indentLevel++;
			DrawObject(obj);
			EditorGUI.indentLevel--;
		} else {
            DrawLabelValue(obj, p);
        }
    }

    static void DrawStringValue(object obj, PropertyInfo p) {
        p.SetValue(obj, EditorGUILayout.TextField(p.Name, (string)p.GetValue(obj, new object[0])), new object[0]);
    }

    static void DrawIntValue(object obj, PropertyInfo p) {
        p.SetValue(obj, EditorGUILayout.IntField(p.Name, (int)p.GetValue(obj, new object[0])), new object[0]);
    }

    static void DrawListValue(object obj, PropertyInfo p) {
        //Debug.Log(p.PropertyType + " Generic: " + p.PropertyType.GetGenericArguments().Length);
        if (p.PropertyType.GetGenericArguments().Length > 0) {
            var list = (IList)p.GetValue(obj, new object[0]);
            var t = p.PropertyType.GetGenericArguments()[0];
            EditorGUILayout.LabelField(p.Name);
            if (t == typeof(string)) {
                EditorGUI.indentLevel++;
                for (int i = 0; i < list.Count; i++) {
                    list[i] = EditorGUILayout.TextField(list[i].ToString());
                }
                if (GUILayout.Button("Add...")) {
                    list.Add("");
                }
                if (list.Count > 0) {
                    if (GUILayout.Button("Remove...")) {
                        list.RemoveAt(list.Count - 1);
                    }
                }
                EditorGUI.indentLevel--;
            }
        } else {
            DrawLabelValue(obj, p);
        }
    }

    static void DrawPhraseSequenceValue(object obj, PropertyInfo p) {
        var ps = (PhraseSequence)p.GetValue(obj, new object[0]);
        if (GUILayout.Button(ps.GetText())) {
            PhraseEditorWindow.Open(ps);
        }
    }

    static void DrawLabelValue(object obj, PropertyInfo p) {
        EditorGUILayout.LabelField(p.PropertyType.ToString(), obj.ToString());
    }

    public static void PlayClip(AudioClip clip) {
        Assembly assembly = typeof(AudioImporter).Assembly;
        Type audioUtilType = assembly.GetType("UnityEditor.AudioUtil");

        Type[] typeParams = { typeof(AudioClip), typeof(int), typeof(bool) };
        object[] objParams = { clip, 0, false };

        MethodInfo method = audioUtilType.GetMethod("PlayClip", typeParams);
        method.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, objParams, null);
    }

    public static T GetAsset<T>(string assetName) where T : UnityEngine.Object {
        var assets = AssetDatabase.FindAssets(assetName);
        foreach (var asset in assets) {
            var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof(T));
            if (obj) {
                return (T)obj;
            }
        }
        return null;
    }



}