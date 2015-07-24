using System;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using Invert.StateMachine;
using uFrame.IOC;
using uFrame.Kernel;
using uFrame.Serialization;

namespace uFrame.MVVM
{
    public static class ViewModelExtensions
    {
        public static string SerializeToJSON(this ViewModel model)
        {
            ISerializerStream stream = new JsonStream();
            ISerializerStorage storage = new StringSerializerStorage();
            stream.DeepSerialize = true;
            model.Write(stream);
            storage.Save(stream);
            return storage.ToString();
        }

        public static void DeserializeFromJSON(this ViewModel model, string json)
        {
            ISerializerStream stream = new JsonStream();
            stream.DeepSerialize = true;
            stream.DependencyContainer = uFrameKernel.Container;
            stream.TypeResolver = new ViewModelResolver() {Container = uFrameKernel.Container};
            stream.Load(Encoding.UTF8.GetBytes(json));
            model.Read(stream);
        }

        public static T Clone<T>(this T viewModel, bool includeCollections = true) where T : ViewModel
        {
            T clone = MVVMKernelExtensions.CreateViewModel<T>();

            foreach (var prop in viewModel.Properties)
            {
                var sourceP = prop.Property;
                var cloneP = clone[sourceP.PropertyName].Property;

                if (!prop.IsCollectionProperty && !prop.IsComputed && !(prop.Property is StateMachine)) //regular property
                {
                    cloneP.ObjectValue = sourceP.ObjectValue;
                }
                else if (prop.IsComputed || prop.Property is StateMachine) //computed stuff
                {
                    //Those will be assigned automatically
                }
                else if (prop.IsCollectionProperty && includeCollections) //collections
                {
                    var sourceList = sourceP as IList;
                    var cloneList = cloneP as IList;

                    for (int i = 0; i < sourceList.Count; i++)
                    {
                        cloneList.Add(sourceList[i]);
                    }
                }
            }

      


            return clone;
        }

    }

    public class ViewModelResolver : ITypeResolver
    {
        public IUFrameContainer Container { get; set; }

        Type ITypeResolver.GetType(string name)
        {
            return Type.GetType(name);
        }
        
        string ITypeResolver.SetType(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        object ITypeResolver.CreateInstance(string name, string identifier)
        {
            var type = ((ITypeResolver) this).GetType(name);

#if NETFX_CORE 
        var isViewModel = type.GetTypeInfo().IsSubclassOf(typeof(ViewModel));
#else
            var isViewModel = typeof (ViewModel).IsAssignableFrom(type);
#endif

            if (isViewModel)
            {
                var contextViewModel = Container.Resolve(type, identifier);
                if (contextViewModel != null)
                {
                    return contextViewModel;
                }
                return MVVMKernelExtensions.CreateViewModel(type, identifier);
            }

            return null;
        }
    }
}