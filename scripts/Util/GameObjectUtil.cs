using UnityEngine; 
using System.Collections; 

public static class GameObjectUtil {

    public static C GetResourceInstance<C>(string resourcePath) where C : Component {
        return GameObject.Instantiate<GameObject>(
            Resources.Load<GameObject>(resourcePath))
            .GetComponent<C>();
    }

}
