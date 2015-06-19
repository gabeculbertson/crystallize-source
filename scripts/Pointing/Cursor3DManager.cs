using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Cursor3DManager : MonoBehaviour {

    public GameObject cursorPrefab;

    Dictionary<int, GameObject> cursorInstances = new Dictionary<int, GameObject>();

	// Use this for initialization
	void Start () {
        if (!LevelSettings.main.isMultiplayer) {
            Destroy(gameObject);
            return;
        }

        CrystallizeEventManager.UI.OnCursor3DPositionChanged += HandleCursor3DPositionChanged;
	}

    void HandleCursor3DPositionChanged(object sender, Cursor3DPositionChangedEventArgs e) {
        SetCursorPosition(e.PlayerID, e.Position);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            var raycastResults = new List<RaycastResult>();
			var eventData = new PointerEventData(EventSystem.current);
			eventData.position = Input.mousePosition;
			EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count == 0 && !UISystem.main.PhraseDragHandler.IsDragging) {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit)) {
                    CrystallizeEventManager.UI.RaiseCursor3DPositionChanged(this, new Cursor3DPositionChangedEventArgs(PlayerManager.main.PlayerID, hit.point));
                }
            }
        }
	}

    void SetCursorPosition(int playerID, Vector3 point) {
        var pgo = GetPlayerGameObject(playerID);
        CrystallizeEventManager.Environment.RaisePersonAnimationRequested(this, new PersonAnimationEventArgs(pgo, PersonAnimationType.Point));
        pgo.transform.forward = (point - pgo.transform.position).ToXZ();
        GetCursorInstance(playerID).transform.position = point;

        CrystallizeEventManager.PlayerState.RaiseGameEvent(this, new Cursor3DPositionChangedEventArgs(playerID, point));
    }

    GameObject GetPlayerGameObject(int playerID) {
        return PlayerManager.main.GetPlayerGameObject(playerID);
    }

    GameObject GetCursorInstance(int playerID) {
        if (!cursorInstances.ContainsKey(playerID)) {
            var go = Instantiate<GameObject>(cursorPrefab);
            go.GetComponent<Cursor3DBehavior>().Initialize(playerID);

            var c = GUIPallet.main.otherColor;
            if(playerID == PlayerManager.main.PlayerID){
                c = GUIPallet.main.selfColor;
            }
            float emission = 0.1f;
            go.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", c * Mathf.LinearToGammaSpace(emission));

            cursorInstances[playerID] = go;
        }
        return cursorInstances[playerID];
    }

}
