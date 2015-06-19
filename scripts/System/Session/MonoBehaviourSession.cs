using UnityEngine;
using System.Collections;

public class MonoBehaviourSession : MonoBehaviour, IDaySession {

    public event System.EventHandler OnComplete;

    public void Begin() {
        //var sessionGO = new GameObject("Session");
        //sessionGO.AddComponent(GetType().ToString());
    }

}
