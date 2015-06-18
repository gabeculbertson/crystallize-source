using UnityEngine;
using System.Collections;

public interface IPhraseContainer {

	GameObject gameObject { get; }
	PhraseSequence Phrase { get ; }

}
