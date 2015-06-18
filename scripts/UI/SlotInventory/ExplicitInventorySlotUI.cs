using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class ExplicitInventorySlotUI : UIMonoBehaviour, IPhraseDropHandler, IPhraseDropEvent, IPointerEnterHandler, 
										IPointerExitHandler, IBeginDragHandler, IDragHandler, IPointerClickHandler
{

	public RectTransform slot;
	public Text translationText;

	PhraseSequenceElement word;
	bool hovering = false;
	float targetAlpha = 0;
	bool rankUpReady = false;

	public event EventHandler<PhraseEventArgs> OnPhraseDropped;

	public PhraseSequenceElement Word { 
		get {
			return word;
		}
	}

	public bool EnableRankUp { get; set; }
	public bool AllowOverride { get; set; }

	void Awake(){
		EnableRankUp = true;
		AllowOverride = LevelSettings.main.isMultiplayer;
        GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
        GetComponentInChildren<Text>().font = GUIPallet.main.defaultFont;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//RefreshSlotState ();

		slot.GetComponent<CanvasGroup> ().alpha = Mathf.MoveTowards (slot.GetComponent<CanvasGroup> ().alpha, targetAlpha, Time.deltaTime);
	}

	void RefreshSlotState(){
		if (word == null) {
			slot.GetComponent<CanvasGroup>().alpha = 0;
			targetAlpha = 0;
			translationText.text = "";
			return;
		}

        slot.GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
        translationText.text = word.GetTranslation();//.GetPlayerText();
		var phraseText = slot.GetComponentInChildren<Text> ();
        // TODO: not sure if this should be here
        if (!phraseText) {
            return;
        }

		phraseText.text = word.GetPlayerText();

		if (rankUpReady && PlayerManager.main.State == PlayerState.Exploring) {
			phraseText.color = Color.Lerp(Color.black, Color.gray, Mathf.PingPong(Time.time * 2f, 1f));
		} else {
			phraseText.color = Color.black;
		}

		if (PlayerManager.main.playerData.WordStorage.ContainsFoundWord (Word.WordID)) {
			slot.GetComponent<Image>().color = GUIPallet.main.GetColorForWordCategory(word.GetPhraseCategory());
			slot.GetComponentInChildren<Text>().font = GUIPallet.main.defaultFont;
			if(hovering){
				targetAlpha = 0.2f;
			} else {
				targetAlpha = 1.5f;
			}
		} else {
			targetAlpha = 0f;
            slot.GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
        }
	}

	public void SetWord(PhraseSequenceElement word){
		this.word = word;
        RefreshSlotState();
	}	
	
	public void AcceptDrop (IWordContainer phraseObject)
	{
        if (Word != null) {
            if (phraseObject.Word.WordID == Word.WordID) {
                if (!PlayerManager.main.playerData.WordStorage.FoundWords.Contains(Word.WordID)) {
                    Fulfill();
                    if (phraseObject.gameObject) {
                        Destroy(phraseObject.gameObject);
                    }

                    AudioManager.main.PlayWordSuccess();
                } else {
                    AudioManager.main.PlayWordFailure();
                }
            }
        }

        //if (Word != null && !AllowOverride) {
        //    if (phraseObject.Word.GetText() == word.Text) {
        //        Fulfill ();
        //        Destroy (phraseObject.gameObject);

        //        AudioManager.main.PlayWordSuccess ();
        //    } else {
        //        AudioManager.main.PlayWordFailure ();
        //    }
        //} else {
        //    word = phraseObject.Word;
        //    Fulfill();
        //    Destroy (phraseObject.gameObject);

        //    AudioManager.main.PlayWordSuccess ();
        //}

        //if (!AllowOverride) {
        //    if (OnPhraseDropped != null) {
        //        OnPhraseDropped (this, new PhraseEventArgs (phraseObject));
        //    }
        //}
	}

	public void Fulfill(){
		//if (!AllowOverride) {
		ObjectiveManager.main.AddFoundWord (gameObject, Word);
		//}

		slot.GetComponent<CanvasGroup> ().alpha = 1f;
	}
	
	public void OnPointerEnter (PointerEventData eventData)
	{
		//hovering = true;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		//hovering = false;
	}
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		if (word != null){
			if(PlayerManager.main.playerData.WordStorage.ContainsFoundWord(word.WordID)){
				UISystem.main.PhraseDragHandler.BeginDrag (word, eventData.position);
			}
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		//Debug.Log ("Draggin");
	}

	public void SetRankUpReady(bool rankUpReady){
		this.rankUpReady = rankUpReady;
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
        if (eventData.button == PointerEventData.InputButton.Left) {
            CrystallizeEventManager.UI.RaiseWordClicked(this, new WordClickedEventArgs(word, "Say"));
        } else {
            CrystallizeEventManager.UI.RaiseWordClicked(this, new WordClickedEventArgs(word, "Dictionary"));
        }

		TrySRS ();
        //if (!EnableRankUp) {
        //    return;
        //}

        //if (LevelSettings.main) {
        //    if (!LevelSettings.main.canRankUp) {
        //        return;
        //    }
        //}

        //if(word != null){
        //    if(WordEntryUI.main.IsOpen){
        //        WordEntryUI.main.CheckMeaning();
        //    } else {
        //        if (ObjectiveManager.main.IsWordFound(word.WordID)) {
        //            if (PlayerManager.main.State == PlayerState.Exploring) {
        //                if (LevelSettings.CanRankUp(word.WordID)) {
        //                    WordEntryUI.main.Open(word.WordID, (Vector2)rectTransform.position + rectTransform.rect.center + rectTransform.rect.height * 0.5f * Vector2.up);
        //                }
        //            }
        //        }
        //    } 
        //}
	}

	void TrySRS(){
		if (word == null) {
			return;
		}

		if (!LevelSettings.main.allowSRS) {
			return;
		}

        if (ObjectiveManager.main.IsWordFound(word.WordID)) {
            if (PlayerManager.main.State == PlayerState.Exploring) {
				Action a = null;
				if(LevelSettings.main.allowLevelUp){
					a = AddExperience;
				}

                CrystallizeEventManager.UI.RaiseUIRequest(this, 
					new WordTranslationUIRequestEventArgs(MainCanvas.main.gameObject, word, 
				                                      GetComponent<RectTransform>(), a, null));
            }
        }
	}
	
	void AddExperience(){
		PlayerData.Instance.InventoryState.CurrentLevelExperience += 5;
	}

}
