using UnityEngine;
using System.Collections;

public class AreaManager : MonoBehaviour {

	const string FirstSingleplayerLevelID = "Tutorial_Level01";
    const string FirstMultiplayerLevelID_AllowEnglish = "SchoolHallway_Multiplayer_English";
    const string FirstMultiplayerLevelID_ForbidEnglish = "SchoolHallway_Multiplayer01";

    static GameObject main;

    public static int SourceAreaID { get; set; }

    public static void TransitionToArea(AreaGameData area) {
        if (!main) {
            main = new GameObject("_AreaTransition");
            main.AddComponent<AreaManager>();
        }
        main.GetComponent<AreaManager>().StartCoroutine(TransitionToAreaSequence(area));
    }

    public static IEnumerator TransitionToAreaSequence(AreaGameData area) {
        if (EffectManager.main) {
            GameObject.Instantiate(EffectLibrary.Instance.uiFadeOutEffect);
            yield return new WaitForSeconds(1f);
        }

		CrystallizeEventManager.Environment.RaiseBeforeSceneChange (null, System.EventArgs.Empty);
        PlayerDataLoader.Save();

        Application.LoadLevel(area.LevelName);
        DataLogger.LogTimestampedData("ChangeArea", area.AreaID.ToString());
    }

    public static int GetCurrentAreaID() {
        return GetAreaIDForLevelID(Application.loadedLevelName);
    }

    public static int GetAreaIDForLevelID(string levelID) {
        foreach (var area in GameData.Instance.NavigationData.Areas.Items) {
            if (area.LevelName == levelID) {
                return area.AreaID;
            }
        }
        return -1;
    }

    public static AreaGameData GetAreaForLevelID(string levelID) {
        return GameData.Instance.NavigationData.Areas.GetItem(GetAreaIDForLevelID(levelID));
    }

	public static int GetFirstAreaID(){
		if (PlayerData.Instance.Flags.GetFlag (FlagPlayerData.IsMultiplayer)) {
			return GetAreaIDForLevelID(FirstSingleplayerLevelID);
		}

        if (PlayerData.Instance.AllowEnglish) {
            return GetAreaIDForLevelID(FirstMultiplayerLevelID_AllowEnglish);
        } else {
            return GetAreaIDForLevelID(FirstMultiplayerLevelID_ForbidEnglish);
        }
	}

}
