using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

//[XmlInclude(typeof(InferenceTaskGameData))]
public class JobTaskGameData {

    public string Name { get; set; }
    public string AreaName { get; set; }
    public ProcessTypeRef ProcessType { get; set; }
    public SceneObjectGameData Actor { get; set; }
    public DialogueSequence Dialogue { get; set; }
    public DialogueActorLine Line { get; set; }

    public JobTaskGameData() {
        Name = "";
        AreaName = "";
        Actor = new SceneObjectGameData();
        Dialogue = new DialogueSequence();
        Line = new DialogueActorLine();
        ProcessType = new ProcessTypeRef();
    }

}
