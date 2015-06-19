using UnityEngine;
using System.Collections;

public enum NetworkPlayerType {
    Player1 = 0,
    Player2 = 1
}

public class NetworkDependentObject : MonoBehaviour {

    public NetworkPlayerType playerType = NetworkPlayerType.Player1;

	// Use this for initialization
	IEnumerator Start () {
        yield return null;  

        CrystallizeEventManager.Network.OnConnectedToNetwork += HandleConnectedToNetwork;

        if (playerType != NetworkPlayerType.Player1) {
            gameObject.SetActive(false);
        }
	}

    void HandleConnectedToNetwork(object sender, System.EventArgs e) {
        gameObject.SetActive((int)playerType == PlayerManager.main.PlayerID);
    }
	
}
