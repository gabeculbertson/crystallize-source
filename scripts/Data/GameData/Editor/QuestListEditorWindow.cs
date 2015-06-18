using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestListEditorWindow : EditorWindow {

	[MenuItem("Crystallize/Quest list")]
    public static void Open() {
        var window = GetWindow<QuestListEditorWindow>();
        window.Initialize();
    }

    Vector2 scroll;
    List<QuestInfoGameData> quests = new List<QuestInfoGameData>();

    void Initialize() {
        quests = (from q in GameData.Instance.QuestData.Quests.Items orderby q.Description select q).ToList();
    }

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var quest in quests){
            var close = false;
            DrawQuest(quest, out close);
            if (close) {
                GameData.Instance.QuestData.Quests.RemoveItem(quest.QuestID);
                quests.Remove(quest);
                break;
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawQuest(QuestInfoGameData quest, out bool close) {
        close = false;
        
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(quest.QuestID.ToString());
        if (GUILayout.Button("x", GUILayout.Width(24f))) {
            close = true;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField(quest.Title);
        EditorGUILayout.LabelField(quest.Description);

        EditorGUILayout.EndVertical();
    }

}
