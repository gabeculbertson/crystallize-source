using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unity.Sequence {
	public sealed class Wait {
	    
		public static Sequence GetSequence(float duration){
			return new Sequence (WaitSequence(duration));
		}

		static IEnumerator WaitSequence(float duration){
			yield return new Wait (duration);
		}

		public float Seconds { get; private set; }
	    
	    public Wait(float seconds) {
	        Seconds = seconds;
	    }
	}
}
