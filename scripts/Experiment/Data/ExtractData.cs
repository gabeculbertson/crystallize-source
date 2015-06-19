using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ExtractData : MonoBehaviour {

	const string DirectoryName = "ExperimentLog";

	// Use this for initialization
	void Start () {
        GetDataFromFiles();
	}

	void GetDataFromFiles(){
		var dir = DirectoryName + "/";
		
        var columns = new string[]{"Study time", "Condition", "Participant", "Tutorial time", "Area 1 time", "Area 2 time", "Area 3 time", 
            "Area 4 time", "Area 5 time", "Levels completed", "Lines of chat", "Words", "Total time", "Quest count", "Correct count", "Wrong count", "Forgotten" };
        var rows = new List<object[]>();
        var files = (from f in Directory.GetFiles(dir) orderby f select f);

        int participant = 0;
        var questCounts = new int[] { 1, 1, 2, 2, 6, 10 };
        var correctPhrases = new string[] { "こんにちは", "さようなら", "がくせい です", "わたし は がくせい です", "わたし も", "がくせい です か ?", "おなまえ は ?",
                                "これ は なに です か ?", "おしごと は なに です か ?", "おなまえ は なに です か ?", "ラーメン を おねがいします", "だいじょうぶ です か ?" };

        var codeDictionary = new Dictionary<string, string>();
        codeDictionary["a"] = "Emote";
        codeDictionary["b"] = "Help request";
        codeDictionary["c"] = "Offer help";
       

        var incorrectDict = new Dictionary<string, int>();

        foreach (var file in files) {
            var row = new object[columns.Length];

            participant++;

            int chatCount = 0;
            int wordCount = 0;
            int correctCount = 0;
            int wrongCount = 0;
            int level = 0;
            string condition = "A";
            string player = "Player 1";
            var gameStart = DateTime.Now;
            var gameEnd = DateTime.Now;
            var lastLevelTime = DateTime.Now;
            var passedStart = false;
            var passedTutorial = false;

            int questCount = 0;
            int levelQuestCount = 0;
            var levelhash = new HashSet<int>();

            var chatLines = new List<string>();

            var text = GetText(file);
            foreach (var line in text) {
                var data = GetData(line);
                var ticks = long.Parse(data[0]);
                var time = new DateTime(ticks);
                var type = data[1];

                gameEnd = time;

                if (type == "Connected") {
                    if (data[2] == "Client") {
                        player = "Player 2";
                    }
                } else if (type == "Chat") {
                    if (data[2].Contains(player)) {
                        chatCount++;
                        //chatLines.Add(data[2].Replace(player + ": ", ""));
                    }
                    if(participant % 2 == 1){
                        var split = data[2].Split(":".ToCharArray(), 2);
                        var idOffset = 1;
                        if(data[2].Contains(player)){
                            idOffset = 0;
                        }

                        chatLines.Add((participant + idOffset).ToString() + "\t" + split[1].Trim());
                    }
                } else if (type == "ChangeArea") {
                    int changeToLevel = int.Parse(data[2]);
                    if (changeToLevel == 16) {
                        if (!passedStart) {
                            gameStart = new DateTime(ticks);
                            passedStart = true;
                        }
                    } else {
                        if (condition == "B" && !levelhash.Contains(changeToLevel)) {
                            row[3 + level] = (time - lastLevelTime).TotalMinutes;
                            level++;
                        }
                    }
                    if (!levelhash.Contains(changeToLevel)) {
                        lastLevelTime = time;
                        levelhash.Add(changeToLevel);
                        Debug.Log("New level: " + changeToLevel);
                    }
                } else if (type == "BeginInteraction") {
                    if (!passedTutorial) {
                        if (data[2] == "1000430" || data[2] == "1000420" || data[2] == "1000400") {
                            row[3 + level] = (time - lastLevelTime).TotalMinutes;
                            level++;
                            lastLevelTime = time;
                            Debug.Log("Tutorial");
                            passedTutorial = true;
                        }
                    }
                } else if (type == "ExperimentCondition") {
                    if (data[3] == "0") {
                        condition = "B";
                    } else {
                        condition = "A";
                    }
                } else if (type == "Found") {
                    wordCount++;
                } else if (type == "QuestComplete") {
                    if (condition == "A") {
                        levelQuestCount++;
                        Debug.Log("Level: " + (level - 1));
                        if (levelQuestCount >= questCounts[level-1]) {
                            row[3 + level] = (time - lastLevelTime).TotalMinutes;
                            level++;
                            lastLevelTime = time;
                            levelQuestCount = 0;
                        }
                    }
                    questCount++;
                } else if (IsJapanese(type)) {
                    if (!type.Contains("_")) {
                        var trimmed = type.Trim();
                        if (correctPhrases.Contains(trimmed)) {
                            correctCount++;
                        } else {
                            if (incorrectDict.ContainsKey(trimmed)) {
                                incorrectDict[trimmed]++;
                            } else {
                                incorrectDict[trimmed] = 1;
                            }
                            wrongCount++;
                        }
                    }
                }
            }

            row[0] = gameStart.ToString();// (gameEnd - gameStart).TotalMinutes;
            row[1] = condition;
            row[2] = participant;
            row[3 + level] = (gameEnd - lastLevelTime).TotalMinutes;
            row[9] = level;
            row[10] = chatCount;
            row[11] = wordCount;
            row[12] = (gameEnd - gameStart).TotalMinutes;
            row[13] = questCount;
            row[14] = correctCount;
            row[15] = wrongCount;
            rows.Add(row);

            WriteChatLog(chatLines);
            Debug.Log("Writing participant " + participant);
        }

        var orderedKeys = (from k in incorrectDict orderby k.Value select k.Key);
        foreach (var key in orderedKeys) {
            Debug.Log("Wrong: " + key + "(" + incorrectDict[key] + ")");
        }

        using (var writer = new StreamWriter("DataOutFile.txt")) {
            foreach (var c in columns) {
                writer.Write(c + "\t");
            }
            writer.WriteLine();

            foreach (var r in rows) {
                foreach (var item in r) {
                    if (item == null) {
                        writer.Write("0" + "\t");
                    } else {
                        writer.Write(item.ToString() + "\t");
                    }
                }
                writer.WriteLine();
            }
        }
	}

    string[] GetText(string file) {
        string[] text = null;
        using (var reader = new StreamReader(file)) {
            text = reader.ReadToEnd().Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
        return text;
    }

    string[] GetData(string line) {
        var s = new string[] { "", "", "", ""};
        var split = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        s[0] = split[0];
        s[1] = split[1];
        if (split.Length >= 3) {
            s[2] = split[2];
        }
        if (split.Length >= 4) {
            s[3] = split[3];
        }
        return s;
    }

    bool IsJapanese(string str) {
        foreach (var c in str) {
            if ((int)c > 500) {
                return true;
            }
        }
        return false;
    }

    void WriteChatLog(List<string> chatLines) {
        string dir = "ChatLogCoding";
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }

        using (var writer = new StreamWriter(dir + "/" + "cl" + ".txt", true)) {
            foreach (var l in chatLines) {
                writer.WriteLine("\t" + l);
            }
        }
    }

}
