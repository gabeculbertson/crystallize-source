using UnityEngine;
using System.Collections;

public class OpenDialogueExchange {

	public int ID { get; set; }
	public string Name { get; set; }
	public int PromptClassID { get; set; }
	public int ResponseClassID { get; set; }

	public OpenDialogueExchange(){
		ID = -1;
		Name = "NULL";
		PromptClassID = -1;
		ResponseClassID = -1;
	}

}
