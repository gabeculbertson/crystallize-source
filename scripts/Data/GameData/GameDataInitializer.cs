using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrystallizeData;

public static class GameDataInitializer {

    public static Dictionary<string, List<string>> phraseSets = new Dictionary<string, List<string>>();

    static GameDataInitializer() {
        var types = (from t in Assembly.GetAssembly(typeof(StaticSerializedGameData)).GetTypes()
                     where t.IsSubclassOf(typeof(StaticSerializedGameData)) && !t.IsAbstract
                     select t);
        foreach (var t in types) {
            var obj = (StaticSerializedGameData)Activator.CreateInstance(t);
            obj.ConstructGameData();
        }
    }

    public static void AddPhrase(string setKey, string phraseKey, int index) {
        if (!phraseSets.ContainsKey(setKey)) {
            phraseSets[setKey] = new List<string>();
        }

        while (phraseSets[setKey].Count <= index) {
            phraseSets[setKey].Add("");
        }
        phraseSets[setKey][index] = phraseKey;
    }

    public static void Initialize() {}

}