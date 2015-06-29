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
        phraseSets["tmp"] = new List<string>();
        phraseSets["tmp"].Add("Hey there");
        phraseSets["tmp"].Add("What's up?");

        var types = (from t in Assembly.GetAssembly(typeof(StaticSerializedGameData)).GetTypes()
                     where t.IsSubclassOf(typeof(StaticSerializedGameData)) && !t.IsAbstract
                     select t);
        foreach (var t in types) {
            var obj = (StaticSerializedGameData)Activator.CreateInstance(t);
            obj.ConstructGameData();
        }
    }

    public static void AddPhrase(string setKey, string phraseKey) {
        if (!phraseSets.ContainsKey(setKey)) {
            phraseSets[setKey] = new List<string>();
        }
        phraseSets[setKey].Add(phraseKey);
    }

    public static void Initialize() {}

}