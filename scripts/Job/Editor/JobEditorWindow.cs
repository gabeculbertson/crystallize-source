using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobEditorWindow : GameDataDictionaryEditorWindow<JobGameData>  {

    [MenuItem("Crystallize/Game Data/Jobs")]
    static void Open() {
        GetWindow<JobEditorWindow>();
    }

    protected override DictionaryCollectionGameData<JobGameData> Dictionary {
        get { return GameData.Instance.Jobs; }
    } 

}
