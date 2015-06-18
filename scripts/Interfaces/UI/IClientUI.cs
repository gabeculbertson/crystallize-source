using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IClientUI {
	
	GameObject gameObject { get; }
	IEnumerable<RectTransform> Clients { get; }
	RectTransform GetEntry(ConversationClient client);

}
