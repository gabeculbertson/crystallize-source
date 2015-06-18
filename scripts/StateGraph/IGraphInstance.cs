using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public interface IGraphInstance {

    /// <summary>
    /// A reference to the backing template
    /// </summary>
    IGraphTemplate Template { get; set; }

    /// <summary>
    /// Set the instance property. If the provided property does not belong to the instance, nothing should happen.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    void SetInstanceProperty(IGraphNode node, PropertyInfo property, object value);

    /// <summary>
    /// This should get the property value from the template if the this property belongs to the template
    /// or the instance if it belongs to the instance.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    object GetPropertyValue(IGraphNode node, PropertyInfo property);

    /// <summary>
    /// Get all nodes in the graph. Probably just want to return the templates nodes.
    /// </summary>
    /// <returns></returns>
    List<IGraphNode> GetNodes();

    /// <summary>
    /// Gets all properties that belong to either the instance or the template.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    List<PropertyInfo> GetNodeProperties(IGraphNode node);

}
