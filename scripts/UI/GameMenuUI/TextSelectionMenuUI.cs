using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextSelectionMenuUI : ConfirmMenuUI<TextMenuItem, TextMenuItemEventArg> {

	const string ResourcePath = "UI/TextSelectionMenu";
	new public static TextSelectionMenuUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<TextSelectionMenuUI>(ResourcePath);
	}
	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, TextMenuItem item)
	{
		obj.GetComponentInChildren<Text> ().text = item.text;
	}
	protected override TextMenuItemEventArg createEventArg (GameObject obj)
	{
		return new TextMenuItemEventArg (obj.GetComponentInChildren<Text> ().text);
	}
	#endregion
}
