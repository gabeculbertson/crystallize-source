using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveSet : MonoBehaviour {

	public static ObjectiveSet main { get; set; }

	public List<PhraseSegmentData> phrases = new List<PhraseSegmentData> ();
	public List<PhraseSegmentData> presolvedPhrases = new List<PhraseSegmentData>();

	void Awake(){
		main = this;
	}

}
