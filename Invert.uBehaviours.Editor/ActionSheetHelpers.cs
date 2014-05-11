using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;

public static class ActionSheetHelpers
{
    //[MenuItem("Tools/RefreshStyles", false, -5)]
    //public static void RS()
    //{
    //    UnityEngine.Object.DestroyImmediate(_skin);
    //    _skin = null;
    //}

    public static void GetActionsMenu()
    {
    }
    static public string GetPath(this Assembly assembly)
    {
   
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        
    }
    public static IEnumerable<Type> GetDerivedTypes<T>(bool includeAbstract = false, bool includeBase = true)
    {
        var type = typeof(T);
        if (includeBase)
            yield return type;
        if (includeAbstract)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(type)))
                {
                    yield return t;
                }
            }
        }
        else
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(type) && !x.IsAbstract))
                {
                    yield return t;
                }
            }
        }
    }

    public static IEnumerable<Type> GetEnumTypes()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var t in assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Enum))))
            {
                yield return t;
            }
        }
    }
}