using System.Text;

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
        stream.DependencyContainer = uFrameMVVMKernel.Container;
        stream.TypeResolver = uFrameMVVMKernel.Instance;
        stream.Load(Encoding.UTF8.GetBytes(json));
        model.Read(stream);
    }
}