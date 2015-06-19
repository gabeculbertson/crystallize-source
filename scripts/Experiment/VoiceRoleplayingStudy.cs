using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Crystallize.Experiment {
    public class VoiceRoleplayingStudy : MonoBehaviour {

        public GameObject nativeSpeakerUI;

        List<GameObject> instances = new List<GameObject>();

        // Use this for initialization
        void Start() {
            GameSettings.SetFlag(GameSystemFlags.LockDialogueSystem, true);

            CrystallizeEventManager.Network.OnConnectedToNetwork += main_OnConnectedToNetwork;
        }

        void main_OnConnectedToNetwork(object sender, System.EventArgs e) {
            foreach (var i in instances) {
                Destroy(i);
            }
            PlayerController.UnlockMovement(this);

            if (PlayerManager.main.PlayerID == 1) {
                var inst = Instantiate<GameObject>(nativeSpeakerUI);
                instances.Add(inst);
                PlayerController.LockMovement(this);
                OmniscientCamera.main.player = PlayerManager.main.OtherGameObject.transform;
                PlayerManager.main.PlayerGameObject = PlayerManager.main.OtherGameObject;
            }
        }

    }
}
