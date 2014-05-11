using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBEnum : UBVariable<Enum>
{
    [SerializeField]
    private string _enumType;

    [SerializeField]
    private int _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Enum;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (int)value;
        }
    }

    public override string ToString(IUBehaviours behaviour)
    {
        return Enum.GetName(ObjectType, LiteralObjectValue);
    }



    public Type ObjectType
    {
        get
        {
            if (string.IsNullOrEmpty(_enumType))
                return typeof(AnimationPlayMode);
            return UBHelper.GetType(_enumType);
        }
    }

    public override Type ValueType
    {
        get { return ObjectType; }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBEnum()
        : base(false)
    {
        _enumType = typeof(AnimationPlayMode).AssemblyQualifiedName;
        _value = 0;
    }

    public UBEnum(int value)
        : base(false)
    {
        _value = value;
        _enumType = typeof(AnimationPlayMode).AssemblyQualifiedName;
    }

    public UBEnum(int value, bool isResult)
        : base(isResult)
    {
        _value = value;
        _enumType = typeof(AnimationPlayMode).AssemblyQualifiedName;
    }

    public UBEnum(Type enumType, bool isResult)
        : base(isResult)
    {
        _enumType = enumType.AssemblyQualifiedName;
        _enumType = typeof(AnimationPlayMode).AssemblyQualifiedName;
    }

    public UBEnum(Type enumType)
        : base(false)
    {
        _enumType = enumType.AssemblyQualifiedName;
    }

    public UBEnum(int value, Type enumType)
        : base(false)
    {
        _value = value;
        _enumType = enumType.AssemblyQualifiedName;
    }

    public UBEnum(int value, Type enumType, bool isResult)
        : base(isResult)
    {
        _value = value;
        _enumType = enumType.AssemblyQualifiedName;
    }

    public UBEnum(string enumType, bool isResult)
        : base(isResult)
    {
        _enumType = enumType;
    }

    public UBEnum(int value, string enumType, bool isResult)
        : base(isResult)
    {
        _value = value;
        _enumType = enumType;
    }

    public static implicit operator UBEnum(int value)
    {
        return new UBEnum()
        {
            _value = value
        };
    }

    //public T GetEnum<T>(UBContext context) where T : struct
    //{
    //    return (T)GetObjectValue(context);
    //}

    public override UBVariableDeclare CreateAsDeclare()
    {
        var result = base.CreateAsDeclare();
        result.EnumType = ValueType;
        return result;
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _enumType = serializer.DeserializeString();
        _value = serializer.DeserializeInt();
    }

    public int GetIntValue(IUBContext context)
    {
        return (int)GetObjectValue(context);
    }

    public override object GetObjectValue(IUBContext context)
    {
        return base.GetObjectValue(context);
    }

    public override string GetVariableReferenceString()
    {
        throw new NotImplementedException();
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        serializer.Serialize(_enumType);
        serializer.Serialize(_value);
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
        yield return new ValueFromInfo(2, "Instance");
    }
}