using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class QuestInfoEditorWindow : EditorWindow {

	public static void Open(QuestInfoGameData quest){
		if (quest == null) {
			Debug.LogWarning("Cannot open window with null quest.");
			return;
		}

		var window = GetWindow<QuestInfoEditorWindow> ();

        window.Initialize(quest);
	}

	QuestInfoGameData quest;
	List<Type> questInfoTypes = new List<Type>();
	string[] questInfoTypeStrings = new string[0];

    List<Type> rewardTypes = new List<Type>();
	string[] rewardTypeStrings = new string[0];

    List<Type> prereqTypes = new List<Type>();
    string[] prereqTypeStrings = new string[0];

	List<PropertyInfo> properties = new List<PropertyInfo>();
    //Dictionary<QuestReward, List<PropertyInfo>> rewardProperties = new Dictionary<QuestReward, List<PropertyInfo>>();

    void Initialize(QuestInfoGameData quest) {
        this.quest = quest;
		this.questInfoTypes = (from t in Assembly.GetAssembly (typeof(QuestInfoGameData)).GetTypes ()
		                         where typeof(QuestInfoGameData).IsAssignableFrom (t) select t).ToList ();
		this.questInfoTypeStrings = (from t in this.questInfoTypes select t.ToString ()).ToArray ();

        this.rewardTypes = (from t in Assembly.GetAssembly (typeof(QuestReward)).GetTypes ()
		                    where typeof(QuestReward).IsAssignableFrom (t) select t).ToList ();
        var rewardStrings = (from t in rewardTypes select t.ToString ()).ToList ();
        rewardTypes.Insert(0, null);
        rewardStrings.Insert(0, "Choose reward type...");
        rewardTypeStrings = rewardStrings.ToArray();

        this.prereqTypes = (from t in Assembly.GetAssembly(typeof(StatePrerequisite)).GetTypes()
                            where typeof(StatePrerequisite).IsAssignableFrom(t) && t.GetType() != typeof(StatePrerequisite)
                            select t).ToList();
        var prereqStrings = (from t in prereqTypes select t.ToString()).ToList();
        prereqTypes.Insert(0, null);
        prereqStrings.Insert(0, "Choose prerequisite type...");
        prereqTypeStrings = prereqStrings.ToArray();

        UpdatePropertyList();
    }

	void OnGUI(){
		EditorGUILayout.BeginVertical ();
		
		int selected = questInfoTypes.IndexOf (quest.GetType());
		selected = EditorGUILayout.Popup ("Quest type", selected, questInfoTypeStrings);
		if (questInfoTypes [selected] != quest.GetType ()) {
			var instance = (QuestInfoGameData)Activator.CreateInstance(questInfoTypes[selected]);
			instance.WorldID = quest.WorldID;
			instance.QuestID = quest.QuestID;
			instance.Description = quest.Description;
            instance.QuestPromptLine = quest.QuestPromptLine;
			quest = instance;
			GameData.Instance.QuestData.Quests.UpdateItem(quest);
            Close();
            Open(quest);
            return;
		}

		EditorGUILayout.LabelField ("WorldID: " + quest.WorldID + "\tQuestID: " + quest.QuestID);

		quest.Title = EditorGUILayout.TextField ("Title", quest.Title);
		quest.Description = EditorGUILayout.TextField ("Description", quest.Description);
        EditorUtilities.DrawNPCLine(quest.QuestPromptLine);

		foreach (var p in properties) {
            EditorUtilities.DrawProperty(quest, p);
		}

		foreach (var obj in quest.GetObjectives()) {
            if (!DrawObjectiveBox(obj)) {
                quest.Objectives.Remove(obj);
                break;
            }
		}

        while (quest.Objectives.Count < quest.GetDefaultObjectives().Count) {
            quest.Objectives.Add(new QuestObjectiveInfoGameData(""));
        }

        EditorGUILayout.Space();

        foreach (var r in quest.Prerequisites) {
            if (!DrawPrerequisite(r)) {
                quest.Prerequisites.Remove(r);
                break;
            }
        }

        var selectedPrereq = EditorGUILayout.Popup(0, prereqTypeStrings);
        if (selectedPrereq != 0) {
            var newPrereq = (StatePrerequisite)Activator.CreateInstance(prereqTypes[selectedPrereq]);
            quest.Prerequisites.Add(newPrereq);
        }

        EditorGUILayout.Space();

        foreach (var r in quest.Rewards) {
            DrawReward(r);
        }

        var selectedReward = EditorGUILayout.Popup (0, rewardTypeStrings);
        if (selectedReward != 0) {
            var newReward = (QuestReward)Activator.CreateInstance(rewardTypes[selectedReward]);
            quest.Rewards.Add(newReward);
        }
	
		EditorGUILayout.EndVertical ();
	}

	bool DrawObjectiveBox(QuestObjectiveInfoGameData obj){
		EditorGUILayout.BeginVertical (GUI.skin.box);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Objective");
        if (GUILayout.Button("x", GUILayout.Width(40f))) {
            return false;
        }
        EditorGUILayout.EndHorizontal();

		obj.Description = EditorGUILayout.TextField ("Description", obj.Description);
		obj.LocationType = (ObjectiveLocationType)EditorGUILayout.EnumPopup (obj.LocationType);

		EditorGUILayout.EndVertical ();
        return true;
	}

	void UpdatePropertyList(){
		properties.Clear ();
		var baseProperties = new HashSet<string> ();
		foreach (var p in typeof(QuestInfoGameData).GetProperties()) {
			baseProperties.Add (p.Name);
		}

        foreach (var p in quest.GetType().GetProperties()) {
            if (baseProperties.Contains(p.Name)) {
                continue;
            }

            //Debug.Log(p + "; " + p.PropertyType);

            if (!p.CanWrite) {
                continue;
            }

            properties.Add(p);
        }
	}

    bool DrawPrerequisite(StatePrerequisite prerequisite) {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(prerequisite.GetType().ToString());
        if (GUILayout.Button("x", GUILayout.Width(30f))) {
            return false;
        }
        EditorGUILayout.EndHorizontal();
        EditorUtilities.DrawObject(prerequisite);
        EditorGUILayout.EndVertical();
        return true;
    }

    void DrawReward(QuestReward reward) {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField(reward.GetRewardDescription());
        foreach (var p in reward.GetType().GetProperties()) {
            if (!p.CanWrite) {
                continue;
            }

            EditorUtilities.DrawProperty(reward, p);
        }

        EditorGUILayout.EndVertical();
    }

}
