using UnityEngine;
using System.Collections;

namespace Crystallize.Experiment {
    public class ExperimentModuleLoader : MonoBehaviour {

        public GameObject[] experimentModules = new GameObject[0];

        // Use this for initialization
        void Start() {
            foreach (var m in experimentModules) {
                if (m.name == GameSettings.Instance.ExperimentModule) {
                    Instantiate(m);
                    break;
                }
            }
        }

    }
}