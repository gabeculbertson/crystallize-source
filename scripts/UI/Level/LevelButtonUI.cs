using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelButtonUI : MonoBehaviour, IPointerClickHandler {

    public bool isArea = true;
    public string levelName;
    public string stageName;
    public string currency;
    public int cost;
    public LevelState levelState = LevelState.Locked;
    //public LevelButtonUI nextButton;

    LevelStateData stateData;

    // Use this for initialization
    void Start() {
        if (isArea) {
            stateData = PlayerData.Instance.LevelData.GetLevelStateData(levelName);
            if (stateData == null) {
                PlayerData.Instance.LevelData.SetLevelState(levelName, levelState);
                stateData = PlayerData.Instance.LevelData.GetLevelStateData(levelName);
            }

            levelState = stateData.LevelState;
            stageName = GetComponentInChildren<Text>().text;
        }
    }

    void Update() {
        if (isArea) {
            if (stateData.LevelState == LevelState.Locked) {
                GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
                GetComponentInChildren<Text>().text = currency + cost.ToString();
                GetComponent<Button>().interactable = true;
            } else if (stateData.LevelState == LevelState.Hidden) {
                GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                GetComponentInChildren<Text>().text = "???";
                GetComponent<Button>().interactable = false;
            } else {
                GetComponent<Image>().color = Color.white;
                GetComponentInChildren<Text>().text = stageName;
                GetComponent<Button>().interactable = true;
            }
        }
    }

    #region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData) {
        if (isArea) {
            var state = stateData.LevelState;
            if (state == LevelState.Locked) {
                if (PlayerData.Instance.Money >= cost) {
                    PlayerData.Instance.Money -= cost;
                    PlayerData.Instance.LevelData.SetLevelState(levelName, LevelState.Unlocked);
                    //GetComponent<Image>().color = Color.white;
                    //GetComponentInChildren<Text>().text = stageName;
                    //state = LevelState.Unlocked;
                }
            } else if (state == LevelState.Unlocked || state == LevelState.Played) {
                PlayerDataLoader.Save();
                Application.LoadLevel(levelName);
            }
        } else {
            Application.LoadLevel(levelName);
        }
    }

    #endregion
}
