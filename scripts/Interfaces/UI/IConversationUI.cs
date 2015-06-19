using UnityEngine;
using System.Collections;

public interface IConversationUI {

	GameObject gameObject { get; }
	RectTransform GetSlot(int slot);
	bool IsLocked { get; set; }
	bool ShowMessages { get; set; }
	void Open();

}
