using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class JobTaskGameData {

    public string Name { get; set; }
    public string AreaName { get; set; }
    public SceneObjectGameData SceneObjectIdentifier { get; set; }
    public DialogueSequence Dialogue { get; set; }
    public DialogueActorLine Line { get; set; }

    public JobTaskGameData() {
        Name = "";
        AreaName = "";
        SceneObjectIdentifier = new SceneObjectGameData();
        Dialogue = new DialogueSequence();
        Line = new DialogueActorLine();
    }

}
