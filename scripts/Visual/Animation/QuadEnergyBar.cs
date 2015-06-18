using UnityEngine;
using System;
using System.Collections;

namespace Crystallize.Visual {
	public class QuadEnergyBar : MonoBehaviour {

		public Transform parentBackground;
		public float amount = 0.5f;
		public GameObject source;

		IEnergySource energySource;

		// Use this for initialization
		void Start () {
			energySource = source.GetInterface<IEnergySource> ();
		}
		
		// Update is called once per frame
		void Update () {
			transform.localScale = new Vector3 (parentBackground.localScale.x * amount,
			                                    parentBackground.localScale.y,
			                                    parentBackground.localScale.z);
			transform.position = parentBackground.position 
									- parentBackground.forward * 0.0001f
									- parentBackground.right * (1f - amount) * parentBackground.localScale.x;

			amount = energySource.Energy;//Mathf.Repeat (Time.time * 0.1f, 1f);
		}
	}
}