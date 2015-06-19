using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class AreaMenuButtonUI : MonoBehaviour, IPointerClickHandler {

    int areaID;
    bool unlocked = false;

    public void Initialize(int areaID) {
        this.areaID = areaID;
        var a = GameData.Instance.NavigationData.Areas.GetItem(areaID);
        GetComponentInChildren<Text>().text = a.AreaName;
        unlocked = PlayerData.Instance.LevelData.GetAreaUnlocked(areaID);
        if (!unlocked) {
            GetComponent<Image>().color = Color.gray;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (unlocked) {
            AreaManager.TransitionToArea(GameData.Instance.NavigationData.Areas.GetItem(areaID));
        }
    }
}
