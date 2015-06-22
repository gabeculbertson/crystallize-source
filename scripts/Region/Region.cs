using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class Region : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        Debug.Log("Set region");
        if (other.IsPlayer()) {
            if (RegionManager.Instance) {
                RegionManager.Instance.SetRegion(this);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.IsPlayer()) {
            if (RegionManager.Instance) {
                RegionManager.Instance.SetRegion(null);
            }
        }
    }

}
