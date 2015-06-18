using UnityEngine;
using System.Collections;

public class PhraseHolder : ISerializableDictionaryItem<int> {

	public int Key {
		get {
			return WorldObjectID;
		}
	}
	
	public int WorldObjectID { get; set; }
	public PhraseSequence Phrase { get; set; }

	public PhraseHolder(){
		WorldObjectID = -1;
		Phrase = new PhraseSequence ();
	}

	public PhraseHolder(int worldID) : this(){
		WorldObjectID = worldID;
	}

}
