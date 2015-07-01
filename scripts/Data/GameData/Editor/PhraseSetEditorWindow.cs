using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;   
using System.Linq;

public class PhraseSetEditorWindow : EditorWindow {

    static HashSet<string> allKeys = new HashSet<string>();
    static Dictionary<string, PhraseSequence> keySequences = new Dictionary<string, PhraseSequence>();

    static void UpdateKeySequences() {
        PhraseSetCollectionGameData.LoadAll();
        var sets = PhraseSetCollectionGameData.GetPhraseSets();

        foreach (var p in PhraseSetCollectionGameData.GetOrCreateItem("Default").Phrases) {
            UpdateKeySequence(p.Translation, p);
        }

        foreach (var set in sets) {
            if(!GameDataInitializer.phraseSets.ContainsKey(set.Name)){
                continue;
            }

            var keys = GameDataInitializer.phraseSets[set.Name];
            for (int i = 0; i < set.Phrases.Count; i++) {
                if (i >= keys.Count) {
                    break;
                }
                
                if (!keySequences.ContainsKey(keys[i])) {
                    UpdateKeySequence(keys[i], set.Phrases[i]);
                }
            }
        }
    }

    static void UpdateKeySequence(string key, PhraseSequence phrase) {
        if (!phrase.IsEmpty) {
            keySequences[key] = phrase;
        } 
    }

    static void GetAllKeys() {
        PhraseSetCollectionGameData.LoadAll();
        foreach (var k in GameDataInitializer.phraseSets.Keys) {
            foreach (var s in GameDataInitializer.phraseSets[k]) {
                if (!allKeys.Contains(s)) {
                    allKeys.Add(s);
                }
            }
        }
    }

    [MenuItem("Crystallize/Game Data/Phrase sets")]
    public static void Open() {
        var window = GetWindow<PhraseSetEditorWindow>();
        window.Initialize();
    }

    List<string> setNames;
    Vector2 scroll;
    string filterString = "";
    PhraseSetGameData target;
    bool showSets = true;

	bool initialized = false;

    void Initialize() {
        if (!initialized) {
            setNames = GameDataInitializer.phraseSets.Keys.ToList();
            setNames.Add("Default");
            initialized = true;
        }

        UpdateKeySequences();
        //Debug.Log(setNames.Length);
        //setNames = GameData.Instance.PhraseSets.Items.Select((ps) => ps.Name).ToArray();
    }

    void OnGUI() {
		Initialize();

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
                    if (n == "Default") {
                        InitializeDefault();
                    }
                    filterString = "";
                }
            }
        } 

        if (target != null) {
            if (target.Name == "Default") {
                DrawDefaultPhraseSet(target);
            } else {
                DrawPhraseSet(target);
            }
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

    void InitializeDefault() {
        GetAllKeys();
        
        var pSet = PhraseSetCollectionGameData.GetOrCreateItem("Default");
        foreach (var k in allKeys) {
            if (!ContainsKey(pSet, k)) {
                PhraseSequence p = null;
                if (keySequences.ContainsKey(k)) {
                    p = new PhraseSequence(keySequences[k]);
                } else {
                    p = new PhraseSequence();
                }
                p.Translation = k;
                pSet.Phrases.Add(p);
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
            EditorGUILayout.LabelField(keys[i], GUILayout.Width(200f));
            EditorUtilities.DrawPhraseSequence(p);

            if (p.IsEmpty && keySequences.ContainsKey(keys[i])) {
                p.PhraseElements = new List<PhraseSequenceElement>(keySequences[keys[i]].PhraseElements);
            }

            if (p.Translation == null || p.Translation == "") {
                p.Translation = keys[i];
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    void DrawDefaultPhraseSet(PhraseSetGameData phraseSet) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(phraseSet.Name);

        for (var i = 0; i < phraseSet.Phrases.Count; i++) {
            var p = phraseSet.Phrases[i];
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("[" + i + "]", GUILayout.Width(24f));
            EditorGUILayout.LabelField(p.Translation, GUILayout.Width(200f));
            EditorUtilities.DrawPhraseSequence(p);
            if (GUILayout.Button("-", GUILayout.Width(16f))) {
                phraseSet.Phrases.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    bool ContainsKey(PhraseSetGameData phraseSet, string key) {
        foreach (var p in phraseSet.Phrases) {
            if (p.Translation == key) {
                return true;
            }
        }
        return false;
    }

}