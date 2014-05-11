using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBBool : UBVariable<bool>
{
    [SerializeField]
    private bool _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Bool;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (bool)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBBool(bool value, bool isResult = false)
        : base(isResult)
    {
        _value = value;
    }

    public UBBool()
        : base(false)
    {
    }

    public static implicit operator UBBool(bool value)
    {
        return new UBBool()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeBool();
    }

    public override string GetVariableReferenceString()
    {
        throw new NotImplementedException();
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        serializer.Serialize(_value);
    }

    public override string ToString()
    {
        if (_value == null) return string.Empty;
        return _value.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Literal", string.Empty);
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
    }
}