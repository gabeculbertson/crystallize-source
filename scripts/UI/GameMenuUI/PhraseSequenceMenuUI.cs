using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseSequenceMenuUI : ConfirmMenuUI<PhraseSequence, PhraseSequenceEventArg> 

{
	#region implemented abstract members of SelectionMenuUI

	protected override void InitializeButton (GameObject obj, PhraseSequence item)
	{
		obj.AddComponent<Text>().text = item.GetText();
		obj.AddComponent<DataContainer>().Store(item);
	}

	protected override PhraseSequenceEventArg createEventArg (GameObject obj)
	{
		PhraseSequenceEventArg arg = new PhraseSequenceEventArg();
		arg.PhraseData = obj.GetComponent<DataContainer>().Retrieve<PhraseSequence>();
	}

	#endregion


	
}
