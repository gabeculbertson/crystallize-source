using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//[XmlInclude(typeof(NPCActorLine))]
//[XmlInclude(typeof(PlayerActorLine))]
public class InferenceDialogueLine : DialogueActorLine {

	private List<string> category;
	public  List<string> Category { 
		get{
			return category;
		}

	}

    public InferenceDialogueLine(List<string> category) : base(){
		this.category = category;
    }

}
