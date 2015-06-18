using UnityEngine;
using UnityEditor;
using System.Collections;

public class ContextDataEditorWindow : EditorWindow {

    public static void Open(ContextData contextData) {
        var window = GetWindow<ContextDataEditorWindow>();

        window.Initialize(contextData);
    }

    ContextData contextData;
    string newContext = "";

    void Initialize(ContextData contextData) {
        this.contextData = contextData;
    }

    void OnGUI() {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(contextData.WorldID.ToString());

        foreach (var c in contextData.Elements) {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(c.Name, GUILayout.Width(100f));
            if (GUILayout.Button(c.Data.GetText())) {
                PhraseEditorWindow.Open(c.Data);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        newContext = EditorGUILayout.TextField(newContext, GUILayout.Width(150f));
        if (GUILayout.Button("Add")) {
            contextData.UpdateElement(newContext, new PhraseSequence());
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

}
