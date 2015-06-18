using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDialoguePhraseClass {

	public int ID { get; set; }
	public string Description { get; set; }
	public List<int> PhraseTemplateIDs { get; set; }

	public bool IsEmpty {
		get {
			return PhraseTemplateIDs.Count == 0;
		}
	}

	public OpenDialoguePhraseClass(){
		ID = -1;
		Description = "";
		PhraseTemplateIDs = new List<int> ();
	}

	public OpenDialoguePhraseClass(int id, string description){
		ID = id;
		Description = description;
		PhraseTemplateIDs = new List<int> ();
	}

	public void AddNewPhraseTemplate(){
		PhraseTemplateIDs.Add (GameData.Instance.PhraseClassData.AddPhraseTemplate().ID);
	}

	public PhraseTemplate[] GetTemplates(){
		var templates = new PhraseTemplate[PhraseTemplateIDs.Count];
		for(int i = 0; i < templates.Length; i++){
			templates[i] = GameData.Instance.PhraseClassData.PhraseTemplates.GetItem(PhraseTemplateIDs[i]);
		}
		return templates;
	}

}
