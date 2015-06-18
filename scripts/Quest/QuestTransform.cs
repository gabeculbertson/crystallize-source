using UnityEngine;
using System.Collections;

public class QuestTransform : MonoBehaviour {

	public int clientID = -1;

	void Start(){
		QuestManager.main.RegisterClient (clientID, transform);
	}

}
