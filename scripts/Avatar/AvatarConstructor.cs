using UnityEngine;
using System.Collections;

public class AvatarConstructor : MonoBehaviour {

    bool male = false;
    GameObject instance;

	// Use this for initialization
	void Start () {
        UpdateAvatar();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            male = !male;
            UpdateAvatar();
        }
	}

    void UpdateAvatar()
    {
        if (instance)
        {
            Destroy(instance);
        }

        instance = AvatarComponentLibrary.Instance.GetAvatarInstance(male);
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }

}
