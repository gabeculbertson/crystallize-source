using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ConversationClient))]
public class ConversationClientInspector : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

	}

}
