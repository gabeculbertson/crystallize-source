//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//public class HorizontalSlotInventoryPanelUI : UIMonoBehaviour, IObjectiveUI, IInventoryUI {

//    public const float Padding = 8f;
//    public const int InventoryCount = 10;

//    static HorizontalSlotInventoryPanelUI main { get; set; }

//    public static bool IsFull(){
//        if (!main) {
//            return false;
//        }

//        return main.raised;
//    }

//    public GameObject slotPrefab;
//    public GameObject emptyPrefab;

//    Dictionary<PhraseSegmentData, GameObject> slotInstances = new Dictionary<PhraseSegmentData, GameObject> ();
//    List<GameObject> emptyInstances = new List<GameObject>();
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

//    void Update(){
//        MainCanvas.main.BottomHeight = rectTransform.rect.height; //56f;

//        if (Input.GetKeyDown (KeyCode.KeypadDivide)) {
//            AddSlot(null);
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
//        foreach (var e in emptyInstances) {
//            Destroy(e);
//        }
//        foreach (var si in slotInstances) {
//            Destroy(si.Value);
//        }
//        emptyInstances.Clear ();
//        slotInstances.Clear ();
//        slotOrder.Clear ();

//        var inv = PlayerManager.main.playerData.WordStorage.InventoryWordIDs;
//        foreach (var wordID in PlayerManager.main.playerData.WordStorage.ObjectiveWordIDs) {
//            var p = ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID);
//            if(p != null){
//                GetSlot(p);

//                if(!inv.Contains(wordID)){
//                    inv.Insert(0, wordID);
//                }
//            }
//        }

//        foreach (var wordID in PlayerManager.main.playerData.WordStorage.FoundWordIDs) {
//            var p = ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID);
//            if(p != null){
//                GetSlot(ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID(wordID));

//                if(!inv.Contains(wordID)){
//                    inv.Insert(0, wordID);
//                }
//            }
//        }

//        foreach (var slot in slotInstances.Values) {
//            slot.SetActive(false);
//        }

//        for(int i = 0; i < InventoryCount; i++){
//            string wordID = null;
//            if(i < inv.Count){
//                wordID = inv[i];
//            }
//            var phrase = ScriptableObjectDictionaries.main.phraseDictionaryData.GetPhraseForID (wordID);
//            var rt = GetSlot(phrase);
//            //rt.gameObject.GetInterface<IPhraseDropEvent>().OnPhraseDropped += HandleOnPhraseDropped;
//            rt.SetAsLastSibling();
//            slotOrder.Add (rt);
//        }
//    }

//    RectTransform GetSlot(PhraseSegmentData phraseData){
//        if (!phraseData) {
//            return AddEmpty();
//        }

//        if (!slotInstances.ContainsKey (phraseData)) {
//            AddSlot(phraseData);
//        }
//        slotInstances [phraseData].SetActive (true);
//        return slotInstances[phraseData].GetComponent<RectTransform>();
//    }

//    RectTransform AddSlot(PhraseSegmentData phraseData){
//        var go = Instantiate (slotPrefab) as GameObject;
//        go.GetComponent<ExplicitInventorySlotUI> ().SetWord (phraseData);
//        go.transform.SetParent (transform);	
//        if (phraseData) {
//            slotInstances.Add (phraseData, go);
//        }
//        return go.GetComponent<RectTransform> ();
//    }

//    RectTransform AddEmpty(){
//        var go = Instantiate (emptyPrefab) as GameObject;
//        go.transform.SetParent (transform);	
//        emptyInstances.Add (go);
//        return go.GetComponent<RectTransform> ();
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
