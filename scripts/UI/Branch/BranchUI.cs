using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BranchUI : MonoBehaviour {

	public GameObject choicePrefab;
    public RectTransform choiceParent;

	public void Initialize(List<BranchedDialogueElement> branches){
        transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f - 150f);

		foreach (var b in branches) {
			var instance = Instantiate(choicePrefab) as GameObject;
			instance.GetComponent<BranchChoiceUI>().Initiallize(b);
			instance.GetComponent<BranchChoiceUI>().OnClicked += HandleOnClicked;
			instance.transform.SetParent(choiceParent);
		}
	}

	public void Close(){
		Destroy (gameObject);
	}

	void HandleOnClicked (object sender, System.EventArgs e)
	{
		var bcui = (BranchChoiceUI)sender;

		// TODO: this will need to be an instance that includes the template and what was filled in
		//var p = Phrase.GetPhraseFromSequence (template.Phrase);
		CrystallizeEventManager.UI.RaiseUIInteraction (this, new DialogueBranchSelectedEventArgs (bcui.BranchElement));

		Close ();
	}

}
