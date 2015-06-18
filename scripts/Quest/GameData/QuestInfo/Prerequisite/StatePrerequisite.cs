using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[XmlInclude(typeof(QuestStatePrerequisite))]
public class StatePrerequisite {

    public virtual bool IsFulfilled() {
        return true;
    }

}
