using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UBTransform : UBVariable<Transform>
{
    [SerializeField]
    private Transform _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Transform;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Transform)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBTransform(Transform value, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
    }

    public UBTransform()
        : base(false)
    {
        ValueFrom = 2;
    }

    public UBTransform(Transform value)
        : base(false)
    {
        _value = value;
        ValueFrom = 2;
    }

    public UBTransform(bool isResult)
        : base(isResult)
    {
        ValueFrom = 2;
    }

    public static implicit operator UBTransform(Transform value)
    {
        return new UBTransform()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        var index = serializer.DeserializeInt();
        LiteralObjectValue = referenceHolder.ObjectReferences[index];
    }

    public override object GetObjectValue(IUBContext context)
    {
        if (ValueFrom == 2)
            return context.Transform;
        if (ValueFrom == 3 || ValueFrom == 4)
        {
            _referenceVariable = context.GetVariableById(_ValueFromVariableName);
            if (_referenceVariable != null)
            {
                if (ValueFrom == 3)
                {
                    var c = _referenceVariable.LiteralObjectValue as Component;
                    return c.transform;
                }
                else
                {
                    var c = _referenceVariable.LiteralObjectValue as GameObject;
                    return c.transform;
                }
            }
            else
            {
                throw new Exception(string.Format("{0} variable was not found.",_ValueFromVariableName));
            }
        }
        var result = base.GetObjectValue(context);
        if (result.Equals(null))
        {
            return context.Transform;
        }
        return result;
    }

    public override string GetVariableReferenceString()
    {
        if (ValueFrom == 2)
        {
            return "this.transform";
        }
        return string.Empty;
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        var index = referenceHolder.ObjectReferences.Count;
        referenceHolder.ObjectReferences.Add(LiteralValue);
        serializer.Serialize(index);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(2, "Instance");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);


        for (int index = 0; index < contextVariables.Length; index++)
        {
            var declare = contextVariables[index];
            
            if (typeof(Component).IsAssignableFrom(declare.ValueType) )
            {
                yield return new ValueFromInfo(3, declare.Name, declare.Guid);
            }
            else if (declare.ValueType == typeof (GameObject))
            {
                yield return new ValueFromInfo(4, declare.Name, declare.Guid);
            }
        }
    }
}

public class ValueFromInfo
{
    public string _DisplayName;
    public int _ValueFrom = 0;
    public string _ValueFromVariableName = string.Empty;

    public ValueFromInfo()
    {
    }

    public ValueFromInfo(int valueFrom, string displayName, string valueFromVariableName)
    {
        _DisplayName = displayName;
        _ValueFrom = valueFrom;
        _ValueFromVariableName = valueFromVariableName;
    }

    public ValueFromInfo(int valueFrom, string displayName)
    {
        _DisplayName = displayName;
        _ValueFrom = valueFrom;
    }
}