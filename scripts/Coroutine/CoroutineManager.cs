using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class CoroutineManager : MonoBehaviour {

    static CoroutineManager _instance;

    public static CoroutineManager Instance {
        get {
            if (!_instance) {
                _instance = new GameObject("Coroutines").AddComponent<CoroutineManager>();
            }
            return _instance;
        }
    }

}
