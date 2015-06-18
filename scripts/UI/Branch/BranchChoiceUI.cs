using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BranchChoiceUI : UIMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public Text choiceText;

	public BranchedDialogueElement BranchElement { get; set; }

	public event System.EventHandler OnClicked;

	public void Initiallize(BranchedDialogueElement branch){
		BranchElement = branch;
		choiceText.text = branch.Description;
		SetHoverState (false);
	}

	void SetHoverState(bool hovering){
		/*if (hovering) {
			canvasGroup.alpha = 0.8f;
		} else {
			canvasGroup.alpha = 0.5f;
		}*/
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		SetHoverState (true);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		SetHoverState (false);
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (OnClicked != null) {
			OnClicked(this, System.EventArgs.Empty);
		}
	}

}
