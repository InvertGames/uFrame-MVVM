using System;
using System.Globalization;

using UnityEngine;

#if DLL
using Invert.uFrame.Editor;
namespace Invert.MVVM
{
#else
using UnityEngine;
#endif
/// <summary>
/// A base class for model properties.
/// </summary>
[Serializable]
public abstract class ModelPropertyBase 
{
    public ViewModel Owner { get; set; }
    public string PropertyName { get; set; }

    protected ModelPropertyBase()
    {
    }

    protected ModelPropertyBase(ViewModel owner, string propertyName)
    {
        Owner = owner;
        PropertyName = propertyName;

    }

    public delegate void PropertyChangedHandler(object value);

    /// <summary>
    /// When the value has changed
    /// </summary>
    public event PropertyChangedHandler ValueChanged;


    protected object _value;

    /// <summary>
    /// The value of this model property
    /// </summary>
    public virtual object ObjectValue
    {
        get
        {
            if (_value == null && ValueType.IsValueType)
            {
                _value = Activator.CreateInstance(ValueType);
            }
            return _value;
        }
        set
        {
            var changed = !object.Equals(_value,value);
            LastValueObject = _value;
            _value = value;
            if (changed)
                OnPropertyChanged(value);
        }
    }
    public object LastValueObject { get; set; }
    /// <summary>
    /// The value type of this property
    /// </summary>
    public virtual Type ValueType
    {
        get
        {
            return typeof(object);
        }
    }

    public static object DeserializeObject(Type valueType, JSONNode node)
    {
        if (typeof(IJsonSerializable).IsAssignableFrom(valueType))
        {
            var objectValue = Activator.CreateInstance(valueType);
            ((IJsonSerializable)objectValue).Deserialize(node);
            return objectValue;
        }
        else if (valueType == typeof(bool))
        {
            return node.AsBool;
        }
        else if (valueType == typeof(string))
        {
            return node.Value.ToString(CultureInfo.InvariantCulture);
        }
#if UNITY_DLL
        else if (valueType == typeof(Vector2))
        {
            return node.AsQuaternion;
        }
        else if (valueType == typeof(Vector3))
        {
            return node.AsVector3;
        }
        else if (valueType == typeof(Rect))
        {
            return node.AsQuaternion;
        }
#endif
        else if (valueType == typeof(int))
        {
            return node.AsInt;
        }
        else if (valueType == typeof(float))
        {
            return node.AsFloat;
        }
        else if (valueType == typeof(double))
        {
            return node.AsDouble;
        }

        return null;
    }

    public static JSONNode SerializeObject(Type valueType, object value)
    {
        var serializable = value as IJsonSerializable;
        if (serializable != null)
        {
            return ((IJsonSerializable)value).Serialize();
        }
        else if (valueType == typeof(Vector2))
        {
            return new JSONClass
            {
                AsVector2 = (Vector2)value
            };
        }
        else if (valueType == typeof(Vector3))
        {
            return new JSONClass
            {
                AsVector3 = (Vector3)value
            };
        }
        else if (valueType == typeof(Rect))
        {
            var j = new JSONClass();
            var v = (Rect)value;

            j["x"].AsFloat = v.x;
            j["y"].AsFloat = v.y;
            j["width"].AsFloat = v.width;
            j["height"].AsFloat = v.height;
            return j;
        }
        else if (valueType == typeof(int))
        {
            return new JSONData((int)value);
        }
        else if (valueType == typeof(float))
        {
            return new JSONData((float)value);
        }
        else if (valueType == typeof(double))
        {
            return new JSONData((Double)value);
        }
        else if (value == null)
        {
            return new JSONData("");
        }

        return new JSONData(value.ToString());
    }

    public abstract void Deserialize(JSONNode node);
    public abstract JSONNode Serialize();

    /// <summary>
    /// Sets the value without invoking any OnPropertyChanged events.
    /// This is useful for two-way bindings
    /// </summary>
    /// <param name="value"></param>
    public void QuietlySetValue(object value)
    {
        _value = value;
    }

    protected virtual void OnPropertyChanged(object value)
    {
        PropertyChangedHandler handler = ValueChanged;
        if (handler != null)
            handler(value);
        if (Owner != null)
        {
            Owner.OnPropertyChanged(PropertyName);
        }
    }
}
#if DLL
}
#endif