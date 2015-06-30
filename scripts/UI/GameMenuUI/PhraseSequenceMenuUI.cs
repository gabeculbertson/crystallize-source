using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseSequenceMenuUI : ConfirmMenuUI<PhraseSequence> 

{
	const string ResourcePath = "UI/PhraseSequenceMenu";
	new public static PhraseSequenceMenuUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<PhraseSequenceMenuUI>(ResourcePath);
	}

	#region implemented abstract members of SelectionMenuUI

	protected override void InitializeButton (GameObject obj, PhraseSequence item)
	{
		obj.GetComponentInChildren<Text>().text = item.GetText();
		obj.AddComponent<DataContainer>().Store(item);
	}
	

	#endregion


	
}
