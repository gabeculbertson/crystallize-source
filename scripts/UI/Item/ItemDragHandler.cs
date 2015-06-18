using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemDragHandler : MonoBehaviour {

    public static ItemDragHandler main { get; set; }

    public GameObject itemDragPrefab;

    GameObject itemDragInstance;
    int itemID;

    public bool IsDragging { get; set; }

    void Awake() {
        main = this;
    }

	// Use this for initialization
	void Start () {
        CrystallizeEventManager.UI.OnItemDragStarted += HandleItemDragStarted;
        CrystallizeEventManager.UI.OnItemDropped += HandleOnItemDropped;
	}

    void HandleOnItemDropped(object sender, ItemDragEventArgs e) {
        if (itemDragInstance) {
            Destroy(itemDragInstance);
        }
    }

    void HandleItemDragStarted(object sender, ItemDragEventArgs e) {
        if (itemDragInstance) {
            Destroy(itemDragInstance);
        }

        itemDragInstance = Instantiate(itemDragPrefab) as GameObject;
        itemDragInstance.GetComponent<ItemDragInstanceUI>().Initialize(e.ItemID);
        itemDragInstance.transform.SetParent(TutorialCanvas.main.transform);
        itemID = e.ItemID;
        IsDragging = true;
        PlayerController.LockMovement(this);
    }
	
	// Update is called once per frame
	void Update () {
        if (itemDragInstance) {
            if (IsDragging) {
                itemDragInstance.transform.position = Input.mousePosition;
            } else {
                var cg = itemDragInstance.GetComponent<CanvasGroup>();
                cg.alpha = Mathf.MoveTowards(cg.alpha, 0, Time.deltaTime);
                if (cg.alpha == 0) {
                    Destroy(itemDragInstance);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && IsDragging) {
            IsDragging = false;
            PlayerController.UnlockMovement(this);

            var accepted = false;
            var raycastResults = new List<RaycastResult>();
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastResults);
            foreach (var r in raycastResults) {
                var itemAcceptor = r.gameObject.GetInterface<IItemDropAcceptor>();
                if (itemAcceptor != null) {
                    itemAcceptor.AcceptDrop(itemID, itemDragInstance);
                    if (itemDragInstance) {
                        Destroy(itemDragInstance);
                    }
                    accepted = true;
                }
            }

            if (!accepted) {
                CrystallizeEventManager.UI.RaiseItemDiscarded(this, new ItemDragEventArgs(itemID));
            }
        }
	}

}
