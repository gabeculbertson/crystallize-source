using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Util.Serialization;

public class PlayerManager : MonoBehaviour {

    static PlayerManager _instance;
	public static PlayerManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return _instance;
        }
    }

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
		PlayerCount = 1;
        OtherPlayerLevelID = -1;
	}

	void Start(){
        PlayerData.Instance.LevelData.SetLevelState (Application.loadedLevelName, LevelState.Unlocked);
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

    void Update() {
        if (previousPlayerLevel != PlayerData.Instance.InventoryState.Level) {
            previousPlayerLevel = PlayerData.Instance.InventoryState.Level;

            if (OnLevelChanged != null) {
                OnLevelChanged(this, EventArgs.Empty);
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
