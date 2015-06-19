using UnityEngine;
using System.Collections;

public class MoneyEffectProducer : MonoBehaviour {

	public GameObject prefab;
	public GameObject eventSource;

	// Use this for initialization
	void Start () {
		eventSource.GetInterface<IChargeEvent> ().ChargeComplete += HandleChargeComplete;;
	}

	void HandleChargeComplete (object sender, TextEventArgs e)
	{
		if (prefab) {
			var instance = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
			instance.GetComponent<MoneyEffect>().amount = e.Text;
			instance.transform.parent = transform;
		}
	}

}
