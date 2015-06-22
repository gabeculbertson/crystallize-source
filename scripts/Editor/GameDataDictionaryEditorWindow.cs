using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class GameDataDictionaryEditorWindow<T> : EditorWindow where T : IHasID, ISerializableDictionaryItem<int>, new() {

    protected abstract DictionaryCollectionGameData<T> Dictionary { get; }

    void OnGUI() {
        foreach (var j in Dictionary.Items) {
            DrawItem(j);
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+")) {
            Dictionary.AddNewItem();
        }
        GUILayout.EndHorizontal();
    }

    void DrawItem(T j) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorUtilities.DrawObject(j);

        EditorGUILayout.EndVertical();
    }

}
