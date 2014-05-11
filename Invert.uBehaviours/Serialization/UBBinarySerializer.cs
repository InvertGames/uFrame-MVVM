using System;
using System.Collections.Generic;
using UnityEngine;

public class UBBinarySerializer
{

    private readonly List<byte> _byteStream = new List<byte>();
    private byte[] _byteArray;
    private int _index = 0;

    public int ByteArraySize
    {
        get
        { 
            return _byteStream.Count;
        }
    }

    public void MoveToIndex(int i)
    {
        _index = i;
    }
    public byte[] ByteArray
    {
        get
        {
            if (_byteArray == null || _byteStream.Count != _byteArray.Length)
                _byteArray = _byteStream.ToArray();

            return _byteArray;
        }
    }

    public UBBinarySerializer()
    {

    }

    public UBBinarySerializer(byte[] ByteArray)
    {
        _byteArray = ByteArray;
        _byteStream = new List<byte>(ByteArray);
    }



    //--- double ---  
    public void Serialize(double d)
    {

        _byteStream.AddRange(BitConverter.GetBytes(d));

    }

    public double DeserializeDouble()
    {
        double d = BitConverter.ToDouble(ByteArray, _index); _index += 8;
        return d;
    }
      

    // --- bool ---  
    public void Serialize(bool b)
    {
        _byteStream.AddRange(BitConverter.GetBytes(b));
    }

    public bool DeserializeBool()
    {
        bool b = BitConverter.ToBoolean(ByteArray, _index); _index += 1;
        return b;
    }
      

    // --- Vector2 ---  
    public void Serialize(Vector2 v)
    {
        _byteStream.AddRange(GetBytes(v));
    }

    public Vector2 DeserializeVector2()
    {
        Vector2 vector2 = new Vector2();
        vector2.x = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        vector2.y = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        return vector2;
    }
      

    // --- Vector3 ---  
    public void Serialize(Vector3 v)
    {
        _byteStream.AddRange(GetBytes(v));
    }

    public Vector3 DeserializeVector3()
    {
        Vector3 vector3 = new Vector3();
        vector3.x = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        vector3.y = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        vector3.z = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        return vector3;
    }

    // --- Color ---  
    public void Serialize(Color v)
    {
        _byteStream.AddRange(GetBytes(v));
    }

    public Color DeserializeColor()
    {
        Color vector3 = new Color();
        vector3.r = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        vector3.g = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        vector3.b = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        vector3.a = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        return vector3;
    }
      
    // --- Type ---  
    public void Serialize(System.Type t)
    {
        // serialize type as string  
        string typeStr = t.AssemblyQualifiedName;
        Serialize(typeStr);
    }

    public Type DeserializeType()
    {
        // type stored as string  
        string typeStr = DeserializeString();
        return UBHelper.GetType(typeStr);
    }
      

    //--- String ---  
    public void Serialize(string str)
    {
        var s = str ?? string.Empty;
        //add the length as a header  
        _byteStream.AddRange(BitConverter.GetBytes(s.Length));
        foreach (char c in s)
            _byteStream.Add((byte)c);
    }

    public string DeserializeString()
    {
        int length = BitConverter.ToInt32(ByteArray, _index); _index += 4;
        string s = "";
        for (int i = 0; i < length; i++)
        {
            s += (char)ByteArray[_index]; 
            _index++;
        }

        return s;
    }
      

    // --- byte[] ---  
    public void Serialize(byte[] b)
    {
        // add the length as a header  
        _byteStream.AddRange(BitConverter.GetBytes(b.Length));
        _byteStream.AddRange(b);
    }

    public byte[] DeserializeByteArray()
    {
        int length = BitConverter.ToInt32(ByteArray, _index); _index += 4;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = ByteArray[_index];
            _index++;
        }

        return bytes;
    }
      

    // --- Quaternion ---  
    public void Serialize(Quaternion q)
    {
        _byteStream.AddRange(GetBytes(q));
    }

    public Quaternion DeserializeQuaternion()
    {
        Quaternion quat = new Quaternion();
        quat.x = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        quat.y = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        quat.z = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        quat.w = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        return quat;
    }
          // --- Rect ---  
    public void Serialize(Rect q)
    {
        _byteStream.AddRange(GetBytes(q));
    }

    public Rect DeserializeRect()
    {
        Rect rect = new Rect();
        rect.x = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        rect.y = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        rect.width = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        rect.height = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        return rect;
    }
      

    // --- float ---  
    public void Serialize(float f)
    {
        _byteStream.AddRange(BitConverter.GetBytes(f));
    }

    public float DeserializeFloat()
    {
        float f = BitConverter.ToSingle(ByteArray, _index); _index += 4;
        return f;
    }
      

    //--- int ---  
    public void Serialize(int i)
    {
        _byteStream.AddRange(BitConverter.GetBytes(i));
    }

    public int DeserializeInt()
    {
        int i = BitConverter.ToInt32(ByteArray, _index); _index += 4;
        return i;
    }
      

    //--- internal ----  
    Vector3 DeserializeVector3(byte[] bytes, ref int index)
    {
        Vector3 vector3 = new Vector3();
        vector3.x = BitConverter.ToSingle(bytes, index); index += 4;
        vector3.y = BitConverter.ToSingle(bytes, index); index += 4;
        vector3.z = BitConverter.ToSingle(bytes, index); index += 4;

        return vector3;
    }

    Quaternion DeserializeQuaternion(byte[] bytes, ref int index)
    {
        Quaternion quat = new Quaternion();
        quat.x = BitConverter.ToSingle(bytes, index); index += 4;
        quat.y = BitConverter.ToSingle(bytes, index); index += 4;
        quat.z = BitConverter.ToSingle(bytes, index); index += 4;
        quat.w = BitConverter.ToSingle(bytes, index); index += 4;
        return quat;
    }

    byte[] GetBytes(Vector2 v)
    {
        List<byte> bytes = new List<byte>(8);
        bytes.AddRange(BitConverter.GetBytes(v.x));
        bytes.AddRange(BitConverter.GetBytes(v.y));
        return bytes.ToArray();
    }

    byte[] GetBytes(Vector3 v)
    {
        List<byte> bytes = new List<byte>(12);
        bytes.AddRange(BitConverter.GetBytes(v.x));
        bytes.AddRange(BitConverter.GetBytes(v.y));
        bytes.AddRange(BitConverter.GetBytes(v.z));
        return bytes.ToArray();
    }
    byte[] GetBytes(Color v)
    {
        List<byte> bytes = new List<byte>(16);
        bytes.AddRange(BitConverter.GetBytes(v.r));
        bytes.AddRange(BitConverter.GetBytes(v.g));
        bytes.AddRange(BitConverter.GetBytes(v.b));
        bytes.AddRange(BitConverter.GetBytes(v.a));
        return bytes.ToArray();
    }

    byte[] GetBytes(Quaternion q)
    {
        List<byte> bytes = new List<byte>(16);
        bytes.AddRange(BitConverter.GetBytes(q.x));
        bytes.AddRange(BitConverter.GetBytes(q.y));
        bytes.AddRange(BitConverter.GetBytes(q.z));
        bytes.AddRange(BitConverter.GetBytes(q.w));
        return bytes.ToArray();
    }
    byte[] GetBytes(Rect q)
    {
        List<byte> bytes = new List<byte>(16);
        bytes.AddRange(BitConverter.GetBytes(q.x));
        bytes.AddRange(BitConverter.GetBytes(q.y));
        bytes.AddRange(BitConverter.GetBytes(q.width));
        bytes.AddRange(BitConverter.GetBytes(q.height));
        return bytes.ToArray();
    }

    public static void Example()
    {
          
        Debug.Log("--- UnitySerializer Example ---");
        Vector2 point = UnityEngine.Random.insideUnitCircle;
        Vector3 position = UnityEngine.Random.onUnitSphere;
        Quaternion quaternion = UnityEngine.Random.rotation;
        float f = UnityEngine.Random.value;
        int i = UnityEngine.Random.Range(0, 10000);
        double d = (double)UnityEngine.Random.Range(0, 10000);
        string s = "Brundle Fly";
        bool b = UnityEngine.Random.value < 0.5f ? true : false;
        System.Type type = typeof(UBBinarySerializer);

          
        Debug.Log("--- Before ---");
        Debug.Log(point + " " + position + " " + quaternion + " " + f + " " + d + " " + s + " " + b + " " + type);

          
        Debug.Log("--- Serialize ---");
        UBBinarySerializer us = new UBBinarySerializer();
        us.Serialize(point);
        us.Serialize(position);
        us.Serialize(quaternion);
        us.Serialize(f);
        us.Serialize(i);
        us.Serialize(d);
        us.Serialize(s);
        us.Serialize(b);
        us.Serialize(type);
        byte[] byteArray = us.ByteArray;

        // the array must be deserialized in the same order as it was serialized  
        Debug.Log("--- Deserialize ---");
        UBBinarySerializer uds = new UBBinarySerializer(byteArray);
        Vector2 point2 = uds.DeserializeVector2();
        Vector3 position2 = uds.DeserializeVector3();
        Quaternion quaternion2 = uds.DeserializeQuaternion();
        float f2 = uds.DeserializeFloat();
        int i2 = uds.DeserializeInt();
        double d2 = uds.DeserializeDouble();
        string s2 = uds.DeserializeString();
        bool b2 = uds.DeserializeBool();
        System.Type type2 = uds.DeserializeType();

          
        Debug.Log("--- After ---");
        Debug.Log(point2 + " " + position2 + " " + quaternion2 + " " + f2 + " " + d2 + " " + s2 + " " + b2 + " " + type2);
    }
}