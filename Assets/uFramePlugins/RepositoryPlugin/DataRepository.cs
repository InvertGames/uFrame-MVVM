
using UnityEngine;

public interface IDataStorageProvider
{
    void Save<T>(T type) where T : ViewModel;
    T Load<T>() where T : ViewModel;
    void SaveList<T>(T list);
    void ReadList<T>(T list);
}

public class DataRepository
{
    [Inject]
    public IDataStorageProvider Storage { get; set; }


}

public interface IUFrameSerializer
{
    void SerializeFloat(float value, string name = null);
    void SerializeInt(int value, string name = null);
    void SerializeColor(Color value, string name = null);
    void SerializeVector2(Vector2 value, string name = null);
    void SerializeVector3(Vector3 value, string name = null);
    void SerializeRect(Rect value, string name = null);
    void SerializeBool(bool value, string name);
    void SerializeQuaternion(Quaternion value, string name);
    void SerializeVector4(Vector4 value, string name);
    void SerializeString(string value, string name);

    void DeserializeFloat(string name, float value);
}

