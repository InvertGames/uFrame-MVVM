using System;
using System.Text;
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