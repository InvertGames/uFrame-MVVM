using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBInt : UBVariable<int>
{
    [SerializeField]
    private int _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Int;

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

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBInt()
        : base(false)
    {
    }

    public UBInt(int value)
        : base(false)
    {
        _value = value;
    }

    public UBInt(int value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public UBInt(bool isResult)
        : base(isResult)
    {
    }

    public static implicit operator UBInt(int value)
    {
        return new UBInt()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeInt();
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
        return _value.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
    }
}