using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TimeButtonUI : MonoBehaviour, IPointerUpHandler {

	public float timeToAdd = 1f;

	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
		ReviewManager.main.AddSimulatedTime (timeToAdd);
	}

	#endregion
}
