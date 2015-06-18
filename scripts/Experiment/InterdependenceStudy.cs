using UnityEngine;
using System.Collections;

namespace Crystallize.Experiment {
    public class InterdependenceStudy : MonoBehaviour {

        public GameObject entrancePrefab;
        public GameObject continueUIPrefab;

        // Use this for initialization
        void Start() {
            if (GameSettings.Instance.ExperimentCondition == 0) {
                GameSettings.SetFlag(GameSystemFlags.LockQuestInterdependence, true);
            } else {
                GameSettings.SetFlag(GameSystemFlags.LockQuestInterdependence, false);
                gameObject.SetActive(false);
            }

            /*var area = AreaManager.GetAreaIDForLevelID(LevelSettings.main.nextLevel);
            var entrances = FindObjectsOfType<WorldCanvasObject>();
            Debug.Log("Entrances: " + entrances.Length);
            foreach (var e in entrances) {
                if (e.id == area) {
                    Debug.Log("Found: " + e);
                    var instance = Instantiate<GameObject>(entrancePrefab);
                    instance.transform.SetParent(WorldCanvas.main.transform);
                    instance.transform.position = e.transform.position;
                    instance.transform.rotation = e.transform.rotation;
                    instance.GetComponent<AreaEntrance>().destinationAreaID = area;
                    break;
                }
            }*/

            CrystallizeEventManager.Environment.OnPersonAnimationRequested += HandleOnPersonAnimationRequested;
        }

        void Update() {
            if (GameSettings.Instance.ExperimentCondition == 0) {
                if (ObjectiveManager.main && LevelSettings.main.useQuesting) {
                    if (ObjectiveManager.main.IsComplete) {
                        var instance = Instantiate<GameObject>(continueUIPrefab);
                        instance.GetComponent<ContinueConfirmationUI>().Initialize(AreaManager.GetAreaForLevelID(LevelSettings.main.nextLevel));
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        void HandleOnPersonAnimationRequested(object sender, PersonAnimationEventArgs e) {
            var id = PlayerManager.main.GetPlayerID(e.TargetObject);

            string text = null;
            switch (e.AnimationType) {
                case PersonAnimationType.Happy:
                    text = "nice!";
                    break;

                case PersonAnimationType.Wave:
                    text = "hi!";
                    break;

                case PersonAnimationType.Thanks:
                    text = "thanks :)";
                    break;
            }

            if (text != null) {
                CrystallizeEventManager.Network.RaiseEnglishLineInput(this, new TextEventArgs("Player " + (id + 1) + ": " + text));
            }
        }

    }
}
