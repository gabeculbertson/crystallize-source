using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class ConfirmMenuUI<T, V> : SelectionMenuUI<T, V> 
	where V : MenuItemEventArg
	where T : GameMenuItem
{

	public UIButton button;

	public override void Initialize(List<T> item){
		base.Initialize (item);
		button.OnClicked += OnClick;
	}
	
	void OnClick(object sender, EventArgs e){
		RaiseComplete ();
		GameObject.Destroy (this);
	}
}
