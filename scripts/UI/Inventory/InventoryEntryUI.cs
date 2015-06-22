using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;	

public class InventoryEntryUI : MonoBehaviour, IPointerDownHandler, IWordContainer {

	const float MoveSpeed = 2000f;

    // TODO: get rid of this
	public PhraseSegmentData phraseData;
    public PhraseSequenceElement word;
	public InventoryType type = InventoryType.Objective;

	public Transform Container { get; set; }
	public Vector2 Anchor { get; set; }
	public bool Dragging { get; private set; }
	public RectTransform rectTransform { get; private set; }

	public PhraseSegmentData PhraseData { 
		get {
			return phraseData;
		}
	}

    public PhraseSequenceElement Word {
        get {
            return word;
        }
    }

	float lifetime = 1f;
	Vector2 dragOffset;

	public event EventHandler<PhraseEventArgs> OnDrop;
	public event EventHandler OnDestroyEvent;

	void Awake(){
		rectTransform = GetComponent<RectTransform> ();

		OnDrop += CrystallizeEventManager.UI.RaiseOnDropWord;
	}

	void Start(){
		Anchor = rectTransform.position;
		if (phraseData) {
			GetComponentInChildren<Text>().text = phraseData.ConvertedText;
		}

		switch (type) {
		case InventoryType.Conversation:
                GetComponent<Image>().sprite = EffectLibrary.Instance.conversationWordShape;
			break;
		case InventoryType.Objective:
            GetComponent<Image>().sprite = EffectLibrary.Instance.objectiveWordShape;
			break;
		case InventoryType.Permanent:
            GetComponent<Image>().sprite = EffectLibrary.Instance.inventoryWordShape;
			break;
		}
	}

	void Update(){
		if (Dragging) {
			if (Input.GetMouseButtonUp (0)) {
				CompleteDrag ();
			}
		}  
	}

	void LateUpdate(){
		GetComponent<Image> ().color = GUIPallet.Instance.GetColorForWordCategory (PhraseData.Category);
		
		if (Dragging) {
			rectTransform.position = (Vector2)Input.mousePosition + dragOffset;
		} else if (Container) {
			rectTransform.position = Vector2.MoveTowards (rectTransform.position, Anchor, MoveSpeed * Time.deltaTime);
		} else {
			lifetime -= Time.deltaTime;
			var c = GetComponent<Image> ().color;
			c.a = lifetime;
			GetComponent<Image> ().color = c;
			if (lifetime < 0) {
				Destroy (gameObject);
			}
		}
	}

	void OnDestroy(){
		if (OnDestroyEvent != null) {
			OnDestroyEvent(this, EventArgs.Empty);
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		BeginDrag (eventData.position);
	}

	public void BeginDrag(Vector2 position){
		dragOffset = (Vector2)rectTransform.position - position;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		Dragging = true;
	}

	public void CompleteDrag(){
		var raycastResults = new List<RaycastResult>();
		var eventData = new PointerEventData(EventSystem.current);
		eventData.position = Input.mousePosition;
		EventSystem.current.RaycastAll(eventData, raycastResults);
		foreach(var r in raycastResults){
			var phraseAcceptor = r.gameObject.GetInterface<IPhraseDropHandler>();
			if(phraseAcceptor != null){
                //Debug.Log(phraseAcceptor + " accepting " + word.Tags.Count);
				phraseAcceptor.AcceptDrop(this);
			}
		}
		
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		Dragging = false;

		if (OnDrop != null) {
            //Debug.Log("Dropping " + word.Tags.Count);
			OnDrop(gameObject, new PhraseEventArgs(word));
		}
	}

}
