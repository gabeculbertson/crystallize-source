using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuickDictionaryUI : UIMonoBehaviour {

    public GameObject englishEntryPrefab;

    public GameObject dropHereText;
    public GameObject partOfSpeech;
    public RectTransform contentPanel;
    public RectTransform englishEntryParent;

    public bool ForceOpen { get; set; }

    CanvasGroup contentCanvasGroup;
    WordSlotUI wordSlotUI;

    List<GameObject> englishEntryInstances = new List<GameObject>();

    bool initialized = false;

    public PhraseSequenceElement Word {
        get {
            return wordSlotUI.word;
        }
    }

    public void Initialize() {
        if (initialized) {
            return;
        }

        gameObject.SetActive(true);
        contentCanvasGroup = contentPanel.GetComponent<CanvasGroup>();
        wordSlotUI = GetComponentInChildren<WordSlotUI>();
        wordSlotUI.OnWordDropped += HandleOnWordDropped;
        wordSlotUI.OnWordCleared += HandleOnWordCleared;

        CrystallizeEventManager.UI.OnWordClicked += HandleWordClicked;

        UpdateDictionaryData(null);

        initialized = true;
    }

	// Use this for initialization
	void Start () {
        TutorialCanvas.main.RegisterGameObject("QuickDictionary", gameObject);

        if (!LevelSettings.main.isMultiplayer && !PlayerData.Instance.Flags.GetFlag(FlagPlayerData.DictionaryUnlocked)) {
            gameObject.SetActive(false);
            return;
        }

        Initialize();
	}

    void HandleWordClicked(object sender, WordClickedEventArgs e) {
        if (e.Destination == "Dictionary") {
            UpdateDictionaryData(e.Word);
        }
    }

    void HandleOnWordCleared(object sender, System.EventArgs e) {
        ClearWord();
    }

    void HandleOnWordDropped(object sender, PhraseEventArgs e) {
        if (e.PhraseContainer != null) {
            Destroy(e.PhraseContainer.gameObject);
        }

        UpdateDictionaryData(e.Word);
    }

    void Update() {
        if (UISystem.main.PhraseDragHandler.IsDragging || wordSlotUI.word != null) {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, 5f * Time.deltaTime);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        } else {
            if (!ForceOpen) {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime);
            }
            if (canvasGroup.alpha <= 0) {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        if (wordSlotUI.word == null && !ForceOpen) {
            contentCanvasGroup.alpha = Mathf.MoveTowards(contentCanvasGroup.alpha, 0, Time.deltaTime);
            //wordSlotUI.canvasGroup.alpha = 0.5f;
        } else {
            contentCanvasGroup.alpha = Mathf.MoveTowards(contentCanvasGroup.alpha, 1f, 5f * Time.deltaTime);
            //wordSlotUI.canvasGroup.alpha = 1f;
        }
    }

    void UpdateDictionaryData(PhraseSequenceElement word) {
        if (word != null) {
            if (word.WordID < 1000000) {
                word = null;
            }
        }
        wordSlotUI.SetWord(word);

        if (word == null) {
            dropHereText.SetActive(true);
        } else {
            dropHereText.SetActive(false);
            var e = DictionaryData.Instance.GetEntryFromID(word.WordID);

            var cat = e.PartOfSpeech.GetCategory();
            var c = GUIPallet.Instance.GetColorForWordCategory(cat);
            partOfSpeech.GetComponentInChildren<Text>().text = cat.ToString().ToLower(); 
            partOfSpeech.GetComponentInChildren<Image>().color = c;
            partOfSpeech.GetComponentInChildren<Outline>().effectColor = c;

            foreach (var i in englishEntryInstances) {
                Destroy(i);
            }
            englishEntryInstances.Clear();

            foreach (var eng in e.English) {
                var go = Instantiate(englishEntryPrefab) as GameObject;
                go.GetComponentInChildren<Text>().text = eng;
                go.transform.SetParent(englishEntryParent);
                englishEntryInstances.Add(go);
            }
        }
    }

    public void ClearWord() {
        UpdateDictionaryData(null);
    }

}
