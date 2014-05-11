using System;
using System.Reflection;

public interface IUBFieldInfo
{
    FieldInfo FieldInfo { get; set; }

    Type FieldType { get; }

    string Name { get; }

    object[] GetCustomAttributes(Type type, bool inherit);

    object[] GetCustomAttributes(bool inherit);

    object GetValue(object ubAction);

    void SetValue(object ubAction, object value);
}

public class UBFieldInfo : IUBFieldInfo
{
    public FieldInfo FieldInfo { get; set; }
    public Type FieldType { get; set; }
    public string Name { get; set; }
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