using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBColor : UBVariable<Color>
{
    [SerializeField]
    private Color _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Color;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Color)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBColor()
        : base(false)
    {
    }

    public UBColor(Color value)
        : base(false)
    {
        _value = value;
    }

    public UBColor(Color value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public UBColor(bool isResult)
        : base(isResult)
    {
    }

    public static implicit operator UBColor(Color value)
    {
        return new UBColor()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeColor();
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
        //if (_value == null) return string.Empty;
        return _value.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
    }
}