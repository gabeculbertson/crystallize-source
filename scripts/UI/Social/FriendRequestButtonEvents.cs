using UnityEngine;
using System.Collections;

public class FriendRequestButtonEvents : MonoBehaviour {

	public void Add(){
		transform.parent.gameObject.GetComponent<NetworkCardUI>().BeginAddFriendSequence();
		Destroy (gameObject);
	}

	public void Remove(){
		Destroy (transform.parent.gameObject);
	}

}
