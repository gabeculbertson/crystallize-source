using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FriendsPanelTitleUI : MonoBehaviour, IPointerClickHandler {

	public FriendsPanelUI friendsPanel;
	
	public void OnPointerClick (PointerEventData eventData)
	{
		friendsPanel.isOpen = !friendsPanel.isOpen;
	}

}
