using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphPlayerData {

    public bool GetNodeTranversed(IGraphNode node) {
        // TODO: implement this
        return false;
    }

    public void SetNodeTranversed(IGraphNode node, bool traversed) {
        // TODO: implement
    }

    
    public List<IGraphNode> TraverseNodes(IGraphInstance graph, object value) {
        var traversedNodes = new List<IGraphNode>();
        foreach (var node in graph.GetNodes()) {
            // have we already traversed this node?
            if(GetNodeTranversed(node))
                continue;
            // have all the dependencies been fulfilled?
            if (!node.DependenciesFulfilled(this))
                continue;
            // is the event/data object of the same type as the requirement for this node?
            if (!value.GetType().IsAssignableFrom(node.Value.GetType()))
                continue;
            // does this node have the correct properties?
            if (!FulfillsPropertyRequirements(graph, node, value)) 
                continue;

            SetNodeTranversed(node, true);
            traversedNodes.Add(node);
        }
        return traversedNodes;
    }

    bool FulfillsPropertyRequirements(IGraphInstance graph, IGraphNode node, object value) {
        foreach (var property in graph.GetNodeProperties(node)) {
            if (property.GetValue(value, new object[0]) != graph.GetPropertyValue(node, property)) {
                return false;
            }
        }
        return true;
    }

}
