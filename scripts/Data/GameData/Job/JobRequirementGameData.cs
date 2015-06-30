using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[XmlInclude(typeof(PhraseJobRequirementGameData))]
public abstract class JobRequirementGameData {

    public abstract bool IsFulfilled();

}