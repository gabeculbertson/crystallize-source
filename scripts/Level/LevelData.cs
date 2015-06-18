using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelData {

    public HashSet<int> UnlockedAreas { get; set; }
	public List<LevelStateData> Levels { get; set; }

	public LevelData(){
        UnlockedAreas = new HashSet<int>();
		Levels = new List<LevelStateData>();
	}

	public LevelState GetLevelState(string levelName){
		var state = GetLevelStateData (levelName);
		if (state == null) {
			return LevelState.Locked;
		}
		return state.LevelState;
	}

	public LevelStateData GetLevelStateData(string levelName){
		return (from l in Levels where l.LevelName == levelName select l).FirstOrDefault ();
	}

	public void SetLevelState(string levelName, LevelState levelState){
		var level = GetLevelStateData (levelName);
		if (level == null) {
			level = new LevelStateData (levelName, levelState);
			Levels.Add (level);
		} else {
			level.LevelState = levelState;
		}
	}

    public bool GetAreaUnlocked(int areaID) {
        var a = GameData.Instance.NavigationData.Areas.GetItem(areaID);
        if (a.Cost == 0) {
            return true;
        }
        return UnlockedAreas.Contains(areaID);
    }

    public void SetAreaUnlocked(int areaID, bool unlocked) {
        if (unlocked) {
            if (!UnlockedAreas.Contains(areaID)) {
                UnlockedAreas.Add(areaID);
            }
        } else {
            if (UnlockedAreas.Contains(areaID)) {
                UnlockedAreas.Remove(areaID);
            }
        }
    }

}
