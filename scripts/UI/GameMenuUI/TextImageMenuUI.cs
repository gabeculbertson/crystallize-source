using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextImageMenuUI : ImageMenuUI<TextImageItem, TextMenuItemEventArg> {

	const string ResourcePath = "UI/ImageTextMenu";
	new public static TextImageMenuUI GetInstance() {
		Debug.Log (ResourcePath);
		var obj = GameObjectUtil.GetResourceInstance<TextImageMenuUI> (ResourcePath);
		Debug.Log (obj);
		return obj;
	}

	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, TextImageItem item)
	{
		base.InitializeButton (obj, item);
		obj.GetComponentInChildren<Text> ().text = item.text;
	}

	protected override TextMenuItemEventArg createEventArg (GameObject obj)
	{
		return new TextMenuItemEventArg (obj.GetComponentInChildren<Text> ().text);
	}
	#endregion
	
}
