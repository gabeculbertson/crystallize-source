using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NPCActorLineEditor : EditorWindow {

    [MenuItem("Crystallize/Lines/NPC line editor")]
    public static void Open() {
        var window = GetWindow<NPCActorLineEditor>();
        window.Initialize();
    }

    Vector2 scroll = new Vector2();
    GUIStyle style = null;
    string filterText = "";

    Dictionary<DialogueActorLine, List<DialogueActorLine>> lines;
    List<DialogueActorLine> lineOrder;

    Dictionary<DialogueActorLine, bool> isEditingTogether = new Dictionary<DialogueActorLine, bool>();

    NPCActorLine selectedLine = null;

    void Initialize() {
        lines = EditorUtilities.GetAggregatedNPCLines();

        lineOrder = (from l in lines.Keys orderby l.Phrase.GetText() select l).ToList();
        //foreach (var q in GameData.Instance.QuestData.Quests.Items) {
        //    lines.Add(q.QuestPromptLine);
        //    names[q.QuestPromptLine] = q.Title;
        //}
    }

    void OnGUI() {
        var f = filterText;
        filterText = EditorGUILayout.TextField("Filter", filterText);
        if (f != filterText) {
            lineOrder = (from l in lines.Keys where l.Phrase.GetText(JapaneseTools.JapaneseScriptType.Romaji).Contains(filterText)
                         orderby l.Phrase.GetText() select l).ToList();
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);
        if (style == null) {
            style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleLeft;
        }

        if (selectedLine != null) {
            if (GUILayout.Button("Back")) {
                selectedLine = null;
                return;
            }

            EditorUtilities.DrawNPCLine(selectedLine);

            foreach (var l in lines[selectedLine]) {
                if (!isEditingTogether.ContainsKey(l)) {
                    isEditingTogether[l] = true;
                }

                EditorGUILayout.BeginHorizontal();
                isEditingTogether[l] = EditorGUILayout.Toggle(isEditingTogether[l], GUILayout.Width(30f));
                EditorGUILayout.LabelField(string.Format("{0} ({1})", l.Phrase.GetText(), l.Phrase.Translation));
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Update")) {
                foreach (var l in lines[selectedLine]) {
                    if (isEditingTogether[l]) {
                        l.Phrase.PhraseElements = selectedLine.Phrase.PhraseElements;
                        l.Phrase.Translation = selectedLine.Phrase.Translation;
                    }
                }
            }
        } else {
            foreach (var l in lineOrder){//lines.Keys) {
                if (GUILayout.Button(l.Phrase.GetText(), style)) {
                    selectedLine = l as NPCActorLine;
                }
                //EditorUtilities.DrawPlayerLine(l);
            }
        }

        EditorGUILayout.EndScrollView();
    }

}
