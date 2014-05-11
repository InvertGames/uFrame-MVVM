using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBQuaternion : UBVariable<Quaternion>
{
    [SerializeField]
    private Quaternion _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Quaternion;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Quaternion)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBQuaternion()
        : base(false)
    {
    }

    public UBQuaternion(Quaternion value)
        : base(false)
    {
        _value = value;
    }

    public UBQuaternion(bool isResult)
        : base(isResult)
    {
    }

    public UBQuaternion(Quaternion value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public static implicit operator UBQuaternion(Quaternion value)
    {
        return new UBQuaternion()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeQuaternion();
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