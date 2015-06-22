using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour {

	public RectTransform menuGroup;
	public GameObject levelButtonPrefab;
	public GameObject inventoryMenu;
	public GameObject questInfoMenu;
	public GameObject dictionaryMenu;
	public GameObject holdableMenu;
    public GameObject networking;

	List<GameObject> levelButtons = new List<GameObject>();

	GameObject menuInstance;
    // TODO: move this
    Vector3 origin;

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.UI.OnUIRequested += HandleUIRequested;
       //origin = PlayerManager.main.PlayerGameObject.transform.position;
	}

    void HandleUIRequested(object sender, UIRequestEventArgs e) {
        if (e is MainMenuUIRequestEventArgs) {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    //void Update(){
    //    if (Input.GetKeyDown (KeyCode.E)) {
    //        CloseCurrentMenu();
    //        OpenInventory();
    //    }

    //    if (Input.GetKeyDown (KeyCode.Q)) {
    //        CloseCurrentMenu();
    //        OpenQuestInfo();
    //    }
    //}

	// Update is called once per frame
	void OnEnable () {
		RefreshLevelButtons ();
	}

	void RefreshLevelButtons(){
		if (!PlayerManager.Instance) {
			return;
		}

		foreach (var levelButton in levelButtons) {
			Destroy(levelButton);
		}

		foreach (var level in ScriptableObjectDictionaries.main.levelDictionary.Levels) {
			bool isLocked = false;
            if (PlayerData.Instance.LevelData.GetLevelState(level.levelID) == LevelState.Locked) {
				isLocked = true;
			}
			//Debug.Log(level.levelID + ": " + isLocked);

			var button = Instantiate(levelButtonPrefab) as GameObject;
			button.transform.SetParent(menuGroup);
			button.GetComponent<MainMenuLevelButton>().Initialize(level, isLocked);
			levelButtons.Add(button);
		}
	}

	public void CloseApplication(){
		Debug.Log ("Closing game.");
		Application.Quit ();
	}

    public void Title() {
        Application.LoadLevel("TitleMenu");
    }

	public void Save(){
        PlayerDataLoader.Save();
        EffectManager.main.PlayMessage("Game saved!");
	}

    public void ClearData() {
        PlayerDataLoader.ClearData();
    }

	public void OpenInventory(){
		OpenMenu (inventoryMenu);
	}

	public void OpenQuestInfo(){
		OpenMenu (questInfoMenu);
	}

	public void OpenDictionary(){
		OpenMenu (dictionaryMenu);
	}

	public void ShowHoldableMenu(){
		OpenMenu (holdableMenu);
	}

    public void StartServer() {
        if (!Network.isClient && !Network.isServer) {
            Instantiate<GameObject>(networking);
            networking.GetComponent<NetworkInitializer>().StartServer();
        }
    }

    public void Reset() {
        var po = GameObject.FindGameObjectWithTag("PlayerOrigin");
        if (po != null) {
            PlayerManager.Instance.PlayerGameObject.transform.position = po.transform.position;
            PlayerManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

	void OpenMenu(GameObject menuPrefab){
		var go = Instantiate(menuPrefab) as GameObject;
		go.transform.SetParent (FieldCanvas.main.transform);//MainCanvas.main.transform);
		gameObject.SetActive (false);
		menuInstance = go;
	}

	void CloseCurrentMenu(){
		if (menuInstance) {
			Destroy(menuInstance);
		}
		menuInstance = null;
	}

}
