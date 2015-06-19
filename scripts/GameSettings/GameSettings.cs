using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class GameSettings {

    public const string InterdependenceModule = "InterdependenceModule";

    const string FileName = "CrystallizeSettings";
    const string FileExtension = ".txt";
    const string EditorFilePath = "/Settings/";
    const string PlayerFilePath = "/Settings/";

    static GameSettings _instance;

    static HashSet<string> locks = new HashSet<string>();

    public static GameSettings Instance {
        get {
            if (_instance == null) {
                LoadInstance();
            }
            return _instance;
        }
    }

    static GameSettings() {
        SetFlag(UIFlags.LockCompass, true);
    }

    public static void SetFlag(string lck, bool val) {
        if (val) {
            if (!locks.Contains(lck)) {
                locks.Add(lck);
            }
        } else {
            if (locks.Contains(lck)) {
                locks.Add(lck);
            }
        }
    }

    public static bool GetFlag(string lck) {
        return locks.Contains(lck);
    }

    public static void LoadInstance() {
        _instance = Serializer.LoadFromXml<GameSettings>(GetFilePath());
        if (_instance == null) {
            _instance = new GameSettings();
        }
        CrystallizeEventManager.OnQuit -= HandleQuit;
        CrystallizeEventManager.OnQuit += HandleQuit;
    }

    public static void SaveInstance() {
        if (_instance != null) {
            Serializer.SaveToXml<GameSettings>(GetFilePath(), _instance);
        }
    }

    static string GetFilePath() {
        var dir = Directory.GetParent(Application.dataPath);
        return dir.FullName + EditorFilePath + FileName + FileExtension;
    }

    static void HandleQuit(object sender, System.EventArgs args) {
        SaveInstance();
    }

    public string ExperimentModule { get; set; }
    public int ExperimentCondition { get; set; }

    public GameSettings() {
        ExperimentModule = "";
        ExperimentCondition = 0;
    }

}
