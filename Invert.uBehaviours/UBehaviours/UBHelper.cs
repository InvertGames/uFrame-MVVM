using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

public static class UBHelper
{
    public static Type GetType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            return null;
        var type = typeName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
        // UnityEngine.Debug.Log(type);

        if (type == null)
            return null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var t = assembly.GetType(type);

            if (t != null)
            {
                return t;
            }
        }
        return null;
    }
}