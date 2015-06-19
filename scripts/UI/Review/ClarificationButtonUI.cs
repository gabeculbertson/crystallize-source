using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ClarificationButtonUI : MonoBehaviour, IPointerClickHandler {

	public PhraseSegmentData Word { get; set; }
	public ClarificationPanelUI ClarificationPanel { get; set; }

	public void Initialize(ClarificationPanelUI panel, PhraseSegmentData word){
		transform.SetParent (panel.transform);
		ClarificationPanel = panel;
		Word = word;
		GetComponentInChildren<Text> ().text = word.Translation;
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		//Debug.Log ("Clicked.");
		ClarificationPanel.WordClicked (Word);
	}
	#endregion
}
