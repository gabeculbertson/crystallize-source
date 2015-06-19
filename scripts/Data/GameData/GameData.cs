using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class GameData {

	const string FileName = "CrystallizeGameData";
	const string FileExtension = ".txt";
	const string EditorFilePath = "/crystallize/Resources/";
	const string PlayerFilePath = "/PlayerGameData/";

	static GameData _instance;

	public static GameData Instance {
		get{
			if (_instance == null) {
				LoadInstance();
			}
			return _instance;
		}
	}

	public static void LoadInstance(){
		if (Application.isEditor) {
			Debug.Log("Loading GameData. Is editor.");
			_instance = Serializer.LoadFromXml<GameData>(GetEditorDataPath());
			if(_instance == null){
				_instance = new GameData();
			}
		} else{
			Debug.Log("Loading GameData. Is player.");
			if(File.Exists(GetPlayerDataPath())){
				Debug.LogWarning("(not implemented)");
			} else {
				var text = Resources.Load<TextAsset>(FileName);
				if(text != null){
					_instance = Serializer.LoadFromXmlString<GameData>(text.text);
				} else {
					_instance = new GameData();
				}
			}
		}
	}

	public static void SaveInstance(){
		if (_instance != null) {
			if(Application.isEditor){
				Serializer.SaveToXml<GameData>(GetEditorDataPath() + ".tmp", _instance);

				if(File.Exists(GetEditorDataPath() + ".bak2")){
					File.Delete(GetEditorDataPath() + ".bak2");
				}
				
				if(File.Exists(GetEditorDataPath() + ".bak1")){
					File.Move(GetEditorDataPath() + ".bak1", GetEditorDataPath() + ".bak2");
				}
				
				if(File.Exists(GetEditorDataPath())){
					File.Move(GetEditorDataPath(), GetEditorDataPath() + ".bak1");
				}

				File.Move(GetEditorDataPath() + ".tmp", GetEditorDataPath());
			} else{
				Debug.LogWarning("Is player. (not implemented)");
			}
		}
	}

	static string GetEditorDataPath(){
		return Application.dataPath + EditorFilePath + FileName + FileExtension;
	}

	static string GetPlayerDataPath(){
		return Application.dataPath + PlayerFilePath + FileName + FileExtension;
	}


	public QuestGameData QuestData { get; set; }
	public ChallengeProgressionGameData ProgressionData { get; set; }
	public NavigationGameData NavigationData { get; set; }
	public PhraseClassGameData PhraseClassData { get; set; }
	public WorldGameData WorldData { get; set; }
	public DialogueGameData DialogueData { get; set; }
    public TradeGameData TradeData { get; set; }

	public GameData(){
		QuestData = new QuestGameData ();
		ProgressionData = new ChallengeProgressionGameData ();
		NavigationData = new NavigationGameData ();
		PhraseClassData = new PhraseClassGameData ();
		WorldData = new WorldGameData ();
		DialogueData = new DialogueGameData ();
        TradeData = new TradeGameData();
	}

}
