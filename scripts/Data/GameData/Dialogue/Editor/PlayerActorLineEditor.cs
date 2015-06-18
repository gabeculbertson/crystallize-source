using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlayerActorLineEditor : EditorWindow {

	[MenuItem("Crystallize/Lines/Player line editor")]
    public static void Open() {
        var window = GetWindow<PlayerActorLineEditor>();
        window.Initialize();
    }

    Vector2 scroll;
    Dictionary<DialogueActorLine, List<DialogueActorLine>> lines;

    Dictionary<DialogueActorLine, bool> isEditingTogether = new Dictionary<DialogueActorLine, bool>();

    PlayerActorLine selectedLine = null;

    void Initialize() {
        lines = EditorUtilities.GetAggregatedPlayerLines();
    }

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        if (selectedLine != null) {
            if (GUILayout.Button("Back")) {
                selectedLine = null;
                return;
            }

            EditorUtilities.DrawPlayerLine(selectedLine);

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
            foreach (var l in lines.Keys) {
                if (GUILayout.Button(l.Phrase.GetText())) {
                    selectedLine = l as PlayerActorLine;
                }
                //EditorUtilities.DrawPlayerLine(l);
            }
        }

        EditorGUILayout.EndScrollView();
    }

}
