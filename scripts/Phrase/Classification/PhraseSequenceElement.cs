using UnityEngine;
using System.Collections;
using JapaneseTools;
using System.Collections.Generic;

public enum PhraseSequenceElementType {
	FixedWord = 0,
    Text = 101,
    ContextSlot = 102,
	TaggedSlot = 103,
    Wildcard = 104
}

public class PhraseSequenceElement {

    public static bool IsEqual (PhraseSequenceElement e1, PhraseSequenceElement e2, bool enforceForm = false) {
        if (e1 == null || e2 == null) {
            return e1 == e2;
        }

        if (e1.WordID >= 1000000) {
            if (enforceForm) {
                return e1.WordID == e2.WordID && e1.FormID == e2.FormID;
            } else {
                return e1.WordID == e2.WordID;
            }
        } else {
            return e1.ElementType == e2.ElementType && e1.Text == e2.Text;
        }
    }

	public int WordID { get; set; }
	public int FormID { get; set; }
	public string Text { get; set; }
	public List<string> Tags { get; set; }

	public bool IsFixedWord {
		get {
			return WordID > 100;
		}
	}

	public bool IsDictionaryWord {
		get {
			return WordID >= 1000000;
		}
	}

    public bool IsPlainText {
        get {
            return WordID == (int)PhraseSequenceElementType.Text;
        }
    }

	public PhraseSequenceElementType ElementType {
		get {
            //if (WordID == 101) {
            //    return PhraseSequenceElementType.Text;
            //}

            //if(WordID == 102){
            //    return PhraseSequenceElementType.ContextSlot;
            //}

            //if (WordID == 103) {
            //    return PhraseSequenceElementType.TaggedSlot;
            //}

            //if (WordID == 104) {
            //    return PhraseSequenceElementType.Wildcard;
            //}

			if(WordID >= 1000000){
				return PhraseSequenceElementType.FixedWord;
			} else {
                if (System.Enum.IsDefined(typeof(PhraseSequenceElementType), WordID)) {
                    return (PhraseSequenceElementType)WordID;
                }
            }

			return PhraseSequenceElementType.Text;
		}
	}

	public PhraseSequenceElement(){
		Tags = new List<string> ();
	}

	public PhraseSequenceElement(int wordID, int formID) : this(){
		WordID = wordID;
		FormID = formID;

        var dd = DictionaryData.Instance.GetEntryFromID(WordID);
        if (dd != null) {
            if (dd.HasAuxiliaryData) {
                foreach (var tag in dd.AuxiliaryData.TagIDs) {
                    Tags.Add(GameData.Instance.PhraseClassData.Tags[tag]);
                }
            }
        }
	}

	public PhraseSequenceElement(PhraseSequenceElementType type, string text) : this(){
        WordID = (int)type;
        //Debug.Log(WordID);
        //switch (type) {
            //case PhraseSequenceElementType.Text:
            //    WordID = 101;
            //    break;

            //case PhraseSequenceElementType.ContextSlot:
            //    WordID = 102;
            //    break;

            //case PhraseSequenceElementType.TaggedSlot:
            //    WordID = 103;
            //    break;

            //case PhraseSequenceElementType.Wildcard:
            //    WordID = 104;
            //    break;
        //}
		Text = text;
	}

	public string GetText(JapaneseScriptType scriptType){
        switch (ElementType) {
            case PhraseSequenceElementType.Text:
                return Text;
            case PhraseSequenceElementType.FixedWord:
                var e = DictionaryData.Instance.GetEntryFromID(WordID);
                return ConjugationTool.GetForm(e, FormID, scriptType);
            case PhraseSequenceElementType.TaggedSlot:
                /*if(TagIDs.Count > 0){
                    var s = "";
                    foreach(var t in TagIDs){
                        s += "<" + GameData.Instance.PhraseClassData.Tags[t] + "> ";
                    }
                    return s;
                } else {
                    return "<>";
                }*/
                return "<" + Text + ">";
            case PhraseSequenceElementType.ContextSlot:
                //Debug.Log(Text);
                return "[" + Text + "]";// context.GetElement(Text).Data.GetText(scriptType);

            case PhraseSequenceElementType.Wildcard:
                return "*";
        }
		return Text;
	}

	public string GetText(JapaneseScriptType scriptType, ContextData context){
        switch (ElementType) {
            case PhraseSequenceElementType.Text:
                //Debug.Log("IS TEXT");
                return Text;
            case PhraseSequenceElementType.FixedWord:
                var e = DictionaryData.Instance.GetEntryFromID(WordID);
                return ConjugationTool.GetForm(e, FormID, scriptType);
            case PhraseSequenceElementType.TaggedSlot:
                if (Tags.Count > 0) {
                    var s = "";
                    foreach (var t in Tags) {
                        s += "<" + t + "> ";
                    }
                    return s;
                } else {
                    return "<>";
                }
            case PhraseSequenceElementType.ContextSlot:
                //Debug.Log(Text);
                return context.GetElement(Text).Data.GetText(scriptType);

            case PhraseSequenceElementType.Wildcard:
                return "*";
        }
		return Text;
	}

    public string GetTranslation() {
        if(WordID < 1000000){
            return GetText();
        }
        return DictionaryData.Instance.GetEntryFromID(WordID).GetPreferredTranslation();
    }

    public string GetPlayerText() {
        return KanaConverter.Instance.ConvertToRomaji(GetKanaText());
    }

	public string GetText(){
		return GetText (JapaneseScriptType.Kanji);
	}

	public string GetKanaText(){
		return GetText (JapaneseScriptType.Kana);
	}

	public string GetKanaText(ContextData context){
		return GetText (JapaneseScriptType.Kana, context);
	}

	public PhraseCategory GetPhraseCategory(){
		if (WordID >= 1000000) {
			return DictionaryData.Instance.GetEntryFromID (WordID).PartOfSpeech.GetCategory();
		} else {
            if (Text[0] < 'A'){
                return PhraseCategory.Punctuation;
            }
			return PhraseCategory.Unknown;
		}
	}

    public void AddTag(string tag) {
        if (!Tags.Contains(tag.ToLower())) {
            Tags.Add(tag.ToLower());
        }
    }

    public bool ContainsTag(string tag) {
        var lower = tag.ToLower();
        foreach (var t in Tags) {
            if (t.ToLower() == lower) {
                return true;
            }
        }
        return false;
    }

}
