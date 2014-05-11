using System;
using System.Collections.Generic;
using UnityEngine;

//public class UBList<TItemType> : UBVariable<ICollection<TItemType>> where TItemType : UBVariableBase
//{
//    public override object LiteralObjectValue { get; set; }

//    public UBList(bool isResult)
//        : base(isResult)
//    {
//    }

//    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
//    {
//        throw new NotImplementedException();
//    }

//    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
//    {
//        throw new NotImplementedException();
//    }

//    protected override void FillValueFromOptions(Dictionary<string, int> list)
//    {
//        throw new NotImplementedException();
//    }
//}

[Serializable]
public class UBSystemObject : UBVariable<object>
{
    [SerializeField]
    private string _objectType;

    [SerializeField]
    private object _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.SystemObject;

    public override object LiteralObjectValue
    {
        get { return _value; }
        set { _value = value; }
    }

    public Type ObjectType
    {
        get
        {
            if (string.IsNullOrEmpty(_objectType))
                return null;
            return UBHelper.GetType(_objectType);
        }
    }

    public override Type ValueType
    {
        get
        {
            if (ObjectType == null)
            {
                return typeof(object);
            }
            return ObjectType;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBSystemObject()
        : base(false)
    {
        ValueFrom = 2;
        _objectType = typeof(object).AssemblyQualifiedName;
    }

    public UBSystemObject(object value)
        : base(false)
    {
        ValueFrom = 2;
        _value = value;
        _objectType = typeof(object).AssemblyQualifiedName;
    }

    public UBSystemObject(object value, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = typeof(object).AssemblyQualifiedName;
    }

    public UBSystemObject(Type objectType, bool isResult)
        : base(isResult)
    {
        _objectType = objectType.AssemblyQualifiedName;
    }

    public UBSystemObject(Type objectType)
        : base(false)
    {
        _objectType = objectType.AssemblyQualifiedName;
    }

    public UBSystemObject(object value, Type objectType)
        : base(false)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = objectType.AssemblyQualifiedName;
    }

    public UBSystemObject(object value, Type objectType, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = objectType.AssemblyQualifiedName;
    }

    public UBSystemObject(string objectType, bool isResult)
        : base(isResult)
    {
        _objectType = objectType;
        ValueFrom = 2;
    }

    public UBSystemObject(object value, string objectType, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = objectType;
    }

    public override UBVariableDeclare CreateAsDeclare()
    {
        var result = base.CreateAsDeclare();
        result._objectValueType = ValueType.AssemblyQualifiedName;
        return result;
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
    }

    public override object GetObjectValue(IUBContext context)
    {
        var result = base.GetObjectValue(context);

        return result;
    }

    public T GetValueAs<T>(IUBContext context)
    {
        try
        {
            return (T)GetValue(context);
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("Couldn't convert variable to {0} on GetValueAs<{0}>", typeof(T).Name));
        }
    }

    public override string GetVariableReferenceString()
    {
        return null;
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
    }
}