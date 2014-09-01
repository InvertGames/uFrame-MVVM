
#if DLL
namespace Invert.uFrame.Editor
{
#endif
public interface IJsonSerializable
{
    void Deserialize(JSONNode node);

    JSONNode Serialize();
}

#if DLL
}
#endif