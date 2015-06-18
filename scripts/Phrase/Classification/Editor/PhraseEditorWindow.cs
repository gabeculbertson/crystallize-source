using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;

public class PhraseEditorWindow : EditorWindow {

	PhraseSequence phraseSequence = new PhraseSequence();
	string lastImeText = "";
	string imeText = "";

	int selected = 0;
    int cursorPosition = 0;

	[MenuItem("Crystallize/Phrase IME")]
	public static void Open(){
		//var window = 
        GetWindow<PhraseEditorWindow>();
	}

	public static void Open(PhraseSequence phraseSequence){
		var window = GetWindow<PhraseEditorWindow>();
		window.phraseSequence = phraseSequence;
	}

	//public override void OnGUI (Rect rect)
	void OnGUI()
	{
		//GUILayout.BeginArea (rect);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical();

		imeText = DrawPhrase (phraseSequence, imeText);

		if (selected >= phraseSequence.PhraseElements.Count) {
			DrawFilteredWords (phraseSequence);
		} else if (phraseSequence.PhraseElements [selected].IsDictionaryWord) {
			DrawWordForms (selected);
		} else {
			DrawSlotEditor(selected);
		}

		EditorGUILayout.EndVertical ();

		if (GUILayout.Button ("Confirm", GUILayout.Width (64f), GUILayout.ExpandHeight (false))) {
			Close();
		}

		EditorGUILayout.EndHorizontal ();
		//GUILayout.EndArea ();

		if (lastImeText != imeText) {
			selected = phraseSequence.PhraseElements.Count;
			lastImeText = imeText;
		}
	}
	
	public string DrawPhrase(PhraseSequence phrase, string imeText){
		EditorGUILayout.BeginVertical ();

		var s = phrase.Translation;
		if (s == null) {
			s = "";
		}
		s = EditorGUILayout.TextField ("Translation", s);
		if (s != "") {
			phrase.Translation = s;
		}

		EditorGUILayout.BeginHorizontal ();

		int i = 0;
        for (; i < phrase.PhraseElements.Count; i++) {
            if (cursorPosition == i) {
                imeText = EditorGUILayout.TextField(imeText);
            } else {
                if (GUILayout.Button("o", GUILayout.ExpandWidth(false))) {
                    cursorPosition = i;
                }
            }

            var word = phrase.PhraseElements[i];

            if (GUILayout.Button(word.GetText(), GUILayout.ExpandWidth(false))) {
                selected = i;
            }

            if (GUILayout.Button("x", GUILayout.ExpandWidth(false))) {
                phrase.RemoveAt(i);
                break;
            }
        }

        if(cursorPosition >= phrase.PhraseElements.Count){
            imeText = EditorGUILayout.TextField (imeText);
        } else {
            if(GUILayout.Button("o", GUILayout.ExpandWidth(false))){
			    cursorPosition = phrase.PhraseElements.Count;
            }
        }
		
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		return imeText;
	}

	void DrawWordForms(int wordIndex){
		if (wordIndex < phraseSequence.PhraseElements.Count) {
			var entry = DictionaryData.Instance.GetEntryFromID (phraseSequence.PhraseElements[wordIndex].WordID);
			foreach(var form in ConjugationTool.GetForms(entry)){
				var label = form.GetText();
				if (GUILayout.Button (label)) {
					phraseSequence.UpdateAt(selected, form);
				}
			}
		}
	}

	void DrawSlotEditor(int wordIndex){
		var tag = DrawTags (phraseSequence.PhraseElements [wordIndex].Tags);
		if (tag != null) {
			phraseSequence.PhraseElements[wordIndex].AddTag(tag);
		}
	}

	string DrawTags(List<string> tags){
		var selectionStrings = new string[GameData.Instance.PhraseClassData.Tags.Count + 1];
		selectionStrings [0] = "None";
		System.Array.Copy (GameData.Instance.PhraseClassData.Tags.ToArray (), 0, selectionStrings, 1, GameData.Instance.PhraseClassData.Tags.Count);

		foreach(var e in tags){
			if(GUILayout.Button (e)){
				tags.Remove(e);
				break;
			}
		}

		var selected = EditorGUILayout.Popup(0, selectionStrings);
		if(selected != 0){
			return selectionStrings[selected];
		}
		return null;
	}

	void DrawFilteredWords(PhraseSequence phrase){
        int prevCount = phrase.PhraseElements.Count;
        if (imeText != "") {
            PhraseSequenceElement element = null;

			if(imeText[0] == '*'){
				if(GUILayout.Button("Add slot...")){
                    element = new PhraseSequenceElement(0, 0);
                    imeText = "";
                    //phrase.Add (new PhraseSequenceElement(0, 0));
				}
			} else {
				if(GUILayout.Button("Add as plain text")){
                    element = new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText);
					//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText));
					imeText = "";
				}

				if(GUILayout.Button("Add as context data")){
                    element = new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, imeText);
					//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, imeText));
					imeText = "";
				}

                if (GUILayout.Button("Add as tag")) {
                    element = new PhraseSequenceElement(PhraseSequenceElementType.TaggedSlot, imeText);
                    //phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.TaggedSlot, imeText));
                    imeText = "";
                }

                if (GUILayout.Button("Add as wildcard")) {
                    element = new PhraseSequenceElement(PhraseSequenceElementType.Wildcard, imeText);
                    //phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Wildcard, imeText));
                    imeText = "";
                }

				foreach (var e in DictionaryData.Instance.FilterEntriesFromRomaji(imeText)) {
					var label = string.Format ("[{0}] {1} ({2}) {3}", e.ID, e.Kanji, e.Kana, e.EnglishSummary);
					if (GUILayout.Button (label)) {
                        element = new PhraseSequenceElement(e.ID, 0);
						//phrase.Add (new PhraseSequenceElement(e.ID, 0));
						imeText = "";
						break;
					}
				}

                if(element != null){
                    phrase.PhraseElements.Insert(cursorPosition, element);
                }
				
				if(GUILayout.Button("Search...")){
					var list = DictionaryData.SearchDictionaryWithStartingRomaji(imeText);
					DictionaryEntrySelectionWindow.Open(list);
				}
			}
		}

        if (prevCount != phrase.PhraseElements.Count) {
            cursorPosition = phrase.PhraseElements.Count;
        }
	}
}
