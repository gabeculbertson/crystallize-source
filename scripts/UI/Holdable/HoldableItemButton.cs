using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HoldableItemButton : MonoBehaviour, IPointerClickHandler {

    GameObject parentUI;
    string holdableID;

    public void Initiallize(GameObject parentUI, string holdableID) {
        this.parentUI = parentUI;
        this.holdableID = holdableID;
        GetComponentInChildren<Text>().text = holdableID;
    }

    #region IPointerClickHandler implementation
    public void OnPointerClick(PointerEventData eventData) {
        PlayerManager.Instance.PlayerGameObject.GetComponent<ArmAnimationController>().HoldItem(holdableID);
        Destroy(parentUI);
    }
    #endregion

}
