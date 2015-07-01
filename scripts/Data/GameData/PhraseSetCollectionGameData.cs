using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class PhraseSetCollectionGameData {

    const string FileExtension = ".txt";
    const string ResourcePath = "PhraseSets/";
    const string EditorFilePath = "/crystallize/Resources/" + ResourcePath;

    static Dictionary<string, PhraseSetGameData> instances = new Dictionary<string, PhraseSetGameData>();

    static string GetResourcePath(string file) {
        return ResourcePath + file;
    }

    static string GetEditorDirectory() {
        return Application.dataPath + EditorFilePath;
    }

    static string GetEditorDataPath(string file) {
        return Application.dataPath + EditorFilePath + file + FileExtension;
    }

    public static void SaveAll() {
        foreach (var item in instances.Keys) {
            SaveItem(item);
        }
    }

    public static void LoadAll() {
        //Debug.Log(GetEditorDirectory());
        foreach (var f in Directory.GetFiles(GetEditorDirectory(), "*.txt")) {
            var name = Path.GetFileNameWithoutExtension(f);
            if (!instances.ContainsKey(name)) {
                instances[name] = LoadItem(name);
            }
            //Debug.Log(name);
        }
    }

    public static IEnumerable<PhraseSetGameData> GetPhraseSets() {
        return instances.Values;
    }

    static void SaveItem(string key) {
        if (instances.ContainsKey(key)) {
            if (Application.isEditor) {
                Serializer.SaveToXml<PhraseSetGameData>(GetEditorDataPath(key), instances[key]);
            } else {
                Debug.LogWarning("Is player. (not implemented)");
            }
        }
    }

    static PhraseSetGameData LoadItem(string file) {
        PhraseSetGameData data = null;
        if (Application.isEditor) {
            data = Serializer.LoadFromXml<PhraseSetGameData>(GetEditorDataPath(file));
        } else {
            var text = Resources.Load<TextAsset>(GetResourcePath(file));
            if (text != null) {
                data = Serializer.LoadFromXmlString<PhraseSetGameData>(text.text);
            } 
        }
        return data;
    }

    public static PhraseSetGameData GetOrCreateItem(string key) {
        if (!instances.ContainsKey(key)) {
            instances[key] = LoadItem(key);
            if (instances[key] == null) {
                instances[key] = new PhraseSetGameData(key);
            }
        }
        return instances[key];
    }

}