using UnityEngine;
using System;
using System.Collections;

public class MainCanvas : MonoBehaviour {

    static MainCanvas _main;

    public static MainCanvas main
    {
        get
        {
            if (!_main)
            {
                _main = Instantiate<GameObject>(Resources.Load<GameObject>("MainCanvas")).GetComponent<MainCanvas>();
            }
            return _main;
        }
        set
        {
            _main = value;
        }
    }
	
	public GameObject notificationPanelPrefab;
	public GameObject mainMenu;

	GameObject notificationPanelInstance;

	public event EventHandler NextLevelForced;

	public float BottomHeight { get; set; }

	// Use this for initialization
	void Awake () {
		main = this;
	}

	void Start(){
		if (mainMenu) {
			mainMenu.SetActive (false);
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.KeypadMultiply)) {
			if(NextLevelForced != null){
				NextLevelForced(this, EventArgs.Empty);
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			mainMenu.SetActive(!mainMenu.activeSelf);
		}
	}

	public RectTransform OpenNotificationPanel(string text, float duration = 0){
		if (notificationPanelInstance) {
			Destroy(notificationPanelInstance);
		}

		//if (!notificationPanelInstance) {
		notificationPanelInstance = Instantiate(notificationPanelPrefab) as GameObject;
		notificationPanelInstance.transform.SetParent(transform);
		notificationPanelInstance.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
		notificationPanelInstance.transform.position = new Vector3(Screen.width * 0.5f, Screen.height - 200f, 0);
		if(duration > 0){
			notificationPanelInstance.GetComponent<EnergizeInEffect>().Lifetime = duration;	
			//Destroy(notificationPanelInstance, duration);
		}
		//}

		AudioManager.main.PlayMessage ();
		notificationPanelInstance.GetInterface<INotificationPanel> ().Reset(text);
		return notificationPanelInstance.GetComponent<RectTransform> ();
	}

	public void CloseNotificationPanel(){
		if (notificationPanelInstance) {
			Destroy(notificationPanelInstance);
		}
	}

}
