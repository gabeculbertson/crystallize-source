using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class ConfirmMenuUI<T, V> : SelectionMenuUI<T, V>
	where V : MenuItemEventArg
{

	public UIButton button;

	public override void Initialize(List<T> item){
		base.Initialize (item);
		button.transform.SetParent (transform);
		button.transform.SetAsLastSibling ();
		button.transform.localPosition = new Vector3 (0f, 0f, 0f);
		button.GetComponentInChildren<RectTransform> ().anchoredPosition = new Vector2 (0f, 0f);
		button.GetComponentInChildren<RectTransform> ().localPosition = new Vector3 (0f, 0f, 0f);		
		button.OnClicked += OnClick;
	}
	
	void OnClick(object sender, EventArgs e){
		if (HasSelection())
			return;
		RaiseComplete ();
		GameObject.Destroy (this.gameObject);
	}
}
