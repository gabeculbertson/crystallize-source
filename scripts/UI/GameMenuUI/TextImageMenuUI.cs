using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextImageMenuUI : ImageMenuUI<TextImageItem, TextMenuItemEventArg> {

	const string ResourcePath = "UI/ImageTextMenu";
	new public static TextImageMenuUI GetInstance() {
		var obj = GameObjectUtil.GetResourceInstance<TextImageMenuUI> (ResourcePath);
		return obj;
	}

	//a local cache of the original text given to the menu
	//this will not be updated when the shown text is changed
	//can be used for purposes like hidden answer key
	private string myText;

	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, TextImageItem item)
	{
		base.InitializeButton (obj, item);
		myText = item.text;
		if(item.showText){
			Text objText = obj.GetComponentInChildren<Text> ();
			objText.text = item.text;
		}
	}

	protected override TextMenuItemEventArg createEventArg (GameObject obj)
	{
		return new TextMenuItemEventArg (myText);
	}
	#endregion
	
}
