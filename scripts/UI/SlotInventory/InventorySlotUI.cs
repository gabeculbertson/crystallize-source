//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System;
//using System.Collections;

//public class InventorySlotUI : MonoBehaviour, IPhraseDropHandler, IPointerDownHandler, IPointerClickHandler {

//    public Color idleColor;
//    public Color insetColor;
//    public Color activeColor;
//    public Color flashColor = Color.yellow;
//    public GameObject completedImage;
//    public RectTransform insetAreaTransform;
//    public RectTransform cooldownBar;
//    public Image[] coloredImages;
//    public Text levelText;

//    public int level = 0;
//    public int idleExperience = 0;
	
//    public int SlotIndex { get; set; }
//    public PhraseSegmentData PhraseData {
//        get {
//            return ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(PlayerManager.main.playerData.InventoryState.WordIDs [SlotIndex]);
//        }
//        set {
//            if(value == null){
//                PlayerManager.main.playerData.InventoryState.WordIDs [SlotIndex] = null;
//            } else {
//                PlayerManager.main.playerData.InventoryState.WordIDs [SlotIndex] = value.ID;
//            }
//        }
//    }

//    public event EventHandler<PhraseEventArgs> OnWordAdded;
	                                  
//    GameObject currentInstance;

//    void Awake () {
//        //UpdateVisualState ();
//        completedImage.SetActive (false);
//    }

//    void Start(){
//        insetAreaTransform.GetComponent<Image> ().sprite = EffectManager.main.inventoryWordShape;
//        completedImage.GetComponent<Image> ().sprite = EffectManager.main.inventoryWordShape;
//    }
	
//    // Update is called once per frame
//    void Update () {
//        UpdateVisualState();
//    }

//    public void ClearWord(){
//        //phraseData = null;
//        PhraseData = null;
//        completedImage.SetActive(false);
//    }

//    void UpdateVisualState(){
//        if (PhraseData) {
//            GetComponent<Image>().color = activeColor;

//            var levelColor = PhraseEvaluator.GetColor(PhraseData);

//            cooldownBar.gameObject.SetActive(false);
//            var review = ReviewManager.main.reviewLog.GetReview(PhraseData.ID);
//            var duration = review.GetDurationToNextReview();
//            var end = review.GetNextReviewTime();
//            if(end > DateTime.Now){
//                cooldownBar.gameObject.SetActive(true);
//                cooldownBar.GetComponent<CanvasGroup>().blocksRaycasts = false;
//                cooldownBar.SetAsLastSibling();
//                cooldownBar.localScale = new Vector3((float)(end.Ticks - DateTime.Now.Ticks)/duration.Ticks, 1f, 1f);
//            } else {
//                levelColor = Color.Lerp(levelColor, flashColor, 0.5f + 0.5f * Mathf.Sin(Time.time * 3f));
//            }

//            foreach(var image in coloredImages){
//                image.color = levelColor;
//            }
//            level = Mathf.RoundToInt(PhraseEvaluator.GetPhraseLevel(PhraseData)) - 1;
//            levelText.text = level.ToString();

//            completedImage.GetComponent<Image>().color = levelColor; 
//        } else {
//            GetComponent<Image>().color = idleColor;
//            foreach(var image in coloredImages){
//                image.color = insetColor;
//            }
//            levelText.text = "";
//            cooldownBar.gameObject.SetActive(false);
//        }
//    }

//    public void AcceptDrop (IPhraseContainer phraseObject)
//    {
//        var entry = phraseObject.gameObject.GetComponent<InventoryEntryUI>();
//        if (!entry) {
//            return;
//        }

//        if(PhraseData) {
//            return ;
//        }

//        if(entry.type != InventoryType.Permanent) {
//            return;
//        }

//        AcceptPhrase (phraseObject.PhraseData);

//        Destroy(phraseObject.gameObject);
//        OpenReview(); 
//    }

//    public void AcceptPhrase(PhraseSegmentData phrase){
//        PhraseData = phrase;
//        completedImage.SetActive(true);
//        completedImage.GetComponentInChildren<Text>().text = PhraseData.ConvertedText;
//        completedImage.GetComponent<CanvasGroup>().blocksRaycasts = false;

//        if (OnWordAdded != null) {
//            OnWordAdded(this, PhraseEventArgs.Empty);
//        }
//    }

//    public void OpenReview(){
//        if (PhraseData) {
//            WordEntryUI.main.Open (PhraseData.ID, (Vector2)GetComponent<RectTransform> ().position + new Vector2(-200f, 60f), 
//                                   (i) => HandleEntryResult(level, i));
//        }
//    }

//    public void HandleEntryResult(int lastLevel, bool result){
//        var thisLevel = Mathf.RoundToInt(PhraseEvaluator.GetPhraseLevel(PhraseData)) - 1;
//        if (result) {
//            if(lastLevel == thisLevel){
//                idleExperience += 1;
//            }
//        }
//    }
	
//    public void OnPointerDown (PointerEventData eventData)
//    {
//        if (PhraseData) {
//            var go = UISystem.main.PhraseDragHandler.BeginDrag (PhraseData.GetPhraseElement(), eventData.position);
//            go.GetComponent<InventoryEntryUI>().type = InventoryType.Conversation;
//            currentInstance = go;
//        }
//    }
	
//    public void OnPointerClick (PointerEventData eventData)
//    {
//        if (currentInstance) {
//            Destroy(currentInstance);
//        }
//        OpenReview ();
//    }

//}
