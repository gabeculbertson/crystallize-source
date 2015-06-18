using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveManager : MonoBehaviour {

	public static ObjectiveManager main { get; set; }

	public List<PhraseSegmentData> phrases;
	public List<PhraseSegmentData> presolvedPhrases = new List<PhraseSegmentData>();

	public bool Initialized { get; set; }

	public bool IsComplete {
		get{
			if(requiredObjectives.Count == 0){
				return false;
			}

			foreach(var kv in requiredObjectives){
				if(!kv.Value){
					return false;
				}
			}

			return true;
		}
	}

	public bool UseQuesting { get; set; }

	public IEnumerable<PhraseSegmentData> FoundWords {
		get {
			return (from w in PlayerManager.main.playerData.WordStorage.FoundWordIDs 
			        select ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(w));
		}
	}

	Dictionary<PhraseSegmentData, bool> solvedPhrases = new Dictionary<PhraseSegmentData, bool>();

	Dictionary<object, bool> requiredObjectives = new Dictionary<object, bool>();

	public event EventHandler OnLevelComplete;
	public event EventHandler<PhraseEventArgs> OnWordUnlocked;

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {
		if (ObjectiveSet.main) {
			phrases = ObjectiveSet.main.phrases;
			presolvedPhrases = ObjectiveSet.main.presolvedPhrases;
		}

		if (LevelSettings.main) {
			UseQuesting = LevelSettings.main.useQuesting;
		}

		Initialized = true;
	}

	public void SetData(List<PhraseSegmentData> phrases, List<PhraseSegmentData> presolvedPhrases){
		
	}

	public void SetObjective(object source, bool state){
		requiredObjectives [source] = state;

		if (state) {
			Debug.Log("Objective completed: " + source);
		}

		if (IsComplete){// && !UseQuesting) {
			if(OnLevelComplete != null){
				OnLevelComplete(this, EventArgs.Empty);
			}
		}
	}

	public bool GetObjective(object source){
		if (requiredObjectives.ContainsKey (source)) {
			return requiredObjectives[source];
		}
		Debug.LogWarning ("Objective not found!");
		return true;
	}

	public bool IsWordFound(PhraseSegmentData phrase){
		return PlayerManager.main.playerData.WordStorage.ContainsFoundWord (phrase);
	}

    public bool IsWordFound(int wordID) {
        return PlayerManager.main.playerData.WordStorage.ContainsFoundWord(wordID);
    }

	public void AddFoundWord(GameObject sender, PhraseSegmentData phrase){
		if (PlayerManager.main.playerData.WordStorage.AddFoundWord (phrase)){
			CrystallizeEventManager.PlayerState.RaiseOnWordFound (this, new PhraseEventArgs (phrase));
		}
		CrystallizeEventManager.UI.RaiseUpdateUI (this, EventArgs.Empty);
	}

    public void AddFoundWord(GameObject sender, PhraseSequenceElement word) {
        if (PlayerManager.main.playerData.WordStorage.AddFoundWord(word.WordID)) {
            //PlayerManager.main.playerData.WordStorage.InventoryElements.Add(word);
            CrystallizeEventManager.PlayerState.RaiseOnWordFound(this, new PhraseEventArgs(word));
        }
        CrystallizeEventManager.UI.RaiseUpdateUI(this, EventArgs.Empty);
    }

	public void AddUnlockedWord(PhraseSegmentData phrase){
		if (OnWordUnlocked != null) {
			OnWordUnlocked(this, new PhraseEventArgs(phrase));
		}
	}

	public void SetPhraseSolved(PhraseSegmentData phrase){
		solvedPhrases [phrase] = true;

		if (IsComplete) {
			foreach(var p in phrases){
				PlayerManager.main.playerData.WordStorage.AddUnlockedWord(p);
				Debug.Log(Application.loadedLevel);

				PlayerManager.main.playerData.LevelData.SetLevelState(Application.loadedLevelName, LevelState.Played);
				for(int i = 0; i < PlayerManager.main.playerData.LevelData.Levels.Count - 1; i++){
					var level = PlayerManager.main.playerData.LevelData.Levels[i];
					if(level.LevelName == Application.loadedLevelName){
						var nextLevel = PlayerManager.main.playerData.LevelData.Levels[i+1];
						if(nextLevel.LevelState == LevelState.Hidden){
							nextLevel.LevelState = LevelState.Locked;
						}
					}
				}

				PlayerManager.main.Save();
			}
		}
	}

	public bool GetPhraseSolved(PhraseSegmentData phrase){
		if(!solvedPhrases.ContainsKey(phrase)){
			return false;
		}
		return solvedPhrases [phrase];
	}

	public bool IsPresolved(PhraseSegmentData phrase){
		if (phrase.Category == PhraseCategory.Particle || phrase.Category == PhraseCategory.Punctuation || phrase.Category == PhraseCategory.Unknown) {
			return true;
		}

		return presolvedPhrases.Contains(phrase);
	}

	public bool IsPresolved(PhraseSegmentData phrase, List<PhraseSegmentData> missingWords){
		if(missingWords == null){
			return IsPresolved(phrase);
		}

		if(missingWords.Count == 0){
			return IsPresolved(phrase);
		}

		return !missingWords.Contains(phrase);
	}

}
