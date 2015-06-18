using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchedDialogue : ISerializableDictionaryItem<int> {

	public int Key {
		get {
			return ID;
		}
	}

	public int ID { get; set; }
	public List<BranchedDialogueElement> Elements { get; set; }

	public BranchedDialogue(){
		ID = -1;
		Elements = new List<BranchedDialogueElement> ();
	}

	public BranchedDialogue(int id) : this(){
		ID = id;
	}

	public void AddBranch(){
		Elements.Add(new BranchedDialogueElement());
	}

}
