using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// A ViewModel Container and a factory for Controllers and commands.
/// </summary>
public class GameContainer : IGameContainer
{
    private Dictionary<Type, object> _instances;
    private Dictionary<Type, Type> _mappings;
    private Dictionary<string, object> _namedInstances;

    public Dictionary<Type, Type> Mappings
    {
        get { return _mappings ?? (_mappings = new Dictionary<Type, Type>()); }
        set { _mappings = value; }
    }

    protected Dictionary<Type, object> Instances
    {
        get { return _instances ?? (_instances = new Dictionary<Type, object>()); }
        set { _instances = value; }
    }

    protected Dictionary<string, object> NamedInstances
    {
        get { return _namedInstances ?? (_namedInstances = new Dictionary<string, object>()); }
        set { _namedInstances = value; }
    }
    public GameContainer()
    {

    }

    /// <summary>
    /// Clears all type-mappings and instances.
    /// </summary>
    public void Clear()
    {
        Instances.Clear();
        NamedInstances.Clear();
        Mappings.Clear();
    }

    /// <summary>
    /// Injects registered types/mappings into an object
    /// </summary>
    /// <param name="obj"></param>
    public void Inject(object obj)
    {
        if (obj == null) return;

        var members = obj.GetType().GetMembers();
        foreach (var memberInfo in members)
        {
            var injectAttribute = memberInfo.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() as InjectAttribute;
            if (injectAttribute != null)
            {

                if (memberInfo is PropertyInfo)
                {
                    var propertyInfo = memberInfo as PropertyInfo;
                    if (string.IsNullOrEmpty(injectAttribute.Name))
                    {
                        var injectInstance = Instances.ContainsKey(propertyInfo.PropertyType)
                            ? Instances[propertyInfo.PropertyType]
                            : null;
                        if (injectInstance != null)
                        {
                            propertyInfo.SetValue(obj, injectInstance, null);
                        }
                    }
                    else
                    {
                        if (NamedInstances.ContainsKey(injectAttribute.Name))
                        {
                            propertyInfo.SetValue(obj, NamedInstances[injectAttribute.Name], null);
                        }
                    }

                }
                else if (memberInfo is FieldInfo)
                {
                    var fieldInfo = memberInfo as FieldInfo;
                    if (string.IsNullOrEmpty(injectAttribute.Name))
                    {
                        var injectInstance = Instances.ContainsKey(fieldInfo.FieldType) ? Instances[fieldInfo.FieldType] : null;
                        if (injectInstance != null)
                        {
                            if (fieldInfo.FieldType != obj.GetType())
                                Inject(injectInstance);

                            fieldInfo.SetValue(obj, injectInstance);
                        }
                    }
                    else
                    {
                        if (NamedInstances.ContainsKey(injectAttribute.Name))
                        {
                            fieldInfo.SetValue(obj, NamedInstances[injectAttribute.Name]);
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// Register a type mapping
    /// </summary>
    /// <typeparam name="TSource">The base type.</typeparam>
    /// <typeparam name="TTarget">The concrete type</typeparam>
    public void Register<TSource, TTarget>()
    {
        if (Mappings.ContainsKey(typeof(TSource)))
        {
            Mappings[typeof(TSource)] = typeof(TTarget);
            return;
        }
        Mappings.Add(typeof(TSource), typeof(TTarget));
    }

    /// <summary>
    /// Register an instance of a type.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    /// <param name="instance"></param>
    /// <param name="injectNow"></param>
    /// <returns></returns>
    public TBase RegisterInstance<TBase>(TBase instance = null, bool injectNow = true) where TBase : class
    {
        return RegisterInstance(typeof(TBase), instance, injectNow) as TBase;
    }

    /// <summary>
    /// Register a named instance
    /// </summary>
    /// <param name="name">The name for the instance to be resolved.</param>
    /// <param name="instance">The instance that will be resolved be the name</param>
    /// <param name="injectNow">Perform the injection immediately</param>
    public void RegisterInstance(string name, object instance, bool injectNow = true)
    {
        if (NamedInstances.ContainsKey(name))
        {
            NamedInstances[name] = instance;
        }
        else
        {
            NamedInstances.Add(name, instance);
        }
        if (injectNow)
        {
            Inject(instance);
        }
    }

    /// <summary>
    /// Register an instance of a type.
    /// </summary>
    /// <param name="type">The type of the instance</param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public object RegisterInstance(Type type, object instance = null, bool injectNow = true)
    {
        var model = instance ?? Activator.CreateInstance(type);

        if (injectNow)
            Inject(model);

        // If its a derived type lets register that type too
        var modelType = model.GetType();
        if (type != modelType)
        {
            if (Instances.ContainsKey(modelType))
                Instances[modelType] = model;
            else
                Instances.Add(modelType, model);
        }

        // Register the instance
        if (Instances.ContainsKey(type))
            Instances[type] = model;
        else
            Instances.Add(type, model);

        return model;
    }

    /// <summary>
    ///  If an instance of T exist then it will return that instance otherwise it will create a new one based off mappings.
    /// </summary>
    /// <typeparam name="T">The type of instance to resolve</typeparam>
    /// <returns>The/An instance of 'instanceType'</returns>
    public T Resolve<T>() where T : class
    {
        return (T)Resolve(typeof(T));
    }

    /// <summary>
    /// Resolve by the name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Resolve<T>(string name) where T : class
    {
        if (NamedInstances.ContainsKey(name))
        {
            return NamedInstances[name] as T;
        }
        return null;
    }

    /// <summary>
    /// If an instance of instanceType exist then it will return that instance otherwise it will create a new one based off mappings.
    /// </summary>
    /// <param name="instanceType">The type of instance to resolve</param>
    /// <param name="requireInstance">If true will return null if an instance isn't registered.</param>
    /// <returns>The/An instance of 'instanceType'</returns>
    public object Resolve(Type instanceType, bool requireInstance = false)
    {
        if (!Instances.ContainsKey(instanceType))
        {
            if (requireInstance)
            {
                return null;
            }
            if (Mappings.ContainsKey(instanceType))
            {
                var obj = Activator.CreateInstance(Mappings[instanceType]);
                Inject(obj);
                return obj;
            }
            else
            {
                var obj = Activator.CreateInstance(instanceType);
                Inject(obj);
                return obj;
            }
        }
        return Instances[instanceType];
    }

    public void InjectAll()
    {
        foreach (var instance in Instances)
        {
            Inject(instance.Value);
        }
        foreach (var namedInstance in NamedInstances)
        {
            Inject(namedInstance.Value);
        }
    }

}