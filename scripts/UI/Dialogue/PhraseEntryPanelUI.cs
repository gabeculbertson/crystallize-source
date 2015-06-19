using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Crystallize;

public class PhraseEntryPanelUI : UIMonoBehaviour {

	public GameObject wordPrefab;
	public GameObject slotPrefab;
	public RectTransform translationParent;
	public RectTransform wordParent;
    public RectTransform needMoreWordsMessage;
	
	public bool isRight = false;

	PhraseSequence phrase;
    bool provideMissingWordsMessage = false;

    List<int> missingWords = new List<int>();

    List<GameObject> instances = new List<GameObject>();
	Dictionary<PlayerSpeechBubbleDropAreaUI, PhraseSequenceElement> targetWords = new Dictionary<PlayerSpeechBubbleDropAreaUI, PhraseSequenceElement>();
    Dictionary<PlayerSpeechBubbleDropAreaUI, string> contextSlots = new Dictionary<PlayerSpeechBubbleDropAreaUI, string>();
    Dictionary<PlayerSpeechBubbleDropAreaUI, string> taggedSlots = new Dictionary<PlayerSpeechBubbleDropAreaUI, string>();
	//Dictionary<PlayerSpeechBubbleDropAreaUI, PhraseSegmentData> targetWords = new Dictionary<PlayerSpeechBubbleDropAreaUI, PhraseSegmentData>();

    void Awake() {
        canvasGroup.alpha = 0;
    }

	// Use this for initialization
	void Start () {
        HandleSlotStateChanged(null, System.EventArgs.Empty);
        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.FixedPhraseInput));
	}

    void Update() {
        bool hasAllWords = true;
        foreach (var word in missingWords) {
            if (!PlayerManager.main.playerData.WordStorage.ContainsFoundWord(word)) {
                hasAllWords = false;
                break;
            }
        }

        //if (hasAllWords) {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, 5f * Time.deltaTime);
        //} else {
        //    canvasGroup.alpha = 0;
        //}

        if (needMoreWordsMessage != null) {
            if (provideMissingWordsMessage) {
                needMoreWordsMessage.gameObject.SetActive(!hasAllWords);
            } else {
                needMoreWordsMessage.gameObject.SetActive(false);
            }
        }
    }

    void OnDestroy() {
        CrystallizeEventManager.UI.RaiseUIModeRequested(this, new UIModeChangedEventArgs(UIMode.Exploring));
        CrystallizeEventManager.UI.OnWordClicked -= HandleWordClicked;
        UISystem.main.RemoveCenterPanel(this);
    }

    public void Initialize(PhraseSequence phrase, List<int> missingWords, bool provideMissingWordsMessage) {
        this.missingWords = missingWords;
        transform.position = new Vector2(Screen.width * .5f, Screen.height * .5f - 150f);

        if (isRight) {
            GetComponent<Image>().sprite = GUIPallet.main.rightSpeechBubble;
        }

        this.phrase = phrase;
        this.provideMissingWordsMessage = provideMissingWordsMessage;

        if (phrase.Translation == "" || phrase.Translation == null) {
            translationParent.gameObject.SetActive(false);
        } else {
            translationParent.gameObject.SetActive(true);
            translationParent.gameObject.GetComponentInChildren<Text>().text = phrase.Translation;
        }

        //foreach (var word in phrase.ChildPhrases) {
        foreach (var ele in phrase.GetElements()) {
            var word = PhraseSegmentData.GetWordInstance(ele);

            int tagCount = 0;
            GameObject instance = null;
            if (missingWords.Contains(ele.WordID)
                || ele.ElementType == PhraseSequenceElementType.ContextSlot
                || ele.ElementType == PhraseSequenceElementType.TaggedSlot) {
                instance = Instantiate(slotPrefab) as GameObject;
                instance.transform.SetParent(wordParent);
                instance.GetComponent<PlayerSpeechBubbleDropAreaUI>().OnStateChanged += HandleSlotStateChanged;

                targetWords[instance.GetComponent<PlayerSpeechBubbleDropAreaUI>()] = ele;
                if (ele.ElementType == PhraseSequenceElementType.ContextSlot) {
                    contextSlots[instance.GetComponent<PlayerSpeechBubbleDropAreaUI>()] = ele.Text;
                }

                if (ele.ElementType == PhraseSequenceElementType.TaggedSlot) {
                    taggedSlots[instance.GetComponent<PlayerSpeechBubbleDropAreaUI>()] = "tag." + ele.Text; //ele.Text;
                    tagCount++;
                }
            } else {
                instance = Instantiate(wordPrefab) as GameObject;
                instance.transform.SetParent(wordParent);
                instance.GetComponentInChildren<Text>().text = word.ConvertedText;
                instance.GetComponentInChildren<Outline>().effectColor = GUIPallet.main.GetColorForWordCategory(word.Category);
                instance.GetComponentInChildren<Image>().color = GUIPallet.main.GetColorForWordCategory(word.Category);
            }
            instances.Add(instance);
        }

        //if (!PlayerManager.main.playerData.Tutorial.GetTutorialViewed(SlotClickTutorialScript.ID) 
        //    && PlayerManager.main.playerData.WordStorage.GetInventoryCount() > 10
        //    && GetFirstEmptySlot()) {
        //        TutorialCanvas.main.PlayTutorial<SlotClickTutorialScript>().Initialize(GetFirstEmptySlot().GetComponent<RectTransform>());
        //}

        UISystem.main.AddCenterPanel(this);
        CrystallizeEventManager.UI.OnWordClicked += HandleWordClicked;
    }

    void HandleSlotStateChanged(object sender, System.EventArgs args) {
        var c = Color.white;

        bool hasAllWords = true;
        foreach (var word in missingWords) {
            if (!PlayerManager.main.playerData.WordStorage.ContainsFoundWord(word)) {
                hasAllWords = false;
                break;
            }
        }
        if (!hasAllWords) {
            c = new Color(1f, 0.7f, 0.7f);
        }

        if (IsFull()) {
            PlayerManager.main.PlayerGameObject.GetComponent<DialogueActor>().SetPhrase(GetInputPhrase());
            if (IsCorrect()) {
                CrystallizeEventManager.UI.RaiseUIInteraction(this, new PhraseInputEventArgs(phrase, GetContextData()));
                Close();
                c = Color.green;
            } else {
                c = Color.red;
            }
        }

        c.a = 1f;//175f / 255f;
        wordParent.parent.GetComponent<Image>().color = c;

        DataLogger.LogTimestampedData(GetInputPhrase().GetText());
    }

	void Close(){
		Destroy (gameObject);
	}
	
	bool IsFull(){
		foreach (var key in targetWords.Keys) {
			if(key.word == null){
				return false;
			}
		}
		return true;
	}
	
	bool IsCorrect(){
		foreach (var key in targetWords.Keys) {
			if(contextSlots.ContainsKey(key)){
				continue;			
			}

            if (taggedSlots.ContainsKey(key)) {
                continue;
            }

            if (!PhraseSequenceElement.IsEqual(key.word, targetWords[key])) { //PhraseSegmentData.IsEquivalent(key.phrase, targetWords[key])){
				return false;
			}
		}
		return true;
	}

	ContextData GetContextData(){
		var cd = new ContextData ();
		foreach (var cs in contextSlots.Keys) {
			var p = new PhraseSequence();
			p.Add(cs.word);
			cd.Elements.Add(new ContextDataElement(contextSlots[cs], p));
		}

        foreach (var ts in taggedSlots.Keys) {
            var p = new PhraseSequence();
            p.Add(ts.word);
            cd.Elements.Add(new ContextDataElement(taggedSlots[ts], p));
        }

		return cd;
	}

    PhraseSequence GetInputPhrase() {
        var p = new PhraseSequence();
        int count = 0;
        foreach (var i in instances) {
            if (i.GetComponent<PlayerSpeechBubbleDropAreaUI>()) {
                var w = i.GetComponent<PlayerSpeechBubbleDropAreaUI>().word;
                if (w == null) {
                    p.PhraseElements.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "_"));
                } else {
                    p.PhraseElements.Add(w);
                }
            } else {
                p.PhraseElements.Add(phrase.PhraseElements[count]);
            }
            count++;
        }
        return p;
    }

    PlayerSpeechBubbleDropAreaUI GetFirstEmptySlot() {
        if (IsFull()) {
            return null;
        }

        foreach (var i in instances) {
            var da = i.GetComponent<PlayerSpeechBubbleDropAreaUI>();
            if (da) {
                if (da.word == null) {
                    return da;
                }
            }
        }
        return null;
    }


    void HandleWordClicked(object sender, WordClickedEventArgs e) {
        if (e.Destination == "Say") {
            var es = GetFirstEmptySlot();
            if (es) {
                es.AcceptDrop(new WordContainer(e.Word));
            }
        }
    }

}
