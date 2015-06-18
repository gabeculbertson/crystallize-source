using UnityEngine;
using System.Collections;

namespace Crystallize.Experiment {
	public class ExperimentModule : MonoBehaviour {

        public string firstLevel = "";
		public GameObject[] moduleResources;

		void Awake(){
            Debug.Log("Loading experiment module: " + name);
            DataLogger.LogTimestampedData("ExperimentCondition", name.Replace("(Clone)", ""), GameSettings.Instance.ExperimentCondition.ToString());
			DontDestroyOnLoad (this);
			CrystallizeEventManager.OnInitialized += HandleSceneInitialized;

            if (firstLevel != "") {
                if (PlayerData.Instance.Location.AreaID == LocationPlayerData.DefaultAreaID) {
                    PlayerData.Instance.Location.AreaID = AreaManager.GetAreaIDForLevelID(firstLevel);
                }
            }
		}
		
		void OnDestroy(){
			CrystallizeEventManager.OnInitialized -= HandleSceneInitialized;
		}

		void HandleSceneInitialized(object sender, System.EventArgs args){
			foreach (var r in moduleResources) {
				Instantiate(r);
			}
		}

	}
}