using UnityEngine;
using System;
using System.Collections;

public interface IGraphNode {

    string NodeName { get; set; }

    // TODO: make this serializable
    object Value { get; set; }

    // Have all the dependencies been fulfilled for this node?
    bool DependenciesFulfilled(GraphPlayerData graphPlayerData);
	
}
