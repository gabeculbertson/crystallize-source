using UnityEngine;
using System.Collections;
using Crystallize;

public interface IDragBehavior {
	
	void HandleClick(IDropSource source, PhraseSegmentInstance instance);
	void HandleDrop(IDropSource source, PhraseSegmentInstance instance);
	void HandleDrag(IDropSource source, PhraseSegmentInstance instance);

}
