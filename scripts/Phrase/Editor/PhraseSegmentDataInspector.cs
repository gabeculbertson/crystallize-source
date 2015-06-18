using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PhraseSegmentData))]
public class PhraseSegmentDataInspector : Editor {

	void OnEnable(){
		p = new PhraseSequence ();
	}

	PhraseSequence p = new PhraseSequence();

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		var ps = (PhraseSegmentData)target;

		if (p.GetElements ().Count > 0) {
			ps.wordID = p.GetElements()[0].WordID;	
			EditorUtility.SetDirty(ps);
		}

		if(GUILayout.Button("Get ID word")){
			PhraseEditorWindow.Open(p);
			//var l = DictionaryData.Instance.GetEntriesFromKana(ps.Text);
			//if(l.Count > 1){
			//	DictionaryEntrySelectionWindow.Open(l);
			//}
		}
	}

}
