using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConversationDictionary : ScriptableObject {

	public string path = "Crystallize/ScriptableObjects";

	[SerializeField]
	List<FixedPlayerDialog> conversations = new List<FixedPlayerDialog>();

	Dictionary<int, FixedPlayerDialog> _conversationForID;

	Dictionary<int, FixedPlayerDialog> ConversationForID {
		get {
			if (_conversationForID == null) {
				UpdateDictionary ();
			}
			return _conversationForID;
		}
	}

	public IEnumerable<FixedPlayerDialog> Conversations {
		get {
			return conversations;
		}
	}

	public void SetConversations(List<FixedPlayerDialog> conversations){
		this.conversations = conversations;
	}

	public FixedPlayerDialog GetConversationForID(int id){
		if (ConversationForID.ContainsKey (id)) {
			return ConversationForID[id];
		}
		return null;
	}

	public bool ContainsID(int id){
		return ConversationForID.ContainsKey (id);
	}

	void UpdateDictionary(){
		_conversationForID = new Dictionary<int, FixedPlayerDialog> ();
		foreach (var conv in conversations) {
			if(_conversationForID.ContainsKey(conv.id)){
				Debug.LogWarning("Conversation dictionary already contains id " + conv.id + ": " + conv.name);
			}
			_conversationForID[conv.id] = conv;
		}
	}

}
