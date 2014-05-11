using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBRect : UBVariable<Rect>
{
    [SerializeField]
    private Rect _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Rect;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Rect)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBRect()
        : base(false)
    {
    }

    public UBRect(Rect value)
        : base(false)
    {
        _value = value;
    }

    public UBRect(bool isResult)
        : base(isResult)
    {
    }

    public UBRect(Rect value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public static implicit operator UBRect(Rect value)
    {
        return new UBRect()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeRect();
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