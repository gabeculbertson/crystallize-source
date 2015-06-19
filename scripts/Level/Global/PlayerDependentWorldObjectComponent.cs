using UnityEngine;
using System.Collections;

[SelectionBase]
[ExecuteInEditMode]
public class PlayerDependentWorldObjectComponent : MonoBehaviour, IGlobalID {

    [SerializeField]
    int[] worldObjectIDs = new int[]{-1, -1};

    public int PlayerCount {
        get {
            return worldObjectIDs.Length;
        }
    }

    public int GlobalID {
        get {
            return GetID(PlayerManager.main.PlayerID);
        }
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < PlayerCount; i++) {
            if (GetID(i) == -1) {
                if (!Application.isEditor) {
                    Debug.LogError("ID must be initiallized or editor.");
                    return;
                }
                GetNewID(i);
            }
        }
	}

    public int GetID(int index) {
        return worldObjectIDs[index];
    }

    public void GetNewID(int index) {
        worldObjectIDs[index] = GameData.Instance.WorldData.GetNewID();
    }

    public void SetID(int index, int id) {
        this.worldObjectIDs[index] = id;
    }
	
}
