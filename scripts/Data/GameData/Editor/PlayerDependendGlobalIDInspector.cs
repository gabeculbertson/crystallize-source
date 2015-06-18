using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerDependentWorldObjectComponent))]
public class PlayerDependendGlobalIDInspector : Editor {

    public override void OnInspectorGUI() {
        var pdGID = (PlayerDependentWorldObjectComponent)target;
        for (int i = 0; i < pdGID.PlayerCount; i++) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            WorldObjectInspector.DrawGlobalID(pdGID,
                (id) => pdGID.SetID(i, id),
                () => pdGID.GetNewID(i),
                () => pdGID.GetID(i));
            EditorGUILayout.EndVertical();
        }
    }
}
