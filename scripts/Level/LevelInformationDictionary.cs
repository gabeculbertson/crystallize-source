using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelInformationDictionary : ScriptableObject {

	[SerializeField]
	List<LevelInformation> levelInformation = new List<LevelInformation>();

	public IEnumerable<LevelInformation> Levels {
		get {
			return levelInformation;
		}
	}

	public LevelInformation GetLevelInformation(string levelID){
		return (from i in levelInformation where i.levelID == levelID select i).FirstOrDefault ();
	}

}
