using UnityEngine;
using System.Collections;

public class SetAnimation : MonoBehaviour {

	public string animationFlag;
	public bool isState = true;
	public Animator animator;

	// Use this for initialization
	void Start () {
		if (!animator) {
			animator = GetComponentInChildren<Animator>();
		}

		if (isState) {
			animator.Play (animationFlag);
		} else {
			animator.SetBool (animationFlag, true);
		}
	}
	

}
