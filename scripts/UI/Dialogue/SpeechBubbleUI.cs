using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JapaneseTools;

public enum PointerType {
	Normal,
	Phone
}

public class SpeechBubbleUI : MonoBehaviour {

    const string SpeechBubbleTargetTag = "SpeechBubbleTarget";
    static Vector3 DefaultOffset = new Vector3(0, 1.8f, 0);

    public GameObject editButton;
    public GameObject bookmarkButton;
    // TODO: probably want to reference GUIPallet
    public Color wrongColor;

    public Text translationText;
	public GameObject wordButtonPrefab;
	public GameObject wordsPanel;
    public Transform target;
    public PhraseSequence phrase;
	//public PhraseSegmentData phrase;

    bool canEdit = false;
    Transform baseTarget;
    PointerType pointerType;
    bool flipped = true;
	int uiLayer;
	Vector2 offset; 

	public RectTransform rectTransform { get; private set; }
	public Vector2 RootPosition { get; set; }
	public Vector2 TargetVerticalOffset { get; set; }
    public Vector2 HorizontalOffset { get; set; }
	
    public bool Flipped {
        get {
            return flipped;
        }
        set {
            if (flipped != value) {
                flipped = value;
                UpdateBackground();
            }
        }
    }

    public PointerType PointerType {
        get {
            return pointerType;
        }
        set {
            if (pointerType != value) {
                pointerType = value;
                UpdateBackground();
            }
        }
    }

	public Vector2 FlipOffset {
		get {
			if (Flipped) {
				return Vector2.zero;
			} else {
				return -rectTransform.rect.width * Vector2.right;
			}
		}
	}

    public Rect DoubleRect {
        get {
            var r = rectTransform.rect;
            r.width *= 2f;
            r.center = RootPosition;
            return r;
        }
    }

    public GameObject BookmarkButtonInstance { get; set; }

    public void Initialize(Transform target, PhraseSequence phrase, PointerType pointerType, bool canEdit, bool checkGrammar) {
        this.target = null;
        foreach (var t in target.GetComponentsInChildren<Transform>()) {
            if (t.CompareTag(SpeechBubbleTargetTag)) {
                this.target = t;
                break;
            }
        }
        if (this.target == null) {
            this.target = new GameObject(SpeechBubbleTargetTag).transform;
            this.target.SetParent(target, false);
            if (target.CompareTag("Player"))
            {
                this.target.localPosition = new Vector3(0, 1.5f, 0);
                this.HorizontalOffset = new Vector2(48f, 0);
            }
            else
            {
                this.target.localPosition = DefaultOffset;
            }
            this.target.tag = SpeechBubbleTargetTag;
        }

        this.baseTarget = target;
        this.phrase = phrase;
        this.PointerType = pointerType;
        this.canEdit = canEdit;

        if (checkGrammar) {
            if (!GameData.Instance.PhraseClassData.IsValidGrammar(phrase)) {
                GetComponent<Image>().color = wrongColor;
            } 
        }
    }

	void Awake(){
		rectTransform = GetComponent<RectTransform> ();
	}

	void Start () {
		uiLayer = LayerMask.NameToLayer ("UI");

        if (canEdit) {
            //var btn = Instantiate(editButton) as GameObject;
            //btn.transform.SetParent(wordsPanel.transform, false);
            //btn.GetComponent<UIButton>().OnClicked += HandleEditButtonClicked;
        } else {
            var btn = Instantiate(bookmarkButton) as GameObject;
            btn.GetInterface<IInitializable<PhraseSequence>>().Initialize(phrase);
            btn.transform.SetParent(wordsPanel.transform);
        }

        /*if (canBookmark) {
            var btn = Instantiate(bookmarkButton) as GameObject;
            btn.transform.SetParent(wordsPanel.transform);
            btn.GetComponent<UIButton>().OnClicked += HandleBookmarkButtonClicked;
            BookmarkButtonInstance = btn;
        }*/

		foreach (var word in phrase.PhraseElements) {
			var go = Instantiate(wordButtonPrefab) as GameObject;
			go.transform.SetParent(wordsPanel.transform, false);
			go.GetComponent<SpeechBubbleWordUI>().Initialize(word);
		}

		CrystallizeEventManager.UI.RaiseSpeechBubbleOpen (gameObject, new PhraseEventArgs (phrase));
        CrystallizeEventManager.Environment.AfterCameraMove += AfterCameraMove;


		if (translationText.text == "") {
			translationText.gameObject.SetActive(false);
		}

        UpdateBackground();
	}

    void HandleEditButtonClicked(object sender, System.EventArgs e) {
        CrystallizeEventManager.UI.RaiseUIRequest(this, new DragDropFreeInputUIRequestEventArgs(gameObject, phrase));
        CrystallizeEventManager.UI.RaiseSpeechBubbleRequested(this, new SpeechBubbleRequestedEventArgs(baseTarget));
    }

    void HandleBookmarkButtonClicked(object sender, System.EventArgs e) {
        CrystallizeEventManager.UI.RaiseBookmarkChanged(this, new BookmarkChangedEventArgs(phrase));
        //TODO: bad
        ((UIButton)sender).transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
    }

    void OnDestroy() {
        CrystallizeEventManager.Environment.AfterCameraMove -= AfterCameraMove;
    }

    void AfterCameraMove(object sender, System.EventArgs e) {
        offset = Vector2.MoveTowards(offset, TargetVerticalOffset + HorizontalOffset, 100f * Time.deltaTime);

        if (target) {
            if (target.gameObject.layer == uiLayer) {
                rectTransform.position = target.position;
            } else {
                RootPosition = (Vector2)Camera.main.WorldToScreenPoint(target.position);
                rectTransform.position = RootPosition + offset + FlipOffset;

                if (Flipped) {
                    var overflow = rectTransform.position.x + rectTransform.rect.xMax - Screen.width;
                    if (overflow > 0) {
                        rectTransform.position -= overflow * Vector3.right;
                    }
                } else {
                    var overflow = rectTransform.position.x + rectTransform.rect.xMin;
                    if (overflow < 0) {
                        rectTransform.position -= overflow * Vector3.right;
                    }
                }

            }
        }
    }

	public void SetTranslation(string text){
		translationText.text = text;
	}

    void UpdateBackground(){
         switch (pointerType) {
            case PointerType.Normal:
                if (Flipped) {
                    GetComponent<Image>().sprite = GUIPallet.Instance.leftSpeechBubble;
                } else {
                    GetComponent<Image>().sprite = GUIPallet.Instance.rightSpeechBubble;
                };
                break;
            case PointerType.Phone:
                if (Flipped) {
                    GetComponent<Image>().sprite = GUIPallet.Instance.leftPhoneSpeechBubble;
                } else {
                    GetComponent<Image>().sprite = GUIPallet.Instance.rightPhoneSpeechBubble;
                }
                break;
         }
    }

    // TODO: ew....
    public RectTransform GetWord(int wordID) {
        foreach (var w in GetComponentsInChildren<Transform>()) {
            if (w.GetComponent<SpeechBubbleWordUI>()) {
                if (w.GetComponent<SpeechBubbleWordUI>().Word.WordID == wordID) {
                    return w.GetComponent<RectTransform>();
                }
            }
        }
        return null;
    }

}
