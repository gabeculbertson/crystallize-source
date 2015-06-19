using UnityEngine;
using System.Collections;

public class OutdoorSceneManager : LevelArea {

	Transform lastTarget;

	IndoorArea[] areas;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		areas = GetComponentsInChildren<IndoorArea> ();
		//Debug.Log ("Areas: " + areas.Length);
	}
	
	// Update is called once per frame
	void Update () {
		var thisTarget = transform;
        var playerPosition = PlayerManager.main.PlayerGameObject.transform.position;
		foreach (var area in areas) {
            if (!area.enabled) {
                continue;
            }

			if(area.ContainsPlayer(playerPosition)){
				thisTarget = area.transform;
				break;
			}
		}

		if(thisTarget != lastTarget){
			SetObjectsActive(false);
			foreach(var area in areas) {
				area.SetObjectsActive(false);
			}

			//Debug.Log("setting " + thisTarget + " active");
			thisTarget.GetComponent<LevelArea>().SetObjectsActive(true);
			lastTarget = thisTarget;
		}
	}

}
