using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ConversationObjectiveUI : MonoBehaviour, IPhraseDropHandler, IPointerDownHandler {
	
	public PhraseSegmentData phrase;
	public Text translationText;
	public Image imageBackground;
	public Image image;
	public RectTransform insetArea;
	public GameObject solvedBanner;
	public GameObject completionEffectInstance;
	public GameObject completedWordBox;

	public Color unsolvedColor = Color.white;
	public Color insetColor = Color.white;
	public Color solvedColor = Color.white;
	public Color solvedImageColor = Color.yellow;

	bool solved = false;

	public bool IsComplete {
		get {
			return solved;
		}
	}

	// Use this for initialization
	void Start () {
        insetArea.GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
        solvedBanner.GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;

		solvedBanner.SetActive (false);

		if (phrase) {
			translationText.text = phrase.Translation;
			if(phrase.Image){
				image.sprite = phrase.Image;
			} else {
				image.gameObject.SetActive(false);
			}
		}

		if (PlayerManager.main.playerData.WordStorage.ContainsFoundWord (phrase)) {
			SetCorrect();
		}
	}

	bool TryPhrase(PhraseSegmentData phraseData){
		if (phraseData.Text == phrase.Text) {
			SetCorrect();
			return true;
		}
		return false;
	}

	void SetCorrect(){
		solvedBanner.SetActive (true);
		solvedBanner.GetComponentInChildren<Text> ().text = phrase.ConvertedText;
		GetComponent<Image> ().color = solvedColor;
		imageBackground.color = solvedImageColor;

        insetArea.GetComponent<Image>().sprite = EffectLibrary.Instance.conversationWordShape;
        solvedBanner.GetComponent<Image>().sprite = EffectLibrary.Instance.conversationWordShape;

		var effect = Instantiate (completionEffectInstance) as GameObject;
		effect.transform.SetParent (transform);

		solved = true;
	}
	
	public void AcceptDrop (IWordContainer phraseObject)
	{
		var entry = phraseObject.gameObject.GetComponent<InventoryEntryUI>();

		if (!entry) return;
		if (solved) return;
		if (entry.type != InventoryType.Objective) return;

		if (TryPhrase (PhraseSegmentData.GetWordInstance(phraseObject.Word))) {
			ObjectiveManager.main.AddFoundWord(gameObject, phraseObject.Word);
			Destroy(phraseObject.gameObject);
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (solved) {
			var go = UISystem.main.PhraseDragHandler.BeginDrag (phrase.GetPhraseElement(), eventData.position);
			go.GetComponent<InventoryEntryUI>().type = InventoryType.Conversation;
		}
	}

}
