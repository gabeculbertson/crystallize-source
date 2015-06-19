using UnityEngine;
using System.Collections;
using Crystallize;

namespace Crystallize.Animation {
	public class PhraseSegmentVisualization : AnimationPhaseScript {

		public GameObject appearancePrefab;

		GameObject objectInstance;

		// Use this for initialization
		void Start () {
		
		}

		void LateUpdate(){
			if (objectInstance) {
				objectInstance.transform.position =
                        PlayerManager.main.PlayerGameObject.transform.position
						+ Vector3.up * 3f
						+ Vector3.up * 0.25f * Mathf.Sin(Time.time);
			}
		}

		public void SetVisualization(object sender, PhraseEventArgs args){
			if (args.PhraseData.Prefab) {
				objectInstance = (GameObject)Instantiate (args.PhraseData.Prefab);
				objectInstance.transform.rotation = Quaternion.Euler(0, 135f, 0);
				if(appearancePrefab){
					var go = (GameObject)Instantiate(appearancePrefab);
					go.transform.parent = objectInstance.transform;
					go.transform.localPosition = Vector3.zero;
				}
			}
		}

		public void ClearVisualization(object sender, PhraseEventArgs args){
			Destroy (objectInstance);
			objectInstance = null;
		}

	}
}