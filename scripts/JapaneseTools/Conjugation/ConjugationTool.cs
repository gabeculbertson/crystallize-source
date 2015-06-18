using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JapaneseTools {
	public enum JapaneseScriptType {
		Kanji,
		Kana,
		Romaji
	}

	public class ConjugationTool {

		const int PlainPresentIndicative = 100;
		const int PolitePresentIndicative = 110;
		const int PlainPresentNegative = 120;
		const int PolitePresentNegative = 130;
		const int Want = 200;

        const int DefaultVerbForm = PolitePresentIndicative;

		static Dictionary<int, string> group1VowelEndings = new Dictionary<int, string> ();
		static Dictionary<int, string> verbEndings = new Dictionary<int, string> ();
		static Dictionary<string, Dictionary<int, string>> irregularVerbs = new Dictionary<string, Dictionary<int, string>> ();

		static ConjugationTool(){
			group1VowelEndings [PlainPresentIndicative] = "u";
			group1VowelEndings [PolitePresentIndicative] = "i";
			group1VowelEndings [PlainPresentNegative] = "a";
			group1VowelEndings [PolitePresentNegative] = "i";
			group1VowelEndings [PolitePresentNegative] = "i";


			verbEndings [PlainPresentIndicative] = "";
			verbEndings [PolitePresentIndicative] = "ます";
			verbEndings [PlainPresentNegative] = "ない";
			verbEndings [PolitePresentNegative] = "ません";
			verbEndings [Want] = "たい";

            AddIrregularVerb("為る", "する", "します", "しない", "しません");
            AddIrregularVerb("する", "する", "します", "しない", "しません");
            AddIrregularVerb("来る", "くる", "きます", "こない", "きません");
            AddIrregularVerb("だ", "だ", "です", "じゃない", "ではありません");
		}

		static void AddIrregularVerb(string key, string plPrI, string poPrI, string plPrN, string poPrN){
			var dict = new Dictionary<int, string> ();
			dict [PlainPresentIndicative] = plPrI;
			dict [PolitePresentIndicative] = poPrI;
			dict [PlainPresentNegative] = plPrN;
			dict [PolitePresentNegative] = poPrN;
			irregularVerbs [key] = dict;
		}

		public static int[] GetVerbForms(){
			return new int[] { PlainPresentIndicative, PolitePresentIndicative, PlainPresentNegative, PolitePresentNegative, Want };
		}

		public static string ConjugateGroup1Verb(string verb, int form){
			var finalKana = verb [verb.Length - 1].ToString();
			var finalRomaji = KanaConverter.Instance.ConvertToRomaji (finalKana);
			if (!group1VowelEndings.ContainsKey (form)) {
				//Debug.Log("Form not found." + form);
				return ConjugateGroup1Verb(verb, DefaultVerbForm);
			}
		
			finalKana = finalRomaji.Substring (0, finalRomaji.Length - 1) + group1VowelEndings [form];
			finalKana = KanaConverter.Instance.ConvertToHiragana (finalKana);
			return verb.Substring (0, verb.Length - 1) + finalKana + verbEndings [form];
		}

		public static string ConjugateGroup2Verb(string verb, int form){
            if (!verbEndings.ContainsKey(form)) {
                return ConjugateGroup2Verb(verb, DefaultVerbForm);
            }
			return verb.Substring (0, verb.Length - 1) + verbEndings [form];
		}

		public static string ConjugateIrregularVerb(string verb, int form){
			if (irregularVerbs.ContainsKey (verb)) {
				if(irregularVerbs[verb].ContainsKey(form)){
					return irregularVerbs[verb][form];
				} else {
					return ConjugateIrregularVerb(verb, DefaultVerbForm);
				}
			} else {
                Debug.LogWarning("Verb not found in irregular verbs!");
                return verb;
                //throw new UnityException("Verb not found in irregular verbs!");
			}
		}

		public static string GetForm(DictionaryDataEntry entry, int form, JapaneseScriptType scriptType = JapaneseScriptType.Kanji){
			if (entry == null) {
				return null;
			}

			switch (entry.PartOfSpeech) {
			case PartOfSpeech.GodanVerb:
			case PartOfSpeech.IchidanVerb:
			case PartOfSpeech.Copula:
			case PartOfSpeech.SuruVerb:
                switch (scriptType) {
                    case JapaneseScriptType.Romaji:
                        return KanaConverter.Instance.ConvertToRomaji(GetVerbForm(entry, form, scriptType));
                    default:
                        return GetVerbForm(entry, form, scriptType);
                }
				
			default:
				switch(scriptType){
				case JapaneseScriptType.Kana:
					return entry.Kana;
				case JapaneseScriptType.Romaji:
					return KanaConverter.Instance.ConvertToRomaji(entry.Kana);

				default:
					return entry.Kana;//.Kanji;
				}
			}
		}

		public static PhraseSequenceElement[] GetForms(DictionaryDataEntry entry){
			switch (entry.PartOfSpeech) {
			case PartOfSpeech.GodanVerb:
			case PartOfSpeech.IchidanVerb:
			case PartOfSpeech.Copula:
			case PartOfSpeech.SuruVerb:
				return GetVerbForms(entry);

			default:
				return new PhraseSequenceElement[]{ new PhraseSequenceElement(entry.ID, 0) };
			}
		}

		public static string GetVerbForm(DictionaryDataEntry entry, int form, JapaneseScriptType scriptType){
			var text = entry.Kana;
            switch (scriptType) {
            case JapaneseScriptType.Kanji:
                text = entry.Kanji;
                break;
            }

			switch (entry.PartOfSpeech) {
			case PartOfSpeech.GodanVerb:
				return ConjugateGroup1Verb (text, form);
				
			case PartOfSpeech.IchidanVerb:
				return ConjugateGroup2Verb (text, form);
				
			case PartOfSpeech.Copula:
			case PartOfSpeech.SuruVerb:
				return ConjugateIrregularVerb (text, form);
			}
			return null;
		}

		public static PhraseSequenceElement[] GetVerbForms(DictionaryDataEntry entry){
			var forms = GetVerbForms ();
			var arr = new PhraseSequenceElement[forms.Length];
			for (int i = 0; i < forms.Length; i++) {
				arr [i] = new PhraseSequenceElement(entry.ID, forms[i]);
			}
			return arr;
		}
	}
}