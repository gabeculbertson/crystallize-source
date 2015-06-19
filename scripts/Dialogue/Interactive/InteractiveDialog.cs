using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractiveDialog : ScriptableObject {

	public List<InteractiveDialogTurn> dialogTurns = new List<InteractiveDialogTurn>();

	public InteractiveDialogTurn GetTurn(string prompt){
		return (from dt in dialogTurns where dt.promptPhrase.Text == prompt select dt).FirstOrDefault ();
	}

}
