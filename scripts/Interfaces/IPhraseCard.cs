using UnityEngine;
using System.Collections;

public interface IPhraseCard {

	Rect Region { get; }
	Vector2 Center { get; set; }
	bool IsSolved { get; }

}
