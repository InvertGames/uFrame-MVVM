using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class UBGameObject : UBVariable<GameObject>
{
    [SerializeField]
    private GameObject _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.GameObject;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (GameObject)value;
        }
    }

    public override Type ValueType
    {
        get { return typeof(GameObject); }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBGameObject()
        : base(false)
    {
        ValueFrom = 2;
    }

    public UBGameObject(GameObject value)
        : base(false)
    {
        _value = value;
        ValueFrom = 2;
    }

    public UBGameObject(bool isResult)
        : base(isResult)
    {
        ValueFrom = 2;
    }

    public UBGameObject(GameObject value, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
    }

    public static implicit operator UBGameObject(GameObject value)
    {
        return new UBGameObject()
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
            return context.GameObject;
        if (ValueFrom == 3)
        {
            _referenceVariable = context.GetVariableById(_ValueFromVariableName);
            if (_referenceVariable != null)
            {
                if (ValueFrom == 3)
                {
                    var c = _referenceVariable.LiteralObjectValue as Component;
                    if (c == null)
                    {
                        Debug.Log("Component is null");
                    }
                    return c.gameObject;
                }
            }
            else
            {
                throw new Exception(string.Format("{0} variable was not found.",_ValueFromVariableName));
            }
        }

        var result = base.GetObjectValue(context);

        return result;
    }

    public override string GetVariableReferenceString()
    {
        if (ValueFrom == 2)
        {
            return "this.gameObject";
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
        
        if (_value != null)
        return _value.ToString();

        return base.ToString();
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(2, "Instance");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);

        for (int index = 0; index < contextVariables.Length; index++)
        {
            var declare = contextVariables[index];

            if (typeof(Component).IsAssignableFrom(declare.ValueType))
            {
                yield return new ValueFromInfo(3, declare.Name, declare.Guid);
            }
        }
    }
}