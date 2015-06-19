using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;

/// <summary>
/// Phrase segment data.
/// This is the basic class for storing parts of phrases
/// This can be any portion of a phrase: a word, a phrase or a sentence
/// </summary>
public class PhraseSegmentData : ScriptableObject {

	public static bool IsEquivalent(PhraseSegmentData p1, PhraseSegmentData p2){
		var s1 = "";
		if (p1 != null) {
			foreach (var sp in p1.ChildPhrases) {
				if (sp.Category != PhraseCategory.Punctuation) {
					s1 += sp.Text;
				}
			}
		}

		var s2 = "";
		if (p2 != null) {
			foreach (var sp in p2.ChildPhrases) {
				if (sp.Category != PhraseCategory.Punctuation) {
					s2 += sp.Text;
				}
			}
		}

		return s1 == s2;
	}

	public static PhraseSegmentData GetWordInstance(PhraseSequenceElement phraseElement){
		return PhraseSegmentData.GetWordInstance(phraseElement.WordID, phraseElement.FormID, phraseElement.GetKanaText(), phraseElement.GetKanaText(), phraseElement.GetPhraseCategory());
	}

    public PhraseSequenceElement GetPhraseElement() {
        if (WordID >= 1000000) {
            return new PhraseSequenceElement(WordID, FormID);
        } else {
            return new PhraseSequenceElement(PhraseSequenceElementType.Text, ConvertedText);
        }
    }

    public PhraseSequence GetPhraseSequence() {
        var ps = new PhraseSequence();
        foreach (var word in ChildPhrases) {
            if (word.WordID >= 1000000) {
                ps.Add(new PhraseSequenceElement(word.WordID, word.FormID));
            } else {
                ps.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, word.ConvertedText));
            }
        }
        return ps;
    }

	public static PhraseSegmentData GetWordInstance(int id, int form, string kanaText, string translation, PhraseCategory category){
		var psd = ScriptableObject.CreateInstance<PhraseSegmentData> ();
		psd.wordID = id;
        psd.FormID = form;
		psd.text = kanaText;
		psd.translation = translation;
		psd.category = category;
		return psd;
	}

	[SerializeField]
	string text = "";
	[SerializeField]
	string translation = "";
	[SerializeField]
	public int wordID = 0;
	[SerializeField]
	PhraseCategory category = PhraseCategory.Unknown;
	[SerializeField]
	GameObject prefab;
	[SerializeField]
	AudioClip audioClip;
	[SerializeField]
	AudioClip femaleAudioClip;
	[SerializeField]
	Sprite image;
	[SerializeField]
	bool locked = false;
	[SerializeField]
	bool isContructed = false;

	public virtual string Text {
		get {
			return text;
		}
	}

	public string ConvertedText {
		get {
			if (LevelSystemConstructor.main) {
				if (!LevelSystemConstructor.main.levelSystemData.useKana) {
					return KanaConverter.Instance.ConvertToRomaji(Text);
				}
			}
			return Text;
		}
	}

	public string ConvertedTextWithSpaces {
		get {
			var s = "";
			foreach(var ps in ChildPhrases){
				if(ps == null){
					Debug.Log(name);
				}
				s += KanaConverter.Instance.ConvertToRomaji(ps.Text) + " ";
			}
			return s;
		}
	}

	public virtual string Translation {
		get {
			return translation;
		}
	}

	public int WordID {
		get {
			return wordID;
		}
	}

    public int FormID { get; set; }

	public virtual PhraseCategory Category {
		get {
			return category;
		}
	}

	public virtual string ID {
		get {
			return Text + Translation;
		}
	}

	/// <summary>
	/// Get's the associated game object if there is one.
	/// This is liekly mostly for nouns, and maybe adjectives or verbs
	/// </summary>
	/// <value>The prefab.</value>
	public GameObject Prefab {
		get {
			return prefab;
		}
	}
	
	public AudioClip MaleAudioClip{
		get {
			return audioClip;
		}
	}

	public AudioClip FemaleAudioClip {
		get {
			return femaleAudioClip;
		}
	}

	public Sprite Image {
		get {
			return image;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this instance is rumor segment.
	/// </summary>
	/// <value><c>true</c> if this instance is rumor segment; otherwise, <c>false</c>.</value>
	public bool IsRumorSegment {
		get {
			return this is RumorSegmentData;
		}
	}

	/// <summary>
	/// This tells us whether this phrase segment was created by the player or not.
	/// </summary>
	/// <value><c>true</c> if this instance is constructed; otherwise, <c>false</c>.</value>
	public bool IsConstructed { 
		get {
			return isContructed;
		} set {
			isContructed = value;
		}
	}

	public virtual PhraseSegmentData[] ChildPhrases {
		get{
			return new PhraseSegmentData[] { this };
		}
	}

	/// <summary>
	/// A locked phrase cannot be added to the player's inventory.
	/// This may be useful for reducing inventory clutter.
	/// </summary>
	/// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
	public bool Locked { 
		get{
			return locked;
		}
	}

	public void Initialize(string text){
		this.text = text;
	}

	public void SetTranslation(string translation){
		if (!Application.isEditor) {
			Debug.LogError("Must be in editor to set!");
			return;
		}
		this.translation = translation;
	}

	public void SetCategory(PhraseCategory category){
		if (!Application.isEditor) {
			Debug.LogError("Must be in editor to set!");
			return;
		}
		this.category = category;
	}

}
//}