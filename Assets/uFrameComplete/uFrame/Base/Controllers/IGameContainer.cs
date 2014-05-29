using System;
#if DLL

namespace Invert.uFrame
{
    public interface IUFrameContainer
#else
    public interface IGameContainer
#endif

   
    {
        /// <summary>
        /// Clears all type mappings and instances.
        /// </summary>
        void Clear();

        /// <summary>
        /// Injects registered types/mappings into an object
        /// </summary>
        /// <param name="obj"></param>
        void Inject(object obj);

        /// <summary>
        /// Register a type mapping
        /// </summary>
        /// <typeparam name="TSource">The base type.</typeparam>
        /// <typeparam name="TTarget">The concrete type</typeparam>
        void Register<TSource, TTarget>();

        /// <summary>
        /// Register an instance of a type.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="default"></param>
        /// <returns></returns>
        TBase RegisterInstance<TBase>(TBase @default = null, bool injectNow = true) where TBase : class;

        /// <summary>
        /// Register an instance of a type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        object RegisterInstance(Type type, object @default = null, bool injectNow = true);

        /// <summary>
        ///  If an instance of T exist then it will return that instance otherwise it will create a new one based off mappings.
        /// </summary>
        /// <typeparam name="T">The type of instance to resolve</typeparam>
        /// <returns>The/An instance of 'instanceType'</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// If an instance of instanceType exist then it will return that instance otherwise it will create a new one based off mappings.
        /// </summary>
        /// <param name="instanceType">The type of instance to resolve</param>
        /// <param name="requireInstance">Will cause an exception if an instance hasn't been registered</param>
        /// <returns>The/An instance of 'instanceType'</returns>
        object Resolve(Type instanceType, bool requireInstance = false);

        /// <summary>
        /// Injects everything that is registered at once
        /// </summary>
        void InjectAll();

        /// <summary>
        /// Register a named instance
        /// </summary>
        /// <param name="name">The name for the instance to be resolved.</param>
        /// <param name="instance">The instance that will be resolved be the name</param>
        /// <param name="injectNow">Perform the injection immediately</param>
        void RegisterInstance(string name, object instance, bool injectNow = true);

        /// <summary>
        /// Resolve by the name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Resolve<T>(string name) where T : class;
    }

#if DLL
}

#endif