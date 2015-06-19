using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseClassGameData {

	public int CurrentPhraseTemplateID { get; set; }

	public List<OpenDialoguePhraseClass> PhraseClasses { get; set; }
    public List<PhraseSequence> GrammarTemplates { get; set; }
	public SerializableDictionary<int, PhraseTemplate> PhraseTemplates { get; set; }
	public List<string> PhraseFunctions { get; set; }
	public List<string> PhraseParameters { get; set; }
	public List<string> Tags { get; set; }

	public PhraseClassGameData(){
		PhraseClasses = new List<OpenDialoguePhraseClass> ();
		PhraseTemplates = new SerializableDictionary<int, PhraseTemplate> ();
		PhraseFunctions = new List<string> ();
		PhraseParameters = new List<string> ();
		Tags = new List<string> ();

		CurrentPhraseTemplateID = 1000000;
	}

	public void AddPhraseClass(){
		PhraseClasses.Add (new OpenDialoguePhraseClass (PhraseClasses.Count, "New phrase class"));
	}

	public OpenDialoguePhraseClass GetPhraseClassFromID(int id){
		return(from pc in PhraseClasses where pc.ID == id select pc).FirstOrDefault ();
	}

	public PhraseTemplate AddPhraseTemplate(){
		var pt = new PhraseTemplate (CurrentPhraseTemplateID);
		PhraseTemplates.AddItem(pt);
		CurrentPhraseTemplateID += 10;
		return pt;
	}

    public bool IsValidGrammar(PhraseSequence phrase) {
        foreach (var template in GrammarTemplates) {
            if (phrase.FulfillsTemplate(template)) {
                return true;
            }
        }
        return false;
    }

}
