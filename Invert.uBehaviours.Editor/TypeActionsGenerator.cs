using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TypeActionsGenerator
{
    public static List<string> _ignores = new List<string>()
    {
        {"GetHashCode"},
        {"ToString"},
        {"SetRectCenter"},
        {"SetRectHeight"},
        {"SetRectWidth"},
        {"SetRectX"},
        {"SetRectY"},
        {"SetRectXMax"},
        {"SetRectXMin"},
        {"SetRectYMax"},
        {"SetRectYMin"},
       
        
    };
    public List<ClassGenerator> Generators { get; set; }

    public List<MethodInfo> Methods { get; set; }

    public List<PropertyInfo> Properties { get; set; }

    public TypeActionsGenerator()
    {
        Properties = new List<PropertyInfo>();
        Methods = new List<MethodInfo>();
        Generators = new List<ClassGenerator>();
    }

    public TypeActionsGenerator(Type type)
    {
        Properties = new List<PropertyInfo>();
        Methods = new List<MethodInfo>();
        Generators = new List<ClassGenerator>();
        AddType(type);
    }

    public IEnumerable<ClassGenerator> AddMethod(MethodInfo methodInfo)
    {
        if (methodInfo.Name.StartsWith("op_")) yield break;
        if (methodInfo.ReturnType == typeof(bool))
        {
            var generator = new ConditionMethodActionBuilder(methodInfo) { FilePath = GetMethodPath(methodInfo) };
            if (!_ignores.Contains(generator.Filename))
            {
                Generators.Add(generator);
                yield return generator;
            }
           
        }
        else
        {
            var generator = new MethodActionClassGenerator(methodInfo) { FilePath = GetMethodPath(methodInfo) };
            if (!_ignores.Contains(generator.Filename))
            {
                Generators.Add(generator);
                yield return generator;
            }
           
        }
    }

    public IEnumerable<ClassGenerator> AddProperty(PropertyInfo propertyInfo)
    {
        
        Properties.Add(propertyInfo);

        if (propertyInfo.CanRead)
        {
            if (propertyInfo.PropertyType == typeof(bool))
            {
                var generator = new ConditionPropertyActionBuilder(propertyInfo)
                {
                    FilePath = GetPropertyPath(propertyInfo)
                };
                if (!_ignores.Contains(generator.Filename))
                {
                    Generators.Add(generator);
                    yield return generator;
                }
               
            }
            else
            {
                var generator = new GetPropertyActionClassGenerator(propertyInfo)
                {
                    FilePath = GetPropertyPath(propertyInfo)
                };
                if (!_ignores.Contains(generator.Filename))
                {
                    Generators.Add(generator);
                    yield return generator;
                }
                
            }
        }
        if (propertyInfo.CanWrite)
        {
            var generator = new SetPropertyActionClassGenerator(propertyInfo) { FilePath = GetPropertyPath(propertyInfo) };
            if (!_ignores.Contains(generator.Filename))
            {
                Generators.Add(generator);
                yield return generator;
            }
            
        }
    }

    public void AddType(Type type, bool clear = true)
    {
        if (clear)
        {
            Properties.Clear();
            Methods.Clear();
            Generators.Clear();    
        }
        

        foreach (var method in GetApplicableMethods(type).ToArray())
        {
            var results = AddMethod(method).ToArray();
            foreach (var classGenerator in results)
            {
                Debug.Log("Generated " + classGenerator.Name);
            }
        }
        foreach (var property in GetApplicableProperties(type).ToArray())
        {
            var items = AddProperty(property).ToArray();
            foreach (var item in items)
            {
                Debug.Log("Generated " + item.Name);
            }
        }
    }

    public void GenerateAll()
    {
        foreach (var classGenerator in Generators)
        {
            classGenerator.ToFile();
        }
    }

    public static IEnumerable<MethodInfo> GetApplicableMethods(Type type)
    {
        var declaredMembers = type.GetMethods(ActionClassGenerator.METHOD_BINDING_FLAGS).ToArray();

        foreach (var dm in declaredMembers)
        {
           
            if (dm.ContainsGenericParameters) continue;
            var parameters = dm.GetParameters();

            if (dm.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0) continue;
            if (dm.Name.StartsWith("get_") || dm.Name.StartsWith("set_")) continue;
            if (dm.ReturnType != typeof(void) && !IsAllowedType(dm.ReturnType)) continue;
            if (!parameters.All(p => IsAllowedType(p.ParameterType))) continue;
            yield return dm;
        }
    }

    public static IEnumerable<PropertyInfo> GetApplicableProperties(Type type)
    {
        var declaredMembers = type.GetProperties(ActionClassGenerator.PROPERTY_BINDING_FLAGS).ToArray();

        foreach (var dm in declaredMembers)
        {
            
            if (dm.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0) continue;
            if (!IsAllowedType(dm.PropertyType)) continue;
            yield return dm;
        }
    }

    public string GetMethodPath(MethodInfo info)
    {
        var path = Path.Combine(Application.dataPath, "UBActions\\" + info.DeclaringType.Name);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public string GetPropertyPath(PropertyInfo info)
    {
        var path = Path.Combine(Application.dataPath, "UBActions\\" + info.DeclaringType.Name);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static bool IsAllowedType(Type type)
    {
        return UBGeneratorHelper.AllowedParameterTypes.Any(x => x.IsAssignableFrom(type)) || typeof(Enum).IsAssignableFrom(type);
    }
}