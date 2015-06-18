using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Util.Serialization;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager main { get; set; }

	public PlayerData playerData;

	public event EventHandler OnLevelChanged;

	int previousPlayerLevel = 0;
	PhraseSegmentData targetPhrase;
    GameObject _playerGameObject;

	public PlayerState State { get; set; }

    public GameObject PlayerGameObject {
        get {
            if (!_playerGameObject) {
                _playerGameObject = GameObject.FindGameObjectWithTag("Player");
            }
            return _playerGameObject;
        }
        set {
            _playerGameObject = value;
        }
    }

    public GameObject OtherGameObject {
        get {
            if (PlayerID == 0) {
                return GameObject.FindGameObjectWithTag("OtherPlayer");
            } else {
                return GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    public int OtherPlayerLevelID { get; set; }

	public int PlayerCount { get; set; }

    public int PlayerID {
        get {
			if(!PlayerGameObject){
				return -1;
			}

            if (PlayerGameObject.CompareTag("Player")) {
                return 0;
            }
            return 1;
        }
    }

	public PhraseSegmentData TargetPhrase { 
		get {
			return targetPhrase;
		} set {
			if(targetPhrase != value){
				targetPhrase = value;
			}
		}
	}

	void Awake (){
		main = this;
		PlayerCount = 1;
		playerData = new PlayerData ();
        OtherPlayerLevelID = -1;
	}

	void Start(){
		if (File.Exists (Application.dataPath + "TempPlayerData.xml")) {
			Load();
		}
		playerData.LevelData.SetLevelState (Application.loadedLevelName, LevelState.Unlocked);
		//Debug.Log(playerData.LevelData.GetLevelState(Application.loadedLevelName));

		CrystallizeEventManager.UI.OnInteractiveDialogueOpened += HandleOnInteractiveDialogueOpened;
		CrystallizeEventManager.UI.OnInteractiveDialogueClosed += HandleOnInteractiveDialogueClosed;
	}

	void HandleOnInteractiveDialogueOpened (object sender, EventArgs e)
	{
		State = PlayerState.Conversation;
	}

	void HandleOnInteractiveDialogueClosed (object sender, EventArgs e)
	{
		State = PlayerState.Exploring;
	}

	void Update(){
		if (previousPlayerLevel != playerData.InventoryState.Level) {
			previousPlayerLevel = playerData.InventoryState.Level;

			if(OnLevelChanged != null){
				OnLevelChanged(this, EventArgs.Empty);
			}
		}
	}

	public void Save(){
		SavePlayerData(playerData, Application.dataPath + "TempPlayerData.xml");
	}

	public void Load(){
		LoadPlayerData(Application.dataPath + "TempPlayerData.xml");
	}

	public void LoadPlayerData(string filePath){
		playerData = Serializer.LoadFromXml<PlayerData> (filePath);

		if (ReviewManager.main) {
			ReviewManager.main.LoadReviewLog (playerData.ReviewLog);
		}

        CrystallizeEventManager.main.RaiseLoad(this, EventArgs.Empty);

		if (Application.isEditor) {
			LoadPhraseLog ();
		}
	}

	public void SavePlayerData(PlayerData playerData, string filePath){
        // TODO: move this somewhere else
        var areaID = AreaManager.GetCurrentAreaID();
        if (areaID != LocationPlayerData.DefaultAreaID) {
            playerData.Location.AreaID = areaID;
			PlayerData.Instance.LevelData.SetAreaUnlocked(areaID, true);
        }

        //Debug.Log(AreaManager.GetCurrentAreaID());

		if (ReviewManager.main) {
			playerData.ReviewLog = ReviewManager.main.reviewLog;
		}
		Serializer.SaveToXml<PlayerData> (filePath, playerData);

        CrystallizeEventManager.main.RaiseSave(this, EventArgs.Empty);

		if (Application.isEditor) {
			SavePhraseLog ();
		}
	}

    public void ClearData() {
        if (File.Exists(Application.dataPath + "TempPlayerData.xml")) {
            File.Delete(Application.dataPath + "TempPlayerData.xml");
        }

        playerData = new PlayerData();

        CrystallizeEventManager.main.RaiseLoad(this, EventArgs.Empty);
    }

	/****
	 * TODO: EWWWWWWW. Fix this later.
	 * */
	const string SentenceFile = "sentences.txt";
	const string WordFile = "words.txt";

	HashSet<string> sentences = new HashSet<string> ();
	HashSet<string> words = new HashSet<string> ();

	public void LogSentence(string speaker, string sentence){
		string line = speaker + ": " + sentence;
		Debug.Log ("Logging " + line);
		if (!sentences.Contains (line)) {
			sentences.Add(line);
		}
	}
	
	public void LogWord(string word){
		if (!words.Contains (word)) {
			words.Add (word);
		}
	}
	
	public void SavePhraseLog(){
		SaveSet (WordFile, words);
		SaveSet (SentenceFile, sentences);
		Debug.Log ("Saved phrase log data.");
		//_instance = null;
	}
	
	void SaveSet(string filename, HashSet<string> strings){
		if (File.Exists (filename)) {
			File.Delete(filename);
		}
		
		using (var writer = new StreamWriter(filename)) {
			var list = strings.OrderBy((s) => s);
			foreach(var item in list){
				writer.WriteLine(item);
			}
		}
	}
	
	public void LoadPhraseLog(){
		LoadSet(WordFile, words);
		LoadSet (SentenceFile, sentences);
	}
	
	void LoadSet(string filename, HashSet<string> strings){
		if (File.Exists (filename)) {
			using (var reader = new StreamReader(filename)) {
				string line;
				while ((line = reader.ReadLine()) != null) {
					//Debug.Log(line);
					strings.Add (line);
				}
			}
		}
	}

    public GameObject GetPlayerGameObject(int playerID) {
        if (playerID == 0) {
            return GameObject.FindGameObjectWithTag("Player");
        } else {
            return GameObject.FindGameObjectWithTag("OtherPlayer");
        }
    }

	public int GetPlayerID(GameObject go){
		if (go.CompareTag ("Player")) {
			return 0;
		} else if (go.CompareTag ("OtherPlayer")) {
			return 1;
		}
		return -1;
	}

}
