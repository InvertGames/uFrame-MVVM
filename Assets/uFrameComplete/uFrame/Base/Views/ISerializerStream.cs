using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializerStream
{

    void SerializeArray<T>(string name,IEnumerable<T> items);
    void SerializeObjectArray(string name, IEnumerable<object> items);
    //void SerializeObject<T>(string name, T value);
    void SerializeObject(string name, object value);
    void SerializeInt(string name, int value);
    void SerializeBool(string name, bool value);
    void SerializeString(string name, string value);
    void SerializeVector2(string name, Vector2 value);
    void SerializeVector3(string name, Vector3 value);
    void SerializeQuaternion(string name, Quaternion value);
    void SerializeDouble(string name, double value);
    void SerializeFloat(string name, float value);
    //void SerializeEnum(string name, Enum value);
    void SerializeBytes(string name, byte[] bytes);
    
    IEnumerable<T> DeserializeObjectArray<T>(string name);
    T DeserializeObject<T>(string name);
    object DeserializeObject(string name);
    int DeserializeInt(string name);
    bool DeserializeBool(string name);
    string DeserializeString(string name);
    Vector2 DeserializeVector2(string name);
    Vector3 DeserializeVector3(string name);
    Quaternion DeserializeQuaternion(string name);
    double DeserializeDouble(string name);
    float DeserializeFloat(string name);
    //Enum DeserializeEnum(string name);
    byte[] DeserializeBytes(string name);
    void Load(byte[] readAllBytes);
    byte[] Save();
    Dictionary<string, IUFSerializable> ReferenceObjects { get; set; }
    ITypeResolver TypeResolver { get; set; }
}

public interface ITypeResolver
{
    Type GetType(string name);
    string SetType(Type type);
    object CreateInstance(string name, string identifier);
}

public class DefaultTypeResolver : ITypeResolver
{
    public Type GetType(string name)
    {
        return Type.GetType(name);
    }

    public string SetType(Type type)
    {
        return type.AssemblyQualifiedName;
    }

    public virtual object CreateInstance(string name,string identifier)
    {

        return Activator.CreateInstance(GetType(name));
    }
}