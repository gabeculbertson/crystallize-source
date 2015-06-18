using UnityEngine;
using System.Collections;

public class LevelSystemConstructor : MonoBehaviour {

	public static LevelSystemConstructor main { get; set; }

	public LevelSystemData levelSystemData;
	public bool construct = true;

	// Use this for initialization
	void Awake () {
		main = this;
		if (construct) {
			ConstructLevelSystem ();
		}
	}

	void OnDestroy(){
		main = null;
	}
	
	void ConstructLevelSystem(){
		var system = gameObject;// new GameObject ("System");
		var subsystem = new GameObject ("SubSystems");
		var ui = new GameObject ("UI");

		subsystem.transform.parent = system.transform;
		ui.transform.parent = system.transform;

		InstantiateChild (subsystem, levelSystemData.fieldSystemPrefab);
		//InstantiateChild (subsystem, levelSystemData.gridSystemPrefab);
		InstantiateChild (subsystem, levelSystemData.objectiveManagerPrefab);

		InstantiateChild (ui, levelSystemData.palletPrefab);
		InstantiateChild (ui, levelSystemData.eventSystemPrefab);
		//InstantiateChild (ui, levelSystemData.menuSwapperPrefab);
		InstantiateChild (ui, levelSystemData.uiPrefab);
		InstantiateChild (ui, levelSystemData.worldUIPrefab);
	}

	void InstantiateChild(GameObject parent, GameObject prefab){
		var go = Instantiate (prefab) as GameObject;
		go.transform.SetParent(parent.transform);
	}

}
