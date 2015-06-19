using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MainMenuLevelButton : MonoBehaviour, IPointerClickHandler {

	string levelID;

	public void Initialize(LevelInformation levelInformation, bool isLocked){
		GetComponentInChildren<Text> ().text = levelInformation.levelName;
		levelID = levelInformation.levelID;

		if (isLocked) {
			GetComponent<Image>().color = Color.gray;
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		Application.LoadLevel (levelID);
	}

}
