using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(QuestClient))]
public class QuestClientInspector : Editor {

	static List<QuestInfo> quests;
	static string[] questNames;

	QuestClient client;
	int selectedQuest = 0;

	void OnEnable(){
		client = (QuestClient)target;
		quests = QuestInfo.GetAllQuestInfo().ToList ();
		questNames = (from q in quests select q.Name).ToArray();

		var clientQuest = QuestInfo.GetQuestInfo(client.questID);
		if (quests.Contains (clientQuest)) {
			selectedQuest = quests.IndexOf(clientQuest);
		} else{
			selectedQuest = 0;
		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		selectedQuest = EditorGUILayout.Popup ("Quest", selectedQuest, questNames);
		client.questID = quests [selectedQuest].ID;
	}

}
