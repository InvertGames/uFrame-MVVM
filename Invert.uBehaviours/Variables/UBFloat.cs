using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class UBFloat : UBVariable<float>
{
    [SerializeField]
    private float _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Float;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (float)value;
        }
    }

    public override Type ValueType
    {
        get { return typeof(float); }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBFloat()
        : base(false)
    {
    }

    public UBFloat(float value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public UBFloat(float value)
        : base(false)
    {
        _value = value;
    }

    public UBFloat(bool isResult)
        : base(isResult)
    {
    }

    public static implicit operator UBFloat(float value)
    {
        return new UBFloat()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeFloat();
    }

    public override object GetObjectValue(IUBContext context)
    {
        if (ValueFrom == 3)
            return Time.deltaTime;
        if (ValueFrom == 2)
            return Time.time;

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
        return _value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
        yield return new ValueFromInfo(2, "Time");
        yield return new ValueFromInfo(3, "DeltaTime");
    }
}