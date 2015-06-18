using UnityEngine;
using System.Collections;
using Crystallize;

public interface IDropAcceptor {

	Rect DropArea { get; }

	void SetHovering (PhraseSegmentInstance phrase);
	bool AcceptDrop (PhraseSegmentInstance phrase);
	void RemovePhraseInstance(PhraseSegmentInstance phrase);

}
