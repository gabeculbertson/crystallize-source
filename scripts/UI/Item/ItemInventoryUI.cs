using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemInventoryUI : MonoBehaviour {

    const int MaxCount = 4;

    public GameObject emptyPrefab;
    public GameObject itemPrefab;
    public bool interactable = true;
    public bool usePlayerDataInventory = false;
    public List<int> _tmpInt = new List<int>();

    List<int> items = new List<int>();
    List<GameObject> entryInstances = new List<GameObject>();

    int sourceIndex = -1;
    int sourceID = -1;

    public event EventHandler OnItemsChanged;

    // Use this for initialization
	void Start () {
        if (LevelSettings.main) {
            if (!LevelSettings.main.allowItemInventoryUI) {
                Destroy(gameObject);
                return;
            }
        }

        for (int i = 0; i < MaxCount; i++) {
            items.Add(0);
        }

        if(usePlayerDataInventory){
            for (int i = 0; i < MaxCount; i++) {
                items[i] = PlayerManager.main.playerData.ItemInventory.GetItem(i);
            }
        }

        /*for (int i = 0; i < _tmpInt.Count; i++) {
            //items[i] = _tmpInt[i];
        }*/

        CrystallizeEventManager.UI.OnItemDiscarded += HandleItemDiscarded;
        CrystallizeEventManager.UI.OnItemAcquired += HandleItemAquired;

        UpdateInventory();
	}

    void HandleItemAquired(object sender, ItemDragEventArgs args) {
        if (usePlayerDataInventory) {
            for (int i = 0; i < MaxCount; i++) {
                items[i] = PlayerManager.main.playerData.ItemInventory.GetItem(i);
            }
            UpdateInventory();
        }
    }

    void HandleItemDiscarded(object sender, ItemDragEventArgs args) {
        if (sourceIndex >= 0) {
            SetItem(sourceIndex, sourceID);

            UpdateInventory();
        }
    }

    void ClearInstances() {
        foreach(var i in entryInstances){
            Destroy(i);
        }
        entryInstances.Clear();
    }

	void UpdateInventory () {
        ClearInstances();

        for (int i = 0; i < MaxCount; i++) {
            if (GetItem(i) == 0) {
                var inst = GetPrefabInstance(emptyPrefab);
                if (interactable) {
                    inst.GetComponent<EmptySlotUI>().OnItemDropped += HandleItemDropped;
                }
            } else {
                var inst = GetPrefabInstance(itemPrefab);
                if (interactable) {
                    inst.GetComponent<ItemButtonUI>().Initialize(GetItem(i));
                    inst.GetComponent<ItemButtonUI>().OnDragStarted += HandleDragStarted;
                } else {
                    inst.GetComponent<ItemDragInstanceUI>().Initialize(GetItem(i));
                }
            }
        }

        if (OnItemsChanged != null) {
            OnItemsChanged(this, EventArgs.Empty);
        }
	}

    void HandleDragStarted(object sender, System.EventArgs e) {
        var c = sender as Component;
        if (!c) {
            return; 
        }

        var i = entryInstances.IndexOf(c.gameObject);
        if (i >= 0) {
            sourceIndex = i;
            sourceID = GetItem(i);

            SetItem(i, 0);
            UpdateInventory();
        }
    }

    void HandleItemDropped(object sender, ItemDragEventArgs args) {
        var c = sender as Component;
        if (!c) {
            return;
        }
        
        var i = entryInstances.IndexOf(c.gameObject);
        if (i >= 0) {
            SetItem(i, args.ItemID);
            UpdateInventory();
        }
    }

    GameObject GetPrefabInstance(GameObject prefab) {
        var i = Instantiate(prefab) as GameObject;
        i.transform.SetParent(transform);
        entryInstances.Add(i);
        return i;
    }

    public void AddItem(int item) {
        for (int i = 0; i < MaxCount; i++) {
            if (GetItem(i) == 0) {
                SetItem(i, item);
                UpdateInventory();
                break;
            }
        }
    }

    public void SetItems(List<int> items) {
        if (items.Count != MaxCount) {
            Debug.Log("Unable to set items");
            return;
        }

        bool changed = false;
        for (int i = 0; i < MaxCount; i++) {
            if (GetItem(i) != items[i]) {
                SetItem(i, items[i]);
                changed = true;
            }
        }
        if (changed) {
            UpdateInventory();
        }
    }

    public List<int> GetItems() {
        return new List<int>(items);
    }

    public void SetEmpty() {
        for (int i = 0; i < MaxCount; i++) {
            SetItem(i, 0);
        }
        UpdateInventory();
    }

    void SetItem(int index, int itemID){
        if(usePlayerDataInventory){
            PlayerManager.main.playerData.ItemInventory.SetItem(index, itemID);
        } else {
            items[index] = itemID;
        }
    }

    int GetItem(int index){
        if(usePlayerDataInventory){
            return PlayerManager.main.playerData.ItemInventory.GetItem(index);
        } else {
            return items[index];
        }
    }

}
