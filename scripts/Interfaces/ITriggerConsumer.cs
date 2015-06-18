using UnityEngine;
using System.Collections;

public interface ITriggerConsumer {

	void TriggerEntered(GameObject trigger);
	void TriggerExited(GameObject trigger);

}
