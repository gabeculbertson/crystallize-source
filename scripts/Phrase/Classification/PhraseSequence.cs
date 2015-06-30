using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;

public class PhraseSequence {

    public static bool IsPhraseEquivalent(PhraseSequence a, PhraseSequence b) {
        PhraseSequence a2 = new PhraseSequence();
        foreach (var w in a.PhraseElements) {
            if (!w.IsPlainText) {
                a2.Add(w);
            }
        }
        
        PhraseSequence b2 = new PhraseSequence();
        foreach (var w in b.PhraseElements) {
            if (!w.IsPlainText) {
                b2.Add(w);
            }
        }

        if (a2.PhraseElements.Count != b2.PhraseElements.Count) {
            return false;
        }

        for (int i = 0; i < a2.PhraseElements.Count; i++) {
            if (a2.PhraseElements[i].ElementType != b2.PhraseElements[i].ElementType) {
                return false;
            }

            switch (a2.PhraseElements[i].ElementType) {
                case PhraseSequenceElementType.FixedWord:
                    if (a2.PhraseElements[i].WordID != b2.PhraseElements[i].WordID) {
                        return false;
                    }
                    break;

                case PhraseSequenceElementType.ContextSlot:
                    if (a2.PhraseElements[i].Text != b2.PhraseElements[i].Text) {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

	public string Translation { get; set; }
	public List<PhraseSequenceElement> PhraseElements { get; set; }
    
    public bool IsWord {
        get {
            return PhraseElements.Count == 1;
        }
    }

    public PhraseSequenceElement Word {
        get {
            return PhraseElements[0];
        }
    }

    public bool IsEmpty {
        get {
            return PhraseElements.Count == 0;
        }
    }

	public PhraseSequence (){
		PhraseElements = new List<PhraseSequenceElement> ();
	}

    public PhraseSequence(string text) : this() {
        var pe = new PhraseSequenceElement(PhraseSequenceElementType.Text, text);
        Add(pe);
    }

    public PhraseSequence(PhraseSequence original)
    {
        Translation = original.Translation;
        PhraseElements = new List<PhraseSequenceElement>(original.PhraseElements);
    }

    public PhraseSequence(PhraseSequenceElement word) : this() {
        Add(word);
    }

	public List<PhraseSequenceElement> GetElements(){
		return PhraseElements;
	}

	public List<DictionaryDataEntry> GetWords(){
		var l = new List<DictionaryDataEntry> ();
		foreach (var ele in PhraseElements) {
			l.Add (DictionaryData.Instance.GetEntryFromID(ele.WordID));
		}
		return l;
	}

	public void Add( PhraseSequenceElement element){
		PhraseElements.Add (element);
	}

	public void UpdateAt(int index, PhraseSequenceElement element){
		PhraseElements.RemoveAt (index);
		PhraseElements.Insert (index, element);
	}

	public void RemoveAt(int index){
		PhraseElements.RemoveAt (index);
	}

	public string GetText(JapaneseScriptType scriptType = JapaneseScriptType.Kanji){
		var s = "";
		foreach (var e in PhraseElements) {
			s += e.GetText(scriptType) + " ";
		}
		return s;
	}

	public List<string> GetSuppliedContextData(){
		var suppliedContext = new List<string> ();
		foreach (var p in PhraseElements) {
			if(p.ElementType == PhraseSequenceElementType.ContextSlot){
				suppliedContext.Add(p.Text);
			}
		}
		return suppliedContext;
	}

    public bool FulfillsTemplate(PhraseSequence template) {
        var cleanPhrase = new PhraseSequence();
        foreach (var e in this.PhraseElements) {
            if (e.GetPhraseCategory() != PhraseCategory.Punctuation) {
                cleanPhrase.PhraseElements.Add(e);
            } 
        }

        //Debug.Log("Cleaned: " + cleanPhrase.GetText());

        if (cleanPhrase.PhraseElements.Count != template.PhraseElements.Count) {
            //Debug.Log("Count mismatch");
            return false;
        }

        for (int i = 0; i < template.PhraseElements.Count; i++) {
            if (template.PhraseElements[i].ElementType == PhraseSequenceElementType.FixedWord) {
                if (template.PhraseElements[i].WordID != cleanPhrase.PhraseElements[i].WordID) {
                    //Debug.Log("Word mismatch: " + i + "; " + template.PhraseElements[i].WordID + "; " + this.PhraseElements[i].WordID);
                    return false;
                } 
            }

            if (template.PhraseElements[i].ElementType == PhraseSequenceElementType.ContextSlot) {
                if (!cleanPhrase.PhraseElements[i].Tags.Contains(template.PhraseElements[i].Text)) {
                    //Debug.Log("Context mismatch" + i + "; " + template.PhraseElements[i].WordID + "; " + this.PhraseElements[i].WordID);
                    return false;
                }
            }

            if (template.PhraseElements[i].ElementType == PhraseSequenceElementType.TaggedSlot) {
                if (cleanPhrase.PhraseElements[i].GetPhraseCategory().ToString().ToLower() != template.PhraseElements[i].Text.ToLower()) {
                    //Debug.Log("Context mismatch" + i + "; " + template.PhraseElements[i].WordID + "; " + this.PhraseElements[i].WordID);
                    return false;
                }
            }
        }
        return true;
    }

    public PhraseSequence InsertContext(ContextData context) {
        if (context == null) {
            return this;
        }

        var p = new PhraseSequence();
        foreach (var w in PhraseElements) {
            if (w.ElementType == PhraseSequenceElementType.ContextSlot) {
                var cd = context.GetElement(w.Text);
                if (cd != null) {
                    p.Add(cd.Data.PhraseElements[0]);
                } else {
                    p.Add(w);
                }
            } else {
                p.Add(w);
            }
        }
        return p;
    }

}
