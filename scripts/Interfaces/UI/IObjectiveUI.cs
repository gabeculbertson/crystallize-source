using UnityEngine;
using System.Collections;

public interface IObjectiveUI {

	GameObject gameObject { get; }
	RectTransform GetObjective(PhraseSequenceElement word);

}
