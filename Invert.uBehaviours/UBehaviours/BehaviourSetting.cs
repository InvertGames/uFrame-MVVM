using System;
using System.Reflection;
using UnityEngine;
/// <summary>
/// A behaviour setting that is applied to 
/// </summary>
[Serializable]
public class BehaviourSetting : IUBFieldInfo
{
    [SerializeField]
    private string _name;


    public FieldInfo FieldInfo
    {
        get
        {
            return GetType().GetField("_value");
        }
        set
        {
            // no Set
        }
    }
    [SerializeField]
    private string _fieldTypeString;

    public Type FieldType
    {
        get
        {
            return _fieldType ?? (_fieldType = UBHelper.GetType(_fieldTypeString));
        }
        set
        {
            _fieldType = value;
            if (value != null)
                _fieldTypeString = value.AssemblyQualifiedName;
        }
    }

    public object[] GetCustomAttributes(Type type, bool inherit)
    {
        if (FieldInfo == null)
        {
            return new object[] { };
        }
        return FieldInfo.GetCustomAttributes(type, inherit);
    }

    public object[] GetCustomAttributes(bool inherit)
    {
        if (FieldInfo == null)
        {
            return new object[] { };
        }
        return FieldInfo.GetCustomAttributes(inherit);
    }

    public object GetValue(object ubAction)
    {
        return Value;
    }

    public void SetValue(object ubAction, object value)
    {
        Value = value;
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public object Value
    {
        get
        {
            if (FieldType == typeof(bool))
                return _bool;
            if (FieldType == typeof(float))
                return _float;
            if (FieldType == typeof(string))
                return _string;
            if (FieldType == typeof(Vector2))
                return _vector2;
            if (FieldType == typeof(Vector3))
                return _vector3;
            if (FieldType == typeof(int))
                return _int;
            if (typeof(Enum).IsAssignableFrom(FieldType))
                return _int;

            return null;
        }
        set
        {
            if (FieldType == typeof(float))
                _float = (float)value;
            else if (FieldType == typeof(bool))
                _bool = (bool)value;
            else if (FieldType == typeof(string))
                _string = (string)value;
            else if (FieldType == typeof(Vector2))
                _vector2 = (Vector2)value;
            else if (FieldType == typeof(Vector3))
                _vector3 = (Vector3)value;
            else if (FieldType == typeof(int))
                _int = (int)value;
            else if (typeof(Enum).IsAssignableFrom(FieldType))
                _int = (int)value;
        }
    }
    [SerializeField]
    private bool _bool = false;
    [SerializeField]
    private string _string = string.Empty;
    [SerializeField]
    private float _float;

    [SerializeField]
    private Vector2 _vector2;
    [SerializeField]
    private Vector3 _vector3;
    [SerializeField]
    private int _int;

    private Type _fieldType;
}