using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using JapaneseTools;

public class WordEntryUI : UIMonoBehaviour {

	public static WordEntryUI main { get; set; }

	public Text wordText;
	public GameObject readingPanel;
	public InputField readingInput;
	public InputField meaningInput;
	public Text readingPlaceholder;
	public Text meaningPlaceholder;
	public Color successColor = Color.white;

    PhraseSequenceElement word;
	PhraseReviewData currentReviewData;
	//string id = "";
	//WordBlockInstance currentBlock;
	int meaningTries = 0;
	int readingTries = 0;
	bool meaningSolved = false;
	bool readingSolved = false;
	bool gettingClarification = false;

	Action<bool> resultHandler;

	public bool IsOpen { get; private set; }

	public event EventHandler<PhraseEventArgs> OnOpened;
	public event EventHandler<PhraseEventArgs> OnWordRankUp;

	void Awake(){
		main = this;
		Close ();
	}

	void Start(){
		if (!LevelSystemConstructor.main.levelSystemData.useKana) {
			readingPanel.SetActive(false);
		}
	}

	void Update(){
		meaningInput.text = meaningInput.text.Replace ("\n", "").Replace ("\r", "");

		if (LevelSystemConstructor.main.levelSystemData.useKana) {
			readingInput.text = readingInput.text.Replace ("\n", "").Replace ("\r", "");

			if (Input.GetKeyDown (KeyCode.Tab)) {
				if (!meaningSolved && !readingSolved) {
					if (meaningInput.isFocused) {
						FocusControl (readingInput);
					} else {
						FocusControl (meaningInput);
					}
				} else {
					if (meaningSolved) {
						FocusControl (readingInput);
					} else {
						FocusControl (meaningInput);
					}
				}
			}
		} else {
			if(!gettingClarification){
				FocusControl(meaningInput);
			} else {
				EventSystem.current.SetSelectedGameObject (null);
			}
		}

		var left = rectTransform.rect.xMin + rectTransform.position.x;
		if (left < 0) {
			rectTransform.position -= left * Vector3.right;
		}

		var right = rectTransform.rect.xMax + rectTransform.position.x;
		if (right > Screen.width) {
			rectTransform.position -= (right - Screen.width) * Vector3.right;
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			CheckMeaning();
			//Debug.Log("Enter pressed.");
		}

		if (IsOpen) {
			if(Input.GetMouseButtonDown(0)){
				var r = rectTransform.rect;
				r.position += (Vector2)rectTransform.position;
				if(!r.Contains(Input.mousePosition)){
					Close();
				}
			}

			GetComponent<Image>().color = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(Time.time, 1f));
		}
	}

	public void Open(int wordID, Vector2 target, Action<bool> resultHandler = null){
		//Debug.Log ("Opening");
		if (ClarificationPanelUI.main) {
			ClarificationPanelUI.main.Close ();
		}
		gettingClarification = false;

		IsOpen = true;
        word = new PhraseSequenceElement(wordID, 0);

		transform.position = target;

		this.resultHandler = resultHandler;

		currentReviewData = ReviewManager.main.reviewLog.GetReview (wordID);
		gameObject.SetActive (true);
		transform.SetAsLastSibling ();

		meaningInput.GetComponent<Image> ().color = Color.white;
		meaningInput.interactable = true;
		meaningInput.text = "";
		meaningPlaceholder.text = "meaning...";
		if (currentReviewData.Level == 0) {
			GiveMeaning();
		}
		meaningTries = 0;
		meaningSolved = false;

        wordText.text = word.GetText(JapaneseScriptType.Romaji); //ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID (id).ConvertedText;
		if (LevelSystemConstructor.main.levelSystemData.useKana) {
			readingInput.GetComponent<Image> ().color = Color.white;
			readingInput.interactable = true;
			readingInput.text = "";
			readingPlaceholder.text = "reading...";
			readingTries = 0;
			readingSolved = false;

			EventSystem.current.SetSelectedGameObject (readingInput.gameObject, null);
		} else {
			EventSystem.current.SetSelectedGameObject (meaningInput.gameObject, null);
		}

		PlayerController.LockMovement (this);
		MenuSwapper.LockSwapping(this);

		TutorialCanvas.main.ClearAllIndicators ();
		MainCanvas.main.OpenNotificationPanel ("Translate the word to rank up!", 5f);

		if (OnOpened != null) {
			OnOpened(this, new PhraseEventArgs(word));
		}
	}

	void GiveMeaning(){
        string rootText = word.GetTranslation();
		meaningPlaceholder.text = "type <b>" + rootText + "</b>";
	}

	AudioSource GetAudio(){
		if (!GetComponent<AudioSource>()) {
			gameObject.AddComponent<AudioSource>();
		}
		return GetComponent<AudioSource>();
	}

	public void ResetWord(){
		if (IsOpen) {
			currentReviewData.Reset();
			GiveMeaning();
		}
	}

	public void Close(){
		resultHandler = null;

		IsOpen = false;

		gameObject.SetActive (false);

		PlayerController.UnlockMovement (this);
		MenuSwapper.UnlockSwapping(this);

		if (MainCanvas.main) {
			MainCanvas.main.CloseNotificationPanel ();
		}
	}

	public void CheckReading(){
		if (readingInput.text != ""){
			var reading = KanaConverter.Instance.ConvertToRomaji(wordText.text);
			
			if(reading.ToLower() == readingInput.text.ToLower()){
				readingSolved = true;
				readingInput.GetComponent<Image> ().color = successColor;
				readingInput.interactable = false;
				CheckCompleteness();
			} else {
				if(readingTries == 0 && currentReviewData.Level != 0){
					//currentReviewData.LogReview(ReviewManager.main.simulatedTime, false);
				}
				FocusControl(readingInput);
				readingInput.text = "";
				AddReadingTry();
			}
		}
	}

	public void CheckMeaning(){
		if (meaningInput.text != "") {
			var success = word.GetTranslation().ToLower ().Trim () == meaningInput.text.ToLower ().Trim ();
            //if (!success && ScriptableObjectDictionaries.main.phraseDictionaryData.IsAmbiguous (word)) {
            //    if (meaningInput.text.ToLower ().Trim () == ScriptableObjectDictionaries.main.phraseDictionaryData.GetRootMeaning (word)) {
            //        OpenClarificationPanel (word);
            //        return;
            //    }
            //} 
			
			if (success) {//meaning.ToLower() == meaningInput.text.ToLower()){
				meaningSolved = true;
				meaningInput.GetComponent<Image> ().color = successColor;
				meaningInput.interactable = false;
				CheckCompleteness ();
			} else {
				if (meaningTries == 0 && currentReviewData.Level != 0) {
					currentReviewData.LogReview (ReviewManager.main.simulatedTime, false, true);
				}
				FocusControl (meaningInput);
				meaningInput.text = "";
				AddMeaningTry ();

				AudioManager.main.PlayDialogueFailure ();
			}
		} else {
			//Close ();
		}
	}

	void OpenClarificationPanel(PhraseSegmentData word){
		ClarificationPanelUI.main.Open ();
		ClarificationPanelUI.main.Initialize (ScriptableObjectDictionaries.main.phraseDictionaryData.GetDisambiguation (word));
		ClarificationPanelUI.main.GetComponent<RectTransform> ().position = (Vector2)GetComponent<RectTransform> ().position + GetComponent<RectTransform> ().rect.center;
		//targetPhrase = word;
		ClarificationPanelUI.main.OnWordSelected += HandleOnWordSelected;
		gettingClarification = true;
	}

	void HandleOnWordSelected (object sender, PhraseEventArgs e)
	{
		gettingClarification = false;
        //if(targetPhrase.Text == e.PhraseData.Text){
        //    meaningSolved = true;
        //    meaningInput.GetComponent<Image> ().color = successColor;
        //    meaningInput.interactable = false;
        //    CheckCompleteness();
        //} else {
        //    if(meaningTries == 0 && currentReviewData.Level != 0){
        //        currentReviewData.LogReview(ReviewManager.main.simulatedTime, false, false);
        //    }
        //    FocusControl(meaningInput);
        //    meaningInput.text = "";
        //    AddMeaningTry();
        //}
		//ClarificationPanelUI.main.OnWordSelected -= HandleOnWordSelected;
	}

	public void CheckCompleteness(){
		if (!LevelSystemConstructor.main.levelSystemData.useKana) {
			//var lastResult = currentReviewData.GetLastResult();

			var raiseRank = false;
			//Debug.Log(currentReviewData.GetNextReviewTime() + "; " + ReviewManager.main.simulatedTime);
			//Debug.Log("Last result: " + lastResult + "; rvw time: " + currentReviewData.GetNextReviewTime()
			//          + "; rvw mng time: " + ReviewManager.main.simulatedTime);

			if(currentReviewData.GetNextReviewTime() < ReviewManager.main.simulatedTime
			   || currentReviewData.Level == 0){
				PlayerManager.main.playerData.InventoryState.CurrentLevelExperience += 10;
				EffectManager.main.CreateExperiencePointsEffect(GetComponent<RectTransform>(), 10);
				RaiseWordRankUp();
				raiseRank = true;
			} else {
				PlayerManager.main.playerData.InventoryState.CurrentLevelExperience += 1;
				EffectManager.main.CreateExperiencePointsEffect(GetComponent<RectTransform>(), 1);
			}

			AudioManager.main.PlayRankUp();

			currentReviewData.LogReview (ReviewManager.main.simulatedTime, true, raiseRank);

			if(resultHandler != null){
				resultHandler(true);
			}

			Close ();
		} else {
			if (meaningSolved && readingSolved) {
				//currentReviewData.LogReview (ReviewManager.main.simulatedTime, true);

				if(resultHandler != null){
					resultHandler(true);
				}

				Close ();
			} else if (meaningSolved) {
				FocusControl (readingInput);
			} else if (readingSolved) {
				FocusControl (meaningInput);
			}
		}
	}

	void RaiseWordRankUp(){
		if (OnWordRankUp != null) {
			OnWordRankUp(this, new PhraseEventArgs(word));
		}
	}

	void FocusControl(InputField input){
		if (!EventSystem.current.alreadySelecting) {
			EventSystem.current.SetSelectedGameObject (input.gameObject, null);
		}
		//EventSystem.current.alreadySelecting
		input.OnPointerClick (new PointerEventData(EventSystem.current));
	}

	public void OnTextEntered(bool isMeaning){
		if (isMeaning) {
			CheckMeaning ();
		} else {
			CheckReading ();
		}
	}

	void AddMeaningTry(){
		meaningTries++;
		switch (meaningTries) {
		case 1:
			meaningPlaceholder.text = "try again...";
			break;
		case 2:
			meaningPlaceholder.text = "nope...";
			break;
		default:
			meaningPlaceholder.text = "try <b>" + word.GetTranslation() + "</b>";
			break;
		}
	}

	void AddReadingTry(){
		readingTries++;
		switch (readingTries) {
		case 1:
			readingPlaceholder.text = "try again...";
			break;
		case 2:
			readingPlaceholder.text = "nope...";
			break;
		default:
			readingPlaceholder.text = "try <b>" + KanaConverter.Instance.ConvertToRomaji(wordText.text) + "</b>";
			break;
		}
	}

}
