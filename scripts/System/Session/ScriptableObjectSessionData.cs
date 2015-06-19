using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ScriptableObjectSessionData : ScriptableObject, ISessionData {

    public void LoadTo(IDaySession session)
    {
        var targetProperties = new Dictionary<string, PropertyInfo>();
        foreach (var p in session.GetType().GetProperties())
        {
            targetProperties[p.Name] = p;
        }

        var ind = new object[0];
        foreach (var p in GetType().GetProperties())
        {
            if (targetProperties.ContainsKey(p.Name))
            {
                targetProperties[p.Name].SetValue(session, p.GetValue(this, ind), ind);
            }
        }
    }

}
