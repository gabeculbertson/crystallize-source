//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//public class _HorizontalSlotInventoryPanelUI : UIMonoBehaviour, IPointerClickHandler, IObjectiveUI, IInventoryUI {

//    public const float Padding = 8f;

//    static _HorizontalSlotInventoryPanelUI main { get; set; }

//    public static bool IsFull(){
//        if (!main) {
//            return false;
//        }

//        return main.raised;
//    }

//    public GameObject slotPrefab;
//    public RectTransform background;

//    Dictionary<PhraseSegmentData, GameObject> slotInstances = new Dictionary<PhraseSegmentData, GameObject> ();
//    List<RectTransform> slotOrder = new List<RectTransform>();

//    Vector2 pos;
//    Vector2 rootPos;
//    Vector2 offset = Vector2.zero;
//    bool initialized = false;

//    bool raised = false;

//    public IEnumerable<RectTransform> Entries {
//        get {
//            return slotOrder;
//        }
//    }

//    public float Height { get; set; }

//    void Awake(){
//        main = this;
//    }

//    void OnEnable(){
//        GetComponent<CanvasGroup> ().alpha = 0;
//    }

//    // Use this for initialization
//    void Start () {
//        TutorialCanvas.main.InventoryUI = this;
//        TutorialCanvas.main.ObjectiveUI = this;

//        PlayerManager.main.OnLoad += HandleOnLoad;

//        if (!initialized) {
//            Initialize (PlayerManager.main.playerData.InventoryState);
//        }

//        CrystallizeEventManager.main.OnUpdateUI += HandleOnUpdateUI;
//    }

//    void HandleOnUpdateUI (object sender, EventArgs e)
//    {
//        RefreshPanelState ();
//    }

//    void HandleOnLoad (object sender, System.EventArgs e)
//    {
//        Initialize(PlayerManager.main.playerData.InventoryState);
//    }

//    // Update is called once per frame
//    void Update () {
//        MainCanvas.main.BottomHeight = 56f;

//        raised = false;
//        transform.position = new Vector2 (0, offset.y);

//        int columnCount = Mathf.FloorToInt(Screen.width / (160f + Padding));
//        int rowCount = Mathf.CeilToInt((float)slotOrder.Count / columnCount);
//        offset.y = Padding + (Padding + 32f) * rowCount - 56f;
//        Height = offset.y + 120f;
//        var p = new Vector2(Padding, Padding + (rowCount - 1) * (32f + Padding));
//        int rowIndex = 0;
//        foreach(var slot in slotOrder){
//            slot.position = p;
//            p.x += 160f + Padding;
//            rowIndex++;
//            if(rowIndex >= columnCount){
//                p.x = Padding;
//                p.y -= 32f + Padding;
//                rowIndex = 0;
//            }
//        }

//        if (Input.GetKeyDown (KeyCode.KeypadDivide)) {
//            slotOrder.Add(AddSlot(null));
//        }
//    }

//    void LateUpdate(){
//        //Fade in
//        GetComponent<CanvasGroup> ().alpha = Mathf.MoveTowards (GetComponent<CanvasGroup> ().alpha, 1f, 2f * Time.deltaTime);
//    }

//    void Initialize(SlotInventoryState state){
//        RefreshPanelState ();
//    }

//    void RefreshPanelState(){
//        slotOrder.Clear ();

//        var inv = PlayerManager.main.playerData.WordStorage.InventoryWordIDs;
//        foreach (var wordID in PlayerManager.main.playerData.WordStorage.ObjectiveWordIDs) {
//            GetSlot(ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID));

//            if(!inv.Contains(wordID)){
//                inv.Insert(0, wordID);
//            }
//        }

//        foreach (var wordID in PlayerManager.main.playerData.WordStorage.FoundWordIDs) {
//            GetSlot(ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID));

//            if(!inv.Contains(wordID)){
//                inv.Insert(0, wordID);
//            }
//        }

//        var p = new Vector2 (Padding, Padding);
//        foreach (var wordID in inv) {
//            var phrase = ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID (wordID);
//            if (phrase) {
//                var rt = GetSlot (phrase);
//                rt.localPosition = p;
//                rt.SetAsLastSibling();
//                slotOrder.Add (rt);
//                p.x += rt.rect.width + Padding;
//            }
//        }
//    }

//    RectTransform GetSlot(PhraseSegmentData phraseData){
//        if (!phraseData) {
//            return null;
//        }

//        if (!slotInstances.ContainsKey (phraseData)) {
//            AddSlot(phraseData);
//        }
//        return slotInstances[phraseData].GetComponent<RectTransform>();
//    }

//    RectTransform AddSlot(PhraseSequenceElement word){
//        var go = Instantiate (slotPrefab) as GameObject;
//        go.GetComponent<ExplicitInventorySlotUI> ().SetWord (word);
//        go.transform.SetParent (transform);	
//        if (word != null) {
//            slotInstances.Add (word, go);
//        }
//        return go.GetComponent<RectTransform> ();
//    }
	
//    public void OnPointerClick (PointerEventData eventData)
//    {
//        raised = !raised;
//    }

//    public RectTransform GetObjective (PhraseSegmentData phrase)
//    {
//        return GetEntry (phrase);
//    }
	
//    public RectTransform GetEntry (PhraseSegmentData phrase)
//    {
//        return (from i in slotInstances.Values where i.GetComponent<ExplicitInventorySlotUI> ().Word == phrase select i.GetComponent<RectTransform>()).FirstOrDefault ();
//    }

//}
