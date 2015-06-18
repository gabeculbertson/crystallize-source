using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomEditor(typeof(WorldObjectComponent))]
public class WorldObjectInspector : Editor {

    public static void DrawGlobalID(UnityEngine.Object target, Action<int> setID, Action getNewID, Func<int> getID) {
        int newID = EditorGUILayout.IntField("ID", getID());
        if (newID != getID()) {
            setID(newID);
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Get new ID")) {
            getNewID();
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Edit...")) {
            WorldObjectEditorWindow.Open(getID());
        }

        var hasQuest = GameData.Instance.QuestData.GetQuestInfoFromWorldID(getID()) != null;
        if (hasQuest) {
            EditorGUILayout.Toggle("Quest", hasQuest);
        }
    }

	public override void OnInspectorGUI ()
	{
        var id = (WorldObjectComponent)target;
        DrawGlobalID(id, id.SetID, id.GetNewID, id.GetID);
	}

}
