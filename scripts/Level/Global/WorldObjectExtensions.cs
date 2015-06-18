using UnityEngine;
using System.Collections;

public static class WorldObjectExtensions {

    public static bool HasWorldID(this Component component) {
        return GetWorldID(component) != -1;
    }

    public static int GetWorldID(this Component component) {
        var gid = component.gameObject.GetInterface<IGlobalID>();
        if (gid != null) {
            return gid.GlobalID;
        }
        return -1;
    }

    public static Transform GetWorldObject(int worldID) {
        foreach (var wo in GameObject.FindObjectsOfType<WorldObjectComponent>()) {
            if (wo.GlobalID == worldID) {
                return wo.transform;
            }
        }
        return null;
    }

}
