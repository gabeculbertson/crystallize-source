using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WordFinderWordUI : MonoBehaviour, IPointerDownHandler {

	public PhraseSegmentData phrase;

	void Start(){
		GetComponentInChildren<Text> ().text = phrase.Text;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		UISystem.main.PhraseDragHandler.BeginDrag (phrase.GetPhraseElement(), eventData.position);
	}
	
}
