using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConversationClientDictionary : ScriptableObject {

	public string path = "Crystallize/ScriptableObjects";
	public List<ConversationClientData> conversationClients = new List<ConversationClientData>();

	public ConversationClientData GetClientForID(string id){
		return (from c in conversationClients where c.ID == id select c).FirstOrDefault ();
	}

}
