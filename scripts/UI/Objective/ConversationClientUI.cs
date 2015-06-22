using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationClientUI : MonoBehaviour, IPointerClickHandler {

    const float MoveSpeed = 440f;
    const float ScaleSpeed = 200f;
    public const float DefaultSize = 70f;

    public ConversationClientData clientData;
    public Text completedText;
    public Image portraitImage;
    public Image checkmarkImage;

    public List<PhraseSegmentData> PhraseData { get; private set; }// = new List<PhraseSegmentData>();

    public Vector2 Anchor { get; set; }
    public float TargetScale { get; set; }
    public float TargetAlpha { get; set; }
    public bool NeedsSet { get; set; }

    float currentSize = DefaultSize;
    bool initialized = false;

    public RectTransform rectTransform { get; private set; }

    public event EventHandler OnWordsChanged;

    // Use this for initialization
    void Start() {
        CrystallizeEventManager.main.OnLoad += HandleOnLoad;

        rectTransform = GetComponent<RectTransform>();
        GetComponent<CanvasGroup>().alpha = 0;

        if (!initialized) {
            Initialize(clientData);
        }

        ObjectiveManager.main.OnWordUnlocked += HandleOnWordFound;
    }

    void HandleOnWordFound(object sender, System.EventArgs e) {
        PhraseData = clientData.GetMissingWords();

        if (OnWordsChanged != null) {
            OnWordsChanged(this, EventArgs.Empty);
        }
    }

    // Update is called once per frame
    void Update() {
        rectTransform.position = Vector2.MoveTowards(rectTransform.position, Anchor, MoveSpeed * Time.deltaTime);
        currentSize = Mathf.MoveTowards(currentSize, DefaultSize * TargetScale, ScaleSpeed * Time.deltaTime);
        rectTransform.sizeDelta = currentSize * Vector2.one;//Vector2.MoveTowards (rectTransform.sizeDelta, TargetScale * DefaultSize * Vector2.one, ScaleSpeed * Time.deltaTime);
        GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(GetComponent<CanvasGroup>().alpha, 1f, 2f * Time.deltaTime);

        int total = PhraseData.Count;
        int completed = 0;
        foreach (var p in PhraseData) {
            if (PlayerData.Instance.WordStorage.ContainsFoundWord(p)) {
                completed++;
            }
        }

        if (PlayerData.Instance.FriendData.GetFriendData(clientData.ID).FriendLevel == 0) {
            checkmarkImage.gameObject.SetActive(false);
            completedText.gameObject.SetActive(true);
            completedText.text = string.Format("{0}/{1}", completed, total);
        } else {
            checkmarkImage.gameObject.SetActive(true);
            completedText.gameObject.SetActive(false);
        }
        portraitImage.sprite = clientData.socialData.portrait;
    }

    void HandleOnLoad(object sender, System.EventArgs e) {
        Initialize(clientData);
    }

    public void Initialize(ConversationClientData clientData) {
        this.clientData = clientData;
        PhraseData = clientData.GetMissingWords();

        initialized = true;
    }

    public void Set() {
        rectTransform.position = Anchor;
        currentSize = DefaultSize * TargetScale;
        GetComponent<CanvasGroup>().alpha = 0;

        NeedsSet = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            UnlockWords();
        } else {
            transform.parent.GetComponent<ConversationClientPanelUI>().SwitchToClient(this);
        }
    }

    void UnlockWords() {
        transform.parent.GetComponent<ConversationClientPanelUI>().UnlockWords();
    }

}
