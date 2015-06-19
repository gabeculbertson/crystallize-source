using UnityEngine;
using System.Collections;

public class ChallengeEventArgs : System.EventArgs {

	public ChallengeGameData Challenge { get; private set; }

	public ChallengeEventArgs (ChallengeGameData challenge){
		Challenge = challenge;
	}

}
