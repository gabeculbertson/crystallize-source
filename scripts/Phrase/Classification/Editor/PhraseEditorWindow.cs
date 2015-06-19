using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using JapaneseTools;

public class PhraseEditorWindow : EditorWindow {

	/** IME Keyboard configuration ******************************************/
	public static int displayLines  = 10;
	public static KeyCode nextPage = KeyCode.RightArrow;
	public static KeyCode prevPage = KeyCode.LeftArrow;
	public static KeyCode nextEntry = KeyCode.DownArrow;
	public static KeyCode prevEntry = KeyCode.UpArrow;

	DictionaryDataEntry[] OnDisplay = new DictionaryDataEntry[displayLines];
	int offset = 0;
	DictionaryDataEntry[] currentList = new DictionaryDataEntry[0];
	/** END *****************************************************************/



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
		Focus ();

		//GUILayout.BeginArea (rect);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical();


		imeText = DrawPhrase (phraseSequence, imeText);

		if (selected >= phraseSequence.PhraseElements.Count) {
			//DrawFilteredWords (phraseSequence);
			DrawTenFileterWords(phraseSequence, offset);
		} 
		else if (phraseSequence.PhraseElements [selected].IsDictionaryWord) {
			DrawWordForms (selected);
		} 
		else {
			DrawSlotEditor(selected);
		}

		EditorGUILayout.EndVertical ();
		if (GUILayout.Button ("Confirm", GUILayout.Width (64f), GUILayout.ExpandHeight (false))) {
			Close();
		}

		EditorGUILayout.EndHorizontal ();

		if (lastImeText != imeText) {

		}

		//keyBorad event loop
		bool canRead = true;
		if(Event.current.type == EventType.Repaint){
			canRead = true;
		}
		if(Event.current.type == EventType.Layout){
			canRead = false;
		}
		if(canRead){
			if (Event.current.type == EventType.KeyDown) {
				//listen to key event
				KeyCode key = Event.current.keyCode;
				ListenToScroll(key);
				ListenToTyping(key);
			}
			//reset entry when typing
			if(lastImeText != imeText){
				selected = phraseSequence.PhraseElements.Count;
				lastImeText = imeText;
				offset = 0;
			}
		}
		//end of keyBoard event loop
	}

	
	//managing response to keyboard entries about inputs
	void ListenToTyping(KeyCode key){
		if (key == KeyCode.Backspace || key == KeyCode.Delete) {
			imeText = imeText.Substring(0, Mathf.Max(0, imeText.Length - 1));
		}
		if(key.ToString().Length == 1 && IsAlphabet(key.ToString()[0])){
			imeText += key.ToString().ToLower();
		}
		int index;
		if(IsNumeric(key.ToString(), out index)){
			if(index >= 0 && index < OnDisplay.Length && OnDisplay[index] != null){
				phraseSequence.PhraseElements.Insert (cursorPosition, new PhraseSequenceElement(OnDisplay[index].ID, 0));
				imeText = "";
				cursorPosition = phraseSequence.PhraseElements.Count;
			}
		}
	}

	//managing response to keyboard entries about scrolling
	void ListenToScroll(KeyCode key) {
		if(key == nextPage){
			offset = Mathf.Min(offset + displayLines, currentList.Length - displayLines);
		}
		if(key == prevPage){
			offset = Mathf.Max(0, offset - displayLines);
		}
		if(key == nextEntry){
			offset = Mathf.Min(offset + 1, currentList.Length - displayLines);
		}
		if(key == prevEntry){
			offset = Mathf.Max(0, offset - 1);
		}
	}

	//key input helpers
	bool IsAlphabet(char c){
		if (((int)c >= (int)'A' && (int)c <= (int)'Z') || ((int)c >= (int)'a' && (int)c <= (int)'z'))
			return true;
		return false;
	}

	bool IsNumeric(string s, out int index){
		if (s.Length == 6 && s.Substring (0, 5) == "Alpha") {
			int dec = (int) s[5];
			if(dec >= (int) '1' && dec <= (int) '9'){
				index = dec - (int) '1';
				return true;
			}
			else if(dec == (int) '0'){
				index = 9;
				return true;
			}
		}
		index = -1;
		return false;
	}
	//end of key input helpers

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
				//GUI.SetNextControlName("EntryField");
				EditorGUILayout.LabelField (imeText);
                //imeText = EditorGUILayout.TextField(imeText);
				if(imeText != lastImeText){
					lastImeText = imeText;
					offset = 0;
				}
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
			cursorPosition = phrase.PhraseElements.Count;
			EditorGUILayout.LabelField (imeText);
			//imeText = EditorGUILayout.TextField (imeText);
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

	/** tentative method to draw words
	 *  draw ten words on the screen
	 *  Pre: PhraseSquence phrase, int offset, the index of the first element to be drawn (starting at 0)
	 */ 
	void DrawTenFileterWords(PhraseSequence phrase, int offset){
		int prevCount = phrase.PhraseElements.Count;
		if (imeText != "") {
			PhraseSequenceElement element = null;
			
			if (imeText [0] == '*') {
				if (GUILayout.Button ("Add slot...")) {
					element = new PhraseSequenceElement (0, 0);
					imeText = "";
					//phrase.Add (new PhraseSequenceElement(0, 0));
				}
			} else {
				if (GUILayout.Button ("Add as plain text")) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.Text, imeText);
					//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.Text, imeText));
					imeText = "";
				}
				
				if (GUILayout.Button ("Add as context data")) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.ContextSlot, imeText);
					//phrase.Add (new PhraseSequenceElement(PhraseSequenceElementType.ContextSlot, imeText));
					imeText = "";
				}
				
				if (GUILayout.Button ("Add as tag")) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.TaggedSlot, imeText);
					//phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.TaggedSlot, imeText));
					imeText = "";
				}
				
				if (GUILayout.Button ("Add as wildcard")) {
					element = new PhraseSequenceElement (PhraseSequenceElementType.Wildcard, imeText);
					//phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Wildcard, imeText));
					imeText = "";
				}
				currentList = DictionaryData.Instance.FilterEntriesFromRomaji (imeText).ToArray ();
				if (offset + displayLines >= currentList.Length)
					offset = Mathf.Max (0, currentList.Length - displayLines);
				if (offset < 0)
					offset = 0;
				for (int i = 0; i < displayLines; i++) {
					if (offset + i >= currentList.Length || offset + i < 0)
						OnDisplay[i] = null;
					else{
						var e = currentList [offset + i];
						var label = string.Format ("{0} [{1}] {2} ({3}) {4}", i + 1, e.ID, e.Kanji, e.Kana, e.EnglishSummary);
						if (GUILayout.Button (label)) {
							element = new PhraseSequenceElement (e.ID, 0);
							//phrase.Add (new PhraseSequenceElement(e.ID, 0));
							imeText = "";
							break;
						}
						OnDisplay [i] = e;
					}
				}

				
				if (element != null) {
					phrase.PhraseElements.Insert (cursorPosition, element);
				}
				
				if (GUILayout.Button ("Search...")) {
					var searchResultList = DictionaryData.SearchDictionaryWithStartingRomaji (imeText);
					DictionaryEntrySelectionWindow.Open (searchResultList);
				}
			}
		} 
		
		if (prevCount != phrase.PhraseElements.Count) {
			cursorPosition = phrase.PhraseElements.Count;
		}
	}
	


}
