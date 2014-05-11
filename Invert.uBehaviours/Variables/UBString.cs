using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBString : UBVariable<string>
{
    [SerializeField]
    private string _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.String;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (string)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBString()
        : base(false)
    {
    }

    public UBString(string value)
        : base(false)
    {
        _value = value;
    }

    public UBString(bool isResult)
        : base(isResult)
    {
    }

    public UBString(string value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public static implicit operator UBString(string value)
    {
        return new UBString()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeString();
    }

    public override object GetObjectValue(IUBContext context)
    {
        if (ValueFrom == 1)
        {
            return context.GetVariableById(_ValueFromVariableName).LiteralObjectValue.ToString();
        }

        return base.GetObjectValue(context);
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
        return _value;
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
    }
}