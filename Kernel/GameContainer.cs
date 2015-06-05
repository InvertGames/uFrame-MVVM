using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#if DLL
namespace Invert.IOC
#else
namespace uFrame.IOC
#endif
{
    /// <summary>
    /// A ViewModel Container and a factory for Controllers and commands.
    /// </summary>

    public class UFrameContainer : IUFrameContainer

    {
        private TypeInstanceCollection _instances;
        private TypeMappingCollection _mappings;


        public TypeMappingCollection Mappings
        {
            get { return _mappings ?? (_mappings = new TypeMappingCollection()); }
            set { _mappings = value; }
        }

        public TypeInstanceCollection Instances
        {
            get { return _instances ?? (_instances = new TypeInstanceCollection()); }
            set { _instances = value; }
        }

        public TypeRelationCollection RelationshipMappings
        {
            get { return _relationshipMappings; }
            set { _relationshipMappings = value; }
        }

        public IEnumerable<TType> ResolveAll<TType>()
        {
            foreach (var obj in ResolveAll(typeof(TType)))
            {
                yield return (TType)obj;
            }
        }

        /// <summary>
        /// Resolves all instances of TType or subclasses of TType.  Either named or not.
        /// </summary>
        /// <typeparam name="TType">The Type to resolve</typeparam>
        /// <returns>List of objects.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            foreach (var instance1 in Instances.Where(p => p.Base == type && !string.IsNullOrEmpty(p.Name)))
            {
                yield return instance1.Instance;
            }

            foreach (var mapping in Mappings)
            {
                if (!string.IsNullOrEmpty(mapping.Name))
                {
#if NETFX_CORE 
                    var condition = type.GetTypeInfo().IsSubclassOf(mapping.From);
#else
                    var condition = type.IsAssignableFrom(mapping.From);
#endif
                    if (condition)
                    {
                        var item = Activator.CreateInstance(mapping.To);
                        Inject(item);
                        yield return item;
                    }
                }
            }
        }
        /// <summary>
        /// Clears all type-mappings and instances.
        /// </summary>
        public void Clear()
        {
            Instances.Clear();
            Mappings.Clear();
            RelationshipMappings.Clear();
        }

        /// <summary>
        /// Injects registered types/mappings into an object
        /// </summary>
        /// <param name="obj"></param>
        public void Inject(object obj)
        {
            if (obj == null) return;
#if !NETFX_CORE
            var members = obj.GetType().GetMembers();
#else
            var members = obj.GetType().GetTypeInfo().DeclaredMembers;
#endif
            foreach (var memberInfo in members)
            {
                var injectAttribute =
                    memberInfo.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() as InjectAttribute;
                if (injectAttribute != null)
                {
                    if (memberInfo is PropertyInfo)
                    {
                        var propertyInfo = memberInfo as PropertyInfo;
                        propertyInfo.SetValue(obj, Resolve(propertyInfo.PropertyType, injectAttribute.Name), null);
                    }
                    else if (memberInfo is FieldInfo)
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        fieldInfo.SetValue(obj, Resolve(fieldInfo.FieldType, injectAttribute.Name));
                    }
                }
            }
        }

        /// <summary>
        /// Register a type mapping
        /// </summary>
        /// <typeparam name="TSource">The base type.</typeparam>
        /// <typeparam name="TTarget">The concrete type</typeparam>
        public void Register<TSource, TTarget>(string name = null)
        {
            Mappings[typeof(TSource), name] = typeof(TTarget);
        }

        public void Register(Type source, Type target, string name = null)
        {
            Mappings[source, name] = target;
        }

        /// <summary>
        /// Register a named instance
        /// </summary>
        /// <param name="baseType">The type to register the instance for.</param>        
        /// <param name="instance">The instance that will be resolved be the name</param>
        /// <param name="injectNow">Perform the injection immediately</param>
        public void RegisterInstance(Type baseType, object instance = null, bool injectNow = true)
        {
            RegisterInstance(baseType, instance, null, injectNow);
        }

        /// <summary>
        /// Register a named instance
        /// </summary>
        /// <param name="baseType">The type to register the instance for.</param>
        /// <param name="name">The name for the instance to be resolved.</param>
        /// <param name="instance">The instance that will be resolved be the name</param>
        /// <param name="injectNow">Perform the injection immediately</param>
        public virtual void RegisterInstance(Type baseType, object instance = null, string name = null, bool injectNow = true)
        {
            Instances[baseType, name] = instance;
            if (injectNow)
            {
                Inject(instance);
            }
        }

        public void RegisterInstance<TBase>(TBase instance) where TBase : class
        {
            RegisterInstance<TBase>(instance, true);
        }

        public void RegisterInstance<TBase>(TBase instance, bool injectNow) where TBase : class
        {
            RegisterInstance<TBase>(instance, null, injectNow);
        }

        public void RegisterInstance<TBase>(TBase instance, string name, bool injectNow = true) where TBase : class
        {
            RegisterInstance(typeof(TBase), instance, name, injectNow);
        }

        /// <summary>
        ///  If an instance of T exist then it will return that instance otherwise it will create a new one based off mappings.
        /// </summary>
        /// <typeparam name="T">The type of instance to resolve</typeparam>
        /// <returns>The/An instance of 'instanceType'</returns>
        public T Resolve<T>(string name = null, bool requireInstance = false, params object[] args) where T : class
        {
            return (T)Resolve(typeof(T), name, requireInstance, args);
        }

        /// <summary>
        /// If an instance of instanceType exist then it will return that instance otherwise it will create a new one based off mappings.
        /// </summary>
        /// <param name="baseType">The type of instance to resolve</param>
        /// <param name="name">The type of instance to resolve</param>
        /// <param name="requireInstance">If true will return null if an instance isn't registered.</param>
        /// <param name="constructorArgs">The arguments to pass to the constructor if any.</param>
        /// <returns>The/An instance of 'instanceType'</returns>
        public object Resolve(Type baseType, string name = null, bool requireInstance = false, params object[] constructorArgs)
        {
            // Look for an instance first
            var item = Instances[baseType, name];
            if (item != null)
            {
                return item;
            }
            if (requireInstance)
                return null;
            // Check if there is a mapping of the type
            var namedMapping = Mappings[baseType, name];
            if (namedMapping != null)
            {
                var obj = CreateInstance(namedMapping, constructorArgs);
                //Inject(obj);
                return obj;
            }
            return null;
        }

        public object CreateInstance(Type type, params object[] constructorArgs)
        {
            if (constructorArgs != null && constructorArgs.Length > 0)
            {
                //return Activator.CreateInstance(type,BindingFlags.Public | BindingFlags.Instance,Type.DefaultBinder, constructorArgs,CultureInfo.CurrentCulture);
                var obj2 = Activator.CreateInstance(type, constructorArgs);
                Inject(obj2);
                return obj2;
            }
#if !NETFX_CORE
            ConstructorInfo[] constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
#else
        ConstructorInfo[] constructor = type.GetTypeInfo().DeclaredConstructors.ToArray();
#endif

            if (constructor.Length < 1)
            {
                var obj2 = Activator.CreateInstance(type);
                Inject(obj2);
                return obj2;
            }

            var maxParameters = constructor.First().GetParameters();

            foreach (var c in constructor)
            {
                var parameters = c.GetParameters();
                if (parameters.Length > maxParameters.Length)
                {
                    maxParameters = parameters;
                }

            }
            var args = maxParameters.Select(p =>
            {
                if (p.ParameterType.IsArray)
                {
                    return ResolveAll(p.ParameterType);
                }
                return Resolve(p.ParameterType) ?? Resolve(p.ParameterType, p.Name);
            }).ToArray();

            var obj = Activator.CreateInstance(type, args);
            Inject(obj);
            return obj;
        }

        public TBase ResolveRelation<TBase>(Type tfor, params object[] args)
        {
            try
            {
                return (TBase)ResolveRelation(tfor, typeof(TBase), args);
            }
            catch (InvalidCastException castIssue)
            {
                throw new Exception(string.Format("Resolve Relation couldn't cast  to {0} from {1}", typeof(TBase).Name, tfor.Name), castIssue);
            }
        }
        public void InjectAll()
        {
            foreach (var instance in Instances)
            {
                Inject(instance.Instance);
            }
            foreach (var namedInstance in Instances)
            {
                Inject(namedInstance.Instance);
            }
        }
        private TypeRelationCollection _relationshipMappings = new TypeRelationCollection();
        public void RegisterRelation<TFor, TBase, TConcrete>()
        {
            RelationshipMappings[typeof(TFor), typeof(TBase)] = typeof(TConcrete);
        }

        public void RegisterRelation(Type tfor, Type tbase, Type tconcrete)
        {
            RelationshipMappings[tfor, tbase] = tconcrete;
        }
        public object ResolveRelation(Type tfor, Type tbase, params object[] args)
        {
            var concreteType = RelationshipMappings[tfor, tbase];

            if (concreteType == null)
            {
                return null;
            }
            var result = CreateInstance(concreteType, args);
            //Inject(result);
            return result;
        }
        public TBase ResolveRelation<TFor, TBase>(params object[] arg)
        {
            return (TBase)ResolveRelation(typeof(TFor), typeof(TBase), arg);
        }
    }

    public class TypeMappingCollection : List<TypeMapping>
    {
        public Type this[Type from, string name = null]
        {
            get
            {
                var mapping = this.FirstOrDefault(p => p.From == from && p.Name == name);
                if (mapping != null)
                {
                    return mapping.To;
                }
                return null;
            }
            set
            {
                var mapping = this.FirstOrDefault(p => p.From == from && p.Name == name);
                if (mapping == null)
                {
                    Add(new TypeMapping() { From = from, Name = name, To = value });
                }
                else
                {
                    mapping.To = value;
                    mapping.Name = name;
                }
            }
        }
    }
    public class TypeInstanceCollection : List<RegisteredInstance>
    {

        public object this[Type from, string name = null]
        {
            get
            {
                var mapping = this.FirstOrDefault(p => p.Base == from && p.Name == name);
                if (mapping != null)
                {
                    return mapping.Instance;
                }
                return null;
            }
            set
            {
                var mapping = this.FirstOrDefault(p => p.Base == from && p.Name == name);
                if (mapping == null)
                {
                    Add(new RegisteredInstance() { Base = from, Name = name, Instance = value });
                }
                else
                {
                    mapping.Instance = value;
                    mapping.Name = name;
                }
            }
        }
    }
    public class TypeRelationCollection : List<TypeRelation>
    {
        
        public Type this[Type from, Type to]
        {
            get
            {
                var mapping = this.FirstOrDefault(p => p.From == from && p.To == to);
                if (mapping != null)
                {
                    return mapping.Concrete;
                }
                return null;
            }
            set
            {
                var mapping = this.FirstOrDefault(p => p.From == from && p.To == to);
                if (mapping == null)
                {
                    Add(new TypeRelation() { From = from, To = to, Concrete = value });
                }
                else
                {
                    mapping.Concrete = value;

                }
            }
        }
    }
    public class TypeRelation
    {
        public Type From
        {
            get;
            set;
        }

        public Type To { get; set; }

        public Type Concrete { get; set; }

        //public string Name { get; set; }
    }
    public class RegisteredInstance
    {
        public Type Base
        {
            get;
            set;
        }

        public object Instance
        {
            get;
            set;
        }

        public string Name { get; set; }
    }
    public class TypeMapping
    {
        public Type From
        {
            get;
            set;
        }

        public Type To
        {
            get;
            set;
        }
        public string Name { get; set; }
    }

}
