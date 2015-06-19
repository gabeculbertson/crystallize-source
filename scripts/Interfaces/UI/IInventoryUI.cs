using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IInventoryUI {

	GameObject gameObject { get; }
	IEnumerable<RectTransform> Entries { get; }
	float Height { get; }
	RectTransform GetEntry(PhraseSequenceElement phrase);

}
