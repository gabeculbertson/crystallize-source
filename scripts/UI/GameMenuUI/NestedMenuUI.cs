using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NestedMenuUI<T> : ConfirmMenuUI<T, NestedMenuArg>
	where T : MenuAcceptor

{
	const string ResourcePath = "UI/NestedMenu";
	public static NestedMenuUI<T> GetInstance() {
		return GameObjectUtil.GetResourceInstance<NestedMenuUI<T>>(ResourcePath);
	}

	Dictionary<GameObject, T> dataDictionary;
	public override void Initialize (List<T> items)
	{
		buttonPrefab = new GameObject();
		dataDictionary = new Dictionary<GameObject, T>();
		/**
		 * load created menuObjects into the menu by instantiating the 
		 * gameObject with the info from the menuObjects
		 * TODO a lot of information passed around. Is it necessary to have one ScriptableObject
		 * and one GameObject while they are essentially the same?
		 **/
		foreach (T item in items) {
			GameObject instance = Instantiate<GameObject> (buttonPrefab);
			InitializeButton(instance, item);
			instance.transform.SetParent (transform);
			foreach (var c in instance.GetComponentsInChildren<RectTransform>()){
				c.anchoredPosition = new Vector2(0f, 0f);
			}
			instance.transform.localPosition = new Vector3 (0f, 0f, 0f);
			//assign attributes
			InitializeButton(instance, item);
			
			//hook event handler
			instance.GetComponent<UIButton>().OnClicked += MenuUI_OnClicked;
			dataDictionary.Add(instance, item);
		}
	}


	#region implemented abstract members of SelectionMenuUI

	protected override void InitializeButton (GameObject obj, T item)
	{
		obj = MenuItemBuilder.BuildMenuItemObject(item).GO;
	}

	protected override NestedMenuArg createEventArg (GameObject obj)
	{
		T data;
		if(dataDictionary.TryGetValue(obj, out data)){
			arg.Data = data;
		}
		else{
			//TODO handle exceptional case
			arg = null;
		}
		return arg;
	}

	#endregion
}
