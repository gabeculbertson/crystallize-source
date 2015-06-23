using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ValuedSelectionMenuUI : ConfirmMenuUI<ValuedItem, ValuedItemEventArg> {
	

	const string ResourcePath = "UI/ValuedSelectionMenu";
	public static ValuedSelectionMenuUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<ValuedSelectionMenuUI>(ResourcePath);
	}

	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, ValuedItem item)
	{
		var texts = obj.GetComponentsInChildren<Text> ();
		texts [0].text = item.Text;
		texts [1].text = item.Value.ToString();

	}
	protected override ValuedItemEventArg createEventArg (GameObject obj)
	{
		var texts = obj.GetComponentsInChildren<Text> ();
		int num;
		bool isInt = int.TryParse (texts [1].text, out num);
		if(!isInt)
			Debug.LogWarning("text data is not integer");
		return new ValuedItemEventArg (texts [0].text, num);
	}
	#endregion
	
}
