using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogueSelectorWindow : EditorWindow {

    public static void Open(Action<int> confirmAction) {
        var window = GetWindow<NPCDialogueSelectorWindow>();
        window.Initialize(confirmAction);
    }

    Vector2 scroll;
    List<NPCDialogue> npcDialogues;
    Action<int> confirmAction;

    void Initialize(Action<int> confirmAction) {
        this.confirmAction = confirmAction;
        npcDialogues = new List<NPCDialogue>(GameData.Instance.DialogueData.NPCDialogues.Items);
    }

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (var d in npcDialogues) {
            if (!DrawDialogue(d)) {
                GameData.Instance.DialogueData.NPCDialogues.RemoveItem(d.ID);
                break;
            }
        }
        EditorGUILayout.EndScrollView();
    }

    bool DrawDialogue(NPCDialogue dialogue) {
        GUILayout.BeginVertical(GUI.skin.box);
        var result = true;
        var s = "";
        foreach (var l in dialogue.Lines) {
            s += l.Phrase.GetText() + "\n";
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(dialogue.ID.ToString());
        if (GUILayout.Button("x", GUILayout.Width(30f))) {
            result = false;
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button(s)) {
            if (confirmAction != null) {
                confirmAction(dialogue.ID);
                Close();
            }
        }

        GUILayout.EndVertical();

        return result;
    }

}
