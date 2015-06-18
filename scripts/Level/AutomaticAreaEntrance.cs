using UnityEngine;
using System.Collections;

public class AutomaticAreaEntrance : MonoBehaviour {

    public int areaID = 0;

    void OnTriggerEnter(Collider other) {
		if (PlayerData.Instance.Flags.GetFlag (FlagPlayerData.IsMultiplayer)) {
			return;
		}

        //Debug.Log(other + "; " + other.IsPlayer());
        if (other.IsPlayer()) {
            var area = GameData.Instance.NavigationData.Areas.GetItem(areaID);
            AreaManager.TransitionToArea(area);
        }
    }

}
