using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[SelectionBase]
public class WorldObjectComponent : MonoBehaviour, IGlobalID {

	[SerializeField]
	int worldObjectID = -1;

	public int GlobalID {
		get {
			return worldObjectID;
		}
	}
	
	void Start () {
		if (worldObjectID != -1) {
			return;
		}

		if (!Application.isEditor) {
			Debug.LogError("ID must be initiallized or editor.");
		}

        GetNewID();
	}

    public void GetNewID() {
        worldObjectID = GameData.Instance.WorldData.GetNewID();
    }

    public int GetID() {
        return worldObjectID;
    }

    public void SetID(int id) {
        this.worldObjectID = id;
    }

}
