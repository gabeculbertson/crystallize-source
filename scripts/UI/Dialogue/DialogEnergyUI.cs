using UnityEngine;
using System.Collections;
using Crystallize;

public class DialogEnergyUI : MonoBehaviour {

	public static DialogEnergyUI main { get; set; }

	public UIEnergyRect energy;
	public InteractiveDialogActor target;

	void OnEnable(){
		energy.currentEnergy = 0;
	}

	void Awake(){
		main = this;
	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			transform.position = Camera.main.WorldToScreenPoint(target.transform.position) - Vector3.up * 30f;
			//energy.energy = Mathf.Clamp(target.currentTrust / target.totalTrust, 0, 1f);
		}
	}

	public void Open(InteractiveDialogActor target){
		this.target = target;
		gameObject.SetActive (true);
	}

	public void Close(){
		gameObject.SetActive (false);
	}

}
