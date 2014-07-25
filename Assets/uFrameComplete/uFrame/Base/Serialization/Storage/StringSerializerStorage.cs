using System.Text;

public class StringSerializerStorage : ISerializerStorage
{
    public string Result { get; set; }

    public void Load(ISerializerStream stream)
    {
        stream.Load(Encoding.UTF8.GetBytes(Result));
    }

    public void Save(ISerializerStream stream)
    {
        Result = Encoding.UTF8.GetString(stream.Save());
    }

    public override string ToString()
    {
        return Result;
    }
}