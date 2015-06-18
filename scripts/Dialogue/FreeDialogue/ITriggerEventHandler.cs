using UnityEngine;
using System;
using System.Collections;

public interface ITriggerEventHandler {

	void HandleTriggerEntered(object sender, TriggerEventArgs args);
	void HandleTriggerExited(object sender, TriggerEventArgs args);

}
