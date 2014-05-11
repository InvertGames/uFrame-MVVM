using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBAnimation : UBVariable<Animation>
{
    [SerializeField]
    private Animation _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Animation;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Animation)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBAnimation(bool isResult)
        : base(isResult)
    {
    }

    public UBAnimation(Animation value)
        : base(false)
    {
        _value = value;
    }

    public UBAnimation(Animation value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public UBAnimation()
        : base(false)
    {
    }

    public static implicit operator UBAnimation(Animation value)
    {
        return new UBAnimation()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        var index = serializer.DeserializeInt();
        LiteralObjectValue = referenceHolder.ObjectReferences[index];
    }

    public override string GetVariableReferenceString()
    {
        throw new NotImplementedException();
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        var index = referenceHolder.ObjectReferences.Count;
        referenceHolder.ObjectReferences.Add(LiteralValue);
        serializer.Serialize(index);
    }

    public override string ToString()
    {
        if (_value == null) return string.Empty;
        return _value.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
    }
}