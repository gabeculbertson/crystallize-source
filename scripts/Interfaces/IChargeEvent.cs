using UnityEngine;
using System;
using System.Collections;

public interface IChargeEvent {

	event EventHandler<TextEventArgs> ChargeComplete;

}
