using UnityEngine;
using System.Collections;

public class FloatingNameEventArgs : System.EventArgs {

	public FloatingNameHolder Holder { get; set; }

	public FloatingNameEventArgs(FloatingNameHolder holder){
		this.Holder = holder;
	}

}
