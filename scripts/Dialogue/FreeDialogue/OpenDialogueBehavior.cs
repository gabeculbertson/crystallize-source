using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDialogueBehavior {

	public string Description { get; set; }
	public List<PhraseClassExpression> PhraseTemplateIDs { get; set; }

	public OpenDialogueBehavior(){
		Description = "NULL";
		PhraseTemplateIDs = new List<PhraseClassExpression> ();
	}

}
