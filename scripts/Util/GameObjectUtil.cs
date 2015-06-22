using UnityEngine; 
using System.Collections; 

public static class GameObjectUtil {

    public static GameObject GetResourceInstance(string resourcePath) {
        return GameObject.Instantiate<GameObject>(
            Resources.Load<GameObject>(resourcePath));
    }

    public static C GetResourceInstance<C>(string resourcePath) where C : Component {
        return GameObject.Instantiate<GameObject>(
            Resources.Load<GameObject>(resourcePath))
            .GetComponent<C>();
    }

}
