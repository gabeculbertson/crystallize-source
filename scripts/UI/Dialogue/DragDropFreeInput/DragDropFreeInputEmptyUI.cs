using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

public class DragDropFreeInputEmptyUI : MonoBehaviour, IPhraseDropHandler, IPhraseDropEvent, IPointerEnterHandler, IPointerExitHandler {

    const float StretchSpeed = 1000f;

    float currentWidth = 0;
    float restingWidth = 32f;
    float openWidth = 160f;

    public int Index { get; set; }

	public event EventHandler<PhraseEventArgs> OnPhraseDropped;

	void Start(){
		GetComponent<Image> ().color = Color.gray;
	}

    public void Initialize(int index, float restingWidth, float openWidth) {
        this.restingWidth = restingWidth;
        this.openWidth = openWidth;
        this.Index = index;
    }
     
    void Update() {
        if (UISystem.main.PhraseDragHandler.IsDragging) {
            currentWidth = Mathf.MoveTowards(currentWidth, openWidth, StretchSpeed * Time.deltaTime);
        } else{
            currentWidth = Mathf.MoveTowards(currentWidth, restingWidth, StretchSpeed * Time.deltaTime);
        }

        GetComponent<Image>().enabled = currentWidth > 8f;

        GetComponent<LayoutElement>().preferredWidth = currentWidth;
    }

	public void AcceptDrop (IWordContainer phraseObject)
	{
        Destroy(phraseObject.gameObject);

        //Debug.Log(" got " + phraseObject.Word.Tags.Count);
		if (OnPhraseDropped != null) {
			OnPhraseDropped(this, new PhraseEventArgs(phraseObject));
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (UISystem.main.PhraseDragHandler.IsDragging) {
			GetComponent<Image> ().color = Color.white;
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		GetComponent<Image> ().color = Color.gray;
	}
	
}
