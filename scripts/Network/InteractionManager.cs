using UnityEngine;
using System.Collections;

public class InteractionManager {

    public static bool IsInteractingWithOtherPlayer() {
        if (!Network.isClient && !Network.isServer) {
            return false;
        }

        var p1 = GameObject.FindGameObjectWithTag("Player");
        var p2 = GameObject.FindGameObjectWithTag("OtherPlayer");
        if (!(p1 && p2)) {
            return false;
        }

        if (Vector3.Distance(p1.transform.position, p2.transform.position) > 5f) {
            return false;
        }

        return true;
    }

    public static GameObject GetInteractionTarget() {
        if (GameSettings.Instance.ExperimentModule == GameSettings.InterdependenceModule) {
            return AllyGameObject();
        }

        if (DialogueSystemManager.main.InteractionTarget) {
            return DialogueSystemManager.main.InteractionTarget;
        }

        if (IsInteractingWithOtherPlayer()) {
            return AllyGameObject();
        }

        return null;
    }

    public static GameObject AllyGameObject() {
        if (PlayerManager.main.PlayerID == 0) {
            var go = GameObject.FindGameObjectWithTag("OtherPlayer");
            if (go) {
                if (go.transform.position.y > -500f) {
                    return go;
                }
            }
        } else {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) {
                if (go.transform.position.y > -500f) {
                    return go;
                }
            }
        }
        return null;
    }

}
