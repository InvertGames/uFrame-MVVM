using System;
using System.Reflection;

public class ReflectionFieldInfo : IUBFieldInfo
{
    public FieldInfo FieldInfo { get; set; }

    public virtual Type FieldType
    {
        get
        {
            return FieldInfo.FieldType;
        }
    }

    public virtual string Name
    {
        get { return FieldInfo.Name; }
    }

    public ReflectionFieldInfo(System.Reflection.FieldInfo property)
    {
        FieldInfo = property;
    }

    public object[] GetCustomAttributes(Type type, bool inherit)
    {
        return FieldInfo.GetCustomAttributes(type, inherit);
    }

    public object[] GetCustomAttributes(bool inherit)
    {
        return FieldInfo.GetCustomAttributes(inherit);
    }

    public virtual object GetValue(object ubAction)
    {
        return FieldInfo.GetValue(ubAction);
    }

    public virtual void SetValue(object ubAction, object value)
    {
        FieldInfo.SetValue(ubAction, value);
    }
}