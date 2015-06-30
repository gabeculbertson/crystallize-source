using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;   
using System.Linq;

public class PhraseSetEditorWindow : EditorWindow {

    [MenuItem("Crystallize/Game Data/Phrase sets")]
    public static void Open() {
        var window = GetWindow<PhraseSetEditorWindow>();
        window.Initialize();
    }

    string[] setNames;
    Vector2 scroll;
    string filterString = "";
    PhraseSetGameData target;
    bool showSets = true;

    void Initialize() {
        setNames = GameDataInitializer.phraseSets.Keys.ToArray();
        //Debug.Log(setNames.Length);
        //setNames = GameData.Instance.PhraseSets.Items.Select((ps) => ps.Name).ToArray();
    }

    void OnGUI() {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        filterString = EditorGUILayout.TextField("Filter", filterString);
        if (showSets) {
            var filtered = (from n in setNames
                            where n.ToLower().Contains(filterString.ToLower())
                            orderby n
                            select n);

            foreach (var n in filtered) {
                //Debug.Log(n);
                if (GUILayout.Button(n)) {
                    target = PhraseSetCollectionGameData.GetOrCreateItem(n);
                    filterString = "";
                }
            }
        } 

        if (target != null) {
            DrawPhraseSet(target);
        }

        EditorGUILayout.EndScrollView();

        if (Event.current.type == EventType.Repaint) {
            if (filterString == "" && target == null) {
                showSets = true;
            } else if (filterString != "") {
                showSets = true;
            } else {
                //Debug.Log(filterString.Length + "; " + (target == null));
                showSets = false;
            }
        }
    }

    void DrawPhraseSet(PhraseSetGameData phraseSet) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(phraseSet.Name);

        var keys = GameDataInitializer.phraseSets[phraseSet.Name];
        for (var i = 0; i < keys.Count; i++){
            var p = phraseSet.GetOrCreatePhrase(i);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("[" + i + "]", GUILayout.Width(24f));
            EditorGUILayout.LabelField(keys[i], GUILayout.Width(100f));
            EditorUtilities.DrawPhraseSequence(p);

            EditorGUILayout.EndHorizontal();
        }
        
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }


}