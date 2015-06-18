using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseChallenge : ScriptableObject {

	public FixedPlayerDialog dialogue;
	public List<PhraseSegmentData> givenPhrases = new List<PhraseSegmentData>();

	public string GetDisplayName(){
		return dialogue.dialogPhrases [1].Translation;
	}

}
