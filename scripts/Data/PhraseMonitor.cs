using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// This class collects all of the words and sentences that the player encounters durng the game. No longer being used.
/// </summary>
[System.Obsolete]
public class PhraseMonitor {

	const string SentenceFile = "sentences.txt";
	const string WordFile = "words.txt";

	static PhraseMonitor _instance;

	public static PhraseMonitor Instance { 
		get {
			if(_instance == null){
				_instance = new PhraseMonitor();
			}
			return _instance;
		}
	}

	public void LogSentence(string speaker, string sentence){
		PlayerManager.main.LogSentence (speaker, sentence);
	}

	public void LogWord(string word){
		PlayerManager.main.LogWord (word);
	}

}
