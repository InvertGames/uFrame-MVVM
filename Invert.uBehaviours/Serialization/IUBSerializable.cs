/// <summary>
/// An interface for the custom Unity Binary Serialization
/// </summary>
public interface IUBSerializable
{
    void Deserialize(IReferenceHolder referenceHolder, UBBinarySerializer serializer);

    void Serialize(IReferenceHolder referenceHolder, UBBinarySerializer serializer);
}