using UnityEngine;
using System.Collections;

public class LevelSettings : MonoBehaviour {

    static LevelSettings _main;

	public static LevelSettings main {
        get
        {
            if (_main == null)
            {
                var settingsGO = new GameObject("LevelSettings");
                settingsGO.AddComponent<LevelSettings>();
            }
            return _main;
        }
        set
        {
            _main = value;
        }
    }

	public static bool CanRankUp(int wordID){
		if (!main) {
			return false;
		}

		if (main.maxPhraseRank == 0) {
			return true;
		}
		
		return Mathf.RoundToInt (PhraseEvaluator.GetPhraseLevel (wordID)) <= main.maxPhraseRank;
	}

    public static bool AutomaticallyTransition() {
        if (main.useQuesting) {
            if (GameSettings.GetFlag(GameSystemFlags.LockQuestInterdependence)) {
                return false;
            }
        }
        return true;
    }

	void Awake(){
		main = this;
	}

	public string nextLevel = "";
	public int maxPhraseRank = 0;
	public bool useQuesting = false;
	public bool disableMusic = false;
	public bool ignoreGlobalLayout = false;
    public bool canRankUp = false;
	public int areaID = -1;
	public string areaName;

    public bool allowFreeInputUI = false;
    public bool allowItemInventoryUI = false;
    public bool allowCompass = false;
	public bool allowSRS = false;
	public bool allowLevelUp = false;

    public bool isMultiplayer = false;
    public bool hidePartner = false;

    public bool RequireTranslation {
        get {
            return canRankUp && !isMultiplayer;
        }
    }

}
