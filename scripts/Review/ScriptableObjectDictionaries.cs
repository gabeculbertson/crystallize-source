using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScriptableObjectDictionaries : MonoBehaviour {

	static ScriptableObjectDictionaries _main;

	public static ScriptableObjectDictionaries main { 
		get {
			if(!_main){
				_main = GameObject.FindObjectOfType<ScriptableObjectDictionaries>();
			}
			return _main;
		}
	}

	public PhraseDictionaryData phraseDictionaryData;
	public ConversationDictionary conversationDictionary;
	public ConversationClientDictionary clientDictionary;
	public LevelInformationDictionary levelDictionary;
	public HoldableDictionary holdableDictionary;
    public ItemResourceDictionary itemResourceDictionary;
    public PhraseAudioResources audioResources;

	void Awake(){
		_main = this;
	}

	public PhraseSegmentData GetPhraseData(string word){
		return phraseDictionaryData.GetPhraseData (word);//(from p in phraseDictionaryData.phrases where p.Text == word select p).FirstOrDefault ();
	}

	public string GetDefinition(string word){
		return GetPhraseData (word).Translation;//(from p in phraseDictionaryData.phrases where p.Text == word select p.Translation).FirstOrDefault ();
	}

}
