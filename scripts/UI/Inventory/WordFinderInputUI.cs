using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JapaneseTools;

public class WordFinderInputUI : MonoBehaviour {

	public GameObject wordPrefab;
	public Transform foundWordsContainer;
	public Text inputString;

	Dictionary<string, PhraseSegmentData> romajiDictionary = new Dictionary<string, PhraseSegmentData> ();
	Queue<GameObject> wordInstances = new Queue<GameObject>();
	string lastString;

	// Use this for initialization
	void Start () {
		foreach (var phrase in ScriptableObjectDictionaries.main.phraseDictionaryData.Phrases) {
			romajiDictionary[KanaConverter.Instance.ConvertToRomaji(phrase.Text)] = phrase;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(inputString.text != lastString){
			lastString = inputString.text;
			GetAllWords(lastString);
		}

		if (GetComponent<InputField> ().isFocused) {
			PlayerController.LockMovement (this);
		} else {
			PlayerController.UnlockMovement (this);
		}
	}

	void GetAllWords(string text){
		while (wordInstances.Count > 0) {
			Destroy(wordInstances.Dequeue());
		}

		if(text != "" && text != null){
			var selectedPhrases = (from kv in romajiDictionary 
			                       where kv.Key.Substring(0, Mathf.Min(kv.Key.Length, text.Length)) == text 
			                       orderby kv.Key.Length ascending
			                       select kv.Value);
			foreach(var p in selectedPhrases){
				var go = Instantiate(wordPrefab) as GameObject;
				go.GetComponent<WordFinderWordUI>().phrase = p;
				go.transform.SetParent(foundWordsContainer);
				wordInstances.Enqueue(go);
			}
		}
	}
}
