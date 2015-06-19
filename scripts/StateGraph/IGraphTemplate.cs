using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public interface IGraphTemplate {

    /// <summary>
    /// Specifies a property value. 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    void SetTemplateProperty(IGraphNode node, PropertyInfo property, object value);

    /// <summary>
    /// Removes the specified property from the list of specified properties
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    void RemoveTemplateProperty(IGraphNode node, PropertyInfo property);

    /// <summary>
    /// Returns true if this property belongs to the template
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    void GetIsTemplateProperty(IGraphNode node, PropertyInfo property);

    /// <summary>
    /// Sets the property as belonging to the instance.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    void SetAsInstanceProperty(IGraphNode node, PropertyInfo property);

    /// <summary>
    /// Return true if the property belongs to the instance
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    bool GetIsInstanceProperty(IGraphNode node, PropertyInfo property);

    /// <summary>
    /// Make the property no longer belong to the instance
    /// </summary>
    /// <param name="node"></param>
    /// <param name="property"></param>
    void RemoveInstanceProperty(IGraphNode node, PropertyInfo property);

    /// <summary>
    /// Get all properties that belong to the instance for a given node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    List<PropertyInfo> GetInstanceProperties(IGraphNode node);

    /// <summary>
    /// Get all nodes in the graph
    /// </summary>
    /// <returns></returns>
    List<IGraphNode> GetNodes();

}
