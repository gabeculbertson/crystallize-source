using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ImageMenuUI<T, V> : ConfirmMenuUI<T, V> 
	where V : MenuItemEventArg
	where T : ImageMenuItem

{
	#region implemented abstract members of SelectionMenuUI
	protected override void InitializeButton (GameObject obj, T item)
	{
		Image image = obj.GetComponent<Image> ();
		image.sprite = item.Image;
	}
	#endregion
		
}
