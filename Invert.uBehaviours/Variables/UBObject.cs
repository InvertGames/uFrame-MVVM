using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class UBObject : UBVariable<UnityEngine.Object>
{
    [SerializeField]
    private string _objectType;

    [SerializeField]
    private UnityEngine.Object _value;

    [SerializeField]
    private UBVarType _varType = UBVarType.Object;

    public override object LiteralObjectValue
    {
        get { return _value; }
        set { _value = value as UnityEngine.Object; }
    }

    public Type ObjectType
    {
        get
        {
            if (string.IsNullOrEmpty(_objectType))
                return null;
            return UBHelper.GetType(_objectType);
        }
    }

    public override Type ValueType
    {
        get
        {
            if (ObjectType == null)
            {
                return typeof(Object);
            }
            return ObjectType;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBObject()
        : base(false)
    {
        ValueFrom = 2;
        _objectType = typeof(UnityEngine.Object).AssemblyQualifiedName;
    }

    public UBObject(UnityEngine.Object value)
        : base(false)
    {
        ValueFrom = 2;
        _value = value;
        _objectType = typeof(UnityEngine.Object).AssemblyQualifiedName;
    }

    public UBObject(Object value, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = typeof(UnityEngine.Object).AssemblyQualifiedName;
    }

    public UBObject(Type objectType, bool isResult)
        : base(isResult)
    {
        _objectType = objectType.AssemblyQualifiedName;
    }

    public UBObject(Type objectType)
        : base(false)
    {
        _objectType = objectType.AssemblyQualifiedName;
    }

    public UBObject(Object value, Type objectType, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = objectType.AssemblyQualifiedName;
    }
    public UBObject(Object value, Type objectType)
        : base(false)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = objectType.AssemblyQualifiedName;
    }
    public UBObject(string objectType, bool isResult)
        : base(isResult)
    {
        _objectType = objectType;
        ValueFrom = 2;
    }

    public UBObject(Object value, string objectType, bool isResult)
        : base(isResult)
    {
        _value = value;
        ValueFrom = 2;
        _objectType = objectType;
    }

    public override UBVariableDeclare CreateAsDeclare()
    {
        var result = base.CreateAsDeclare();
        result._objectValueType = ValueType.AssemblyQualifiedName;
        return result;
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _objectType = serializer.DeserializeString();
        var index = serializer.DeserializeInt();
        LiteralObjectValue = referenceHolder.ObjectReferences[index];
    }

    public override object GetObjectValue(IUBContext context)
    {

        if (ValueFrom == 2)
        {
            if (context.GameObject == null)
            {
                Debug.LogError("GameObject is null");
            }
            else
            {
                Debug.Log(ObjectType.AssemblyQualifiedName, context.GameObject);
            }
            return context.GameObject.GetComponent(ObjectType);
        }
            

        if (ValueFrom == 3 || ValueFrom == 4)
        {
            _referenceVariable = context.GetVariableById(_ValueFromVariableName);
            if (_referenceVariable != null)
            {
                if (ValueFrom == 3)
                {
                    var c = _referenceVariable.LiteralObjectValue as Component;
                    return c.GetComponent(ObjectType);
                }
                else
                {
                    var c = _referenceVariable.LiteralObjectValue as GameObject;
                    return c.GetComponent(ObjectType);
                }
            }
            else
            {
                throw new Exception(string.Format("{0} variable was not found.", _ValueFromVariableName));
            }
        }

        var result = base.GetObjectValue(context);

        //if (result.Equals(null))
        //    return context.GameObject.GetComponent(ObjectType);

        return result;
    }

    public T GetValueAs<T>(IUBContext context) where T : UnityEngine.Object
    {
        try
        {
            return (T)GetValue(context);
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("Couldn't convert variable to {0} on GetValueAs<{0}>", typeof(T).Name));
            return null;
        }
    }

    public override string GetVariableReferenceString()
    {
        if (this.ValueFrom == 2)
        {
            return string.Format("this.GetComponent<{0}>()", LiteralObjectValue);
        }
        return "UNKNOWN";
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        serializer.Serialize(_objectType);
        var index = referenceHolder.ObjectReferences.Count;
        referenceHolder.ObjectReferences.Add(LiteralValue);
        serializer.Serialize(index);
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield return new ValueFromInfo(0, "Specified");
        yield return new ValueFromInfo(1, "Variable",_ValueFromVariableName);
        yield return new ValueFromInfo(2, "Instance");
        for (int index = 0; index < contextVariables.Length; index++)
        {
            var declare = contextVariables[index];

            if (typeof(Component).IsAssignableFrom(declare.ValueType))
            {
                yield return new ValueFromInfo(3, string.Format("{0}-{1}Component", declare.Name, ObjectType.Name), declare.Guid);
            }
            else if (declare.ValueType == typeof(GameObject))
            {
                yield return new ValueFromInfo(4, string.Format("{0}-{1}Component", declare.Name, ObjectType.Name), declare.Guid);
            }
        }

    }
}