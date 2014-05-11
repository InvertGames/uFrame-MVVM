using System;
using System.Reflection;

public class ArrayItemFieldInfo : ReflectionFieldInfo
{
    private readonly int _arrayIndex;

    public override Type FieldType
    {
        get
        {
            return FieldInfo.FieldType.GetElementType();
        }
    }

    public override string Name
    {
        get { return string.Format("{0} {1}", base.Name, _arrayIndex); }
    }

    public ArrayItemFieldInfo(FieldInfo property, int arrayIndex)
        : base(property)
    {
        _arrayIndex = arrayIndex;
    }

    public override object GetValue(object ubAction)
    {
        var array = FieldInfo.GetValue(ubAction) as Array;
        return array.GetValue(_arrayIndex);
    }

    public override void SetValue(object ubAction, object value)
    {
        var array = FieldInfo.GetValue(ubAction) as Array;
        array.SetValue(value, _arrayIndex);
    }
}