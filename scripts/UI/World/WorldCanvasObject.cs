using UnityEngine;
using System.Collections;

public class WorldCanvasObject : MonoBehaviour {

    public GameObject prefab;
    public int id;

	// Use this for initialization
	IEnumerator Start () {
        while (!WorldCanvas.main) {
            yield return null;
        }

        var go = Instantiate(prefab) as GameObject;
        go.transform.SetParent(WorldCanvas.main.transform);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.GetComponentInChildren<AreaEntrance>().destinationAreaID = id;
        Destroy(gameObject);
	}
	
}
