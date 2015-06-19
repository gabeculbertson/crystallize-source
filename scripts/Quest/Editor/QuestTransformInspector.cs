using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(QuestTransform))]
public class QuestTransformInspector : Editor {
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		
		var client = (QuestTransform)target;
		if (client.clientID == -1) {
			client.clientID = GameData.Instance.QuestData.GetNextClientID();
		}
		
		if (GUILayout.Button ("Randomize name")) {
			target.name = RandomNameGenerator.GetRandomName();
		}
	}
	
}