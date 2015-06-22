using UnityEngine;
using System.Collections;

public class ArmAnimationController : MonoBehaviour {

    public bool IsHolding {
        get {
            return holdableInstance;
        }
    }

    public Transform handTarget;

    Animator animator;
    GameObject holdableInstance;
    float holdableForwardOffset;
    float holdableVerticalOffset;

    bool intialized = false;

    // Use this for initialization
    IEnumerator Start() {
        yield return null;

        animator = GetComponentInChildren<Animator>();
        var i = PlayerData.Instance.Item;
        if (i != "") {
            HoldItem(i);
        }

        intialized = true;
    }

    // Update is called once per frame
    void LateUpdate() {
        if (!intialized) {
            return;
        }

        if (IsHolding) {
            animator.SetLayerWeight(1, 1f);
            holdableInstance.transform.forward = -transform.forward;
            holdableInstance.transform.position = handTarget.position + transform.forward * holdableForwardOffset + Vector3.up * (holdableVerticalOffset + 0.1f);
        } else {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void HoldItem(string itemID) {
        PlayerData.Instance.Item = itemID;
        CrystallizeEventManager.UI.RaiseItemChanged(this, new StringEventArgs(itemID));

        if (IsHolding) {
            StopHolding();
        }

        holdableInstance = Instantiate(ScriptableObjectDictionaries.main.holdableDictionary.GetHoldable(itemID).prefab) as GameObject;
        holdableInstance.transform.SetParent(transform);
        holdableForwardOffset = holdableInstance.GetComponent<BoxCollider>().size.z * 0.5f;
        holdableVerticalOffset = -holdableInstance.GetComponent<BoxCollider>().center.y;
    }

    public void StopHolding() {
        PlayerData.Instance.Item = "";

        if (holdableInstance) {
            Destroy(holdableInstance);
            holdableInstance = null;
        }
    }

}
