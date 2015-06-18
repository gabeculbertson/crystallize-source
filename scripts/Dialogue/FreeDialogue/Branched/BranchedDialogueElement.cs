using UnityEngine;
using System.Collections;

public class BranchedDialogueElement {

	public string Description { get; set; }
	public int PromptPhraseClassID { get; set; }
    public PlayerActorLine PromptPhrase { get; set; }
    public ResponseBehaviorGameData Responese { get; set; }
    public NPCActorLine ResponseLine { get; set; }

	public BranchedDialogueElement(){
		Description = "";
		PromptPhraseClassID = -1;
        Responese = new ResponseBehaviorGameData();
        PromptPhrase = new PlayerActorLine();
        ResponseLine = new NPCActorLine();
	}

    

}
