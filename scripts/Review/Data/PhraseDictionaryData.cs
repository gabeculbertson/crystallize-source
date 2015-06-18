using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseDictionaryData : ScriptableObject {

	[SerializeField]
	List<PhraseSegmentData> phrases;
	public string path = "";

	public IEnumerable<PhraseSegmentData> Phrases {
		get {
			return phrases;
		}
	}

	public void SetPhrases(List<PhraseSegmentData> phrases){
		this.phrases = phrases;
	}

	public PhraseSegmentData GetPhraseData(string phrase){
		return (from p in phrases where p.Text == phrase select p).FirstOrDefault ();
	}

	public PhraseSegmentData GetPhraseForID(string id){
		return (from p in phrases where p.ID == id select p).FirstOrDefault ();
	}

	public bool Contains(string phrase){
		return GetPhraseData(phrase) != null;
	}

	public void AddPhrase(PhraseSegmentData phrase){
		if(!Application.isEditor){
			Debug.LogError("Can't add to dictionary during play mode!");
			return;
		}

		if (!phrases.Contains (phrase)) {
			phrases.Add(phrase);
		}
	}

	public List<PhraseSegmentData> GetDisambiguation(PhraseSegmentData word){
		if (!IsAmbiguous(word)) {
			return null;
		}

		var root = GetRootMeaning (word); //word.Translation.Split ('(')[0];
		var clarifications = (from w in phrases where root == GetRootMeaning (w) select w).ToList ();
		return clarifications;
	}

	public bool IsAmbiguous(PhraseSegmentData word){
		return word.Translation.Contains("(");
	}

	public string GetRootMeaning(PhraseSegmentData word){
		if (IsAmbiguous (word)) {
			return word.Translation.Split ('(') [0].Trim();
		} else {
			return word.Translation.Trim();
		}
	}

	public string GetClarification(PhraseSegmentData word){
		if (!IsAmbiguous (word)) {
			return "";
		}

		return word.Translation.Split ('(') [1].Replace (")", "");
	}

}
