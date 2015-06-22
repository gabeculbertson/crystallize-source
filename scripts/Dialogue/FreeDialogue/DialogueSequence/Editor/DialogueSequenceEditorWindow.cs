using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;
using Util.Serialization;

public class DialogueSequenceEditorWindow : EditorWindow {

    public static void Open(DialogueSequence dialogue) {
        var window = GetWindow<DialogueSequenceEditorWindow>();
        window.Initialize(dialogue);
    }

    public static void Open(string initial, Action<string> setString)
    {
        var window = GetWindow<DialogueSequenceEditorWindow>();
        window.Initialize(initial, setString);
    }

    DialogueSequence dialogue;
    Action<string> setString;

    string[] elementStrings;
    int[] elementIDs;

    void Initialize(DialogueSequence dialogue) {
        this.dialogue = dialogue;
        setString = null;
        GetElementList();
    }

    void Initialize(string xmlString, Action<string> setString)
    {
        if (xmlString != "")
        {
            try
            {
                dialogue = Serializer.LoadFromXmlString<DialogueSequence>(xmlString);
            }
            catch
            {

            }
        }

        if (dialogue == null)
        {
            dialogue = new DialogueSequence();
        }

        this.setString = setString;

        GetElementList();
    }

    void GetElementList()
    {
        elementStrings = new string[dialogue.Elements.Items.Count + 1];
        elementIDs = new int[dialogue.Elements.Items.Count + 1];

        var names = dialogue.Elements.Items.Select((e) => string.Format("[{0}] {1}", e.ID, e.Line.Phrase.GetText())).ToArray();
        var ids = dialogue.Elements.Items.Select((e) => e.ID).ToArray();

        elementStrings[0] = "NULL";
        System.Array.Copy(names, 0, elementStrings, 1, names.Length);

        elementIDs[0] = -1;
        System.Array.Copy(ids, 0, elementIDs, 1, ids.Length);
    }

    void OnGUI()
    {
        foreach (var e in dialogue.Elements.Items)
        {
            DrawElement(e);
        }

        if (GUILayout.Button("Add element"))
        {
            dialogue.GetNewDialogueElement();
            GetElementList();
        }

        if (GUILayout.Button("Save"))
        {
            Debug.Log("Set string");
            if (setString != null) {
                setString(Serializer.SaveToXmlString<DialogueSequence>(dialogue));
            }
        }

        EditorGUILayout.LabelField(Serializer.SaveToXmlString<DialogueSequence>(dialogue));
    }

    void DrawElement(DialogueElement element)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(element.ID.ToString());
        EditorUtilities.DrawObject(element.Line);
        EditorUtilities.DrawPhraseSequence(element.Prompt);

        element.DefaultNextID = GetID("Default Next ID", element.DefaultNextID);

        EditorGUI.indentLevel++;

        for (int i = 0; i < element.NextIDs.Count; i++)
        {
            element.NextIDs[i] = GetID("Next ID [" + i + "]", element.NextIDs[i]);
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            element.NextIDs.Add(-1);
        }
        if (GUILayout.Button("Remove"))
        {
            element.NextIDs.RemoveAt(element.NextIDs.Count - 1);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();
    }

    int GetIndex(int id)
    {
        for (int i = 0; i < elementIDs.Length; i++)
        {
            if (elementIDs[i] == id)
            {
                return i;
            }
        }
        return -1;
    }

    int GetID(string label, int originalID)
    {
        var index = GetIndex(originalID);
        var newIndex = EditorGUILayout.Popup(label, index, elementStrings);
        return elementIDs[newIndex];
    }

}
