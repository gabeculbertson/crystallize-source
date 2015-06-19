using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotInventoryState {

	public int Level { get; set; }
	public int CurrentLevelExperience { get; set; }
	public int NextLevelExperience { get; set; }
	public List<string> WordIDs { get; set; }

	public SlotInventoryState(){
		CurrentLevelExperience = 0;
		NextLevelExperience = 10;
		WordIDs = new List<string> ();
		//WordIDs.Add(null);
	}

}
