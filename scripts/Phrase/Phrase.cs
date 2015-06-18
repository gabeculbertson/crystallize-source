using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Phrase made up of multiple phrase segments
/// Essentially, this allows us to create a tree structure for sentences.
/// </summary>
public class Phrase : PhraseSegmentData {

	public static Phrase GetPhraseFromSequence(PhraseSequence sequence){
		var phrase = ScriptableObject.CreateInstance<Phrase> ();
		phrase.SetTranslation (sequence.Translation);
		foreach (var ele in sequence.GetElements()) {
			var word = PhraseSegmentData.GetWordInstance(ele.WordID, ele.FormID, ele.GetKanaText(), ele.GetKanaText(), ele.GetPhraseCategory());
			phrase.phraseSegments.Add(word);
		}
		return phrase;
	}

	public static Phrase GetPhraseFromSequence(PhraseSequence sequence, ContextData context){
		var phrase = ScriptableObject.CreateInstance<Phrase> ();
		phrase.SetTranslation (sequence.Translation);
		foreach (var ele in sequence.GetElements()) {
            if (ele.ElementType == PhraseSequenceElementType.TaggedSlot) {
                var cd = context.GetElement("tag." + ele.Text);
                // TODO: need to change this if we allow phrases
                var word = PhraseSegmentData.GetWordInstance(cd.Data.PhraseElements[0]);
                phrase.phraseSegments.Add(word);
            } else {
                var word = PhraseSegmentData.GetWordInstance(ele.WordID, ele.FormID, ele.GetKanaText(context), ele.GetKanaText(context), ele.GetPhraseCategory());
                phrase.phraseSegments.Add(word);
            }
		}
		return phrase;
	}

	public List<PhraseSegmentData> phraseSegments = new List<PhraseSegmentData>();

	/// <summary>
	/// TODO: cache this
	/// </summary>
	/// <value>The text.</value>
	public override string Text {
		get {
			var s = "";
			foreach(var ps in phraseSegments){
				if(ps == null){
					Debug.Log(name);
				}
				s += ps.Text;
			}
			return s;
		}
	}

	public override PhraseSegmentData[] ChildPhrases {
		get{
			return phraseSegments.ToArray();
		}
	}

}
