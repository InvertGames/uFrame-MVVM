using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UBMaterial : UBVariable<Material>
{
    [SerializeField]
    private Material _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Material;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Material)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBMaterial()
        : base(false)
    {
    }

    public UBMaterial(Material value)
        : base(false)
    {
        _value = value;
    }

    public UBMaterial(bool isResult)
        : base(isResult)
    {
    }

    public UBMaterial(Material value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public static implicit operator UBMaterial(Material value)
    {
        return new UBMaterial()
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