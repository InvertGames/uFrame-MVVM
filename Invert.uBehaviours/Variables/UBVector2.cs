using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBVector2 : UBVariable<Vector2>
{
    [SerializeField]
    private Vector2 _value = Vector2.zero;

    [SerializeField]
    private UBVarType _varType = UBVarType.Vector2;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Vector2)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBVector2()
        : base(false)
    {
    }

    public UBVector2(Vector2 value)
        : base(false)
    {
        _value = value;
    }

    public UBVector2(Vector2 value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public UBVector2(bool isResult)
        : base(isResult)
    {
    }

    public static implicit operator UBVector2(Vector2 value)
    {
        return new UBVector2()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeVector2();
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