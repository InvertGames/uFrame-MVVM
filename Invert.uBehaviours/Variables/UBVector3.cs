using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class UBVector3 : UBVariable<Vector3>
{
    [SerializeField]
    private Vector3 _value = Vector3.zero;

    [SerializeField]
    private UBVarType _varType = UBVarType.Vector3;

    public override object LiteralObjectValue
    {
        get
        {
            return _value;
        }
        set
        {
            _value = (Vector3)value;
        }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public UBVector3()
        : base(false)
    {
    }

    public UBVector3(Vector3 value)
        : base(false)
    {
        _value = value;
    }

    public UBVector3(bool isResult)
        : base(isResult)
    {
    }

    public UBVector3(Vector3 value, bool isResult)
        : base(isResult)
    {
        _value = value;
    }

    public static implicit operator UBVector3(Vector3 value)
    {
        return new UBVector3()
        {
            _value = value
        };
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _value = serializer.DeserializeVector3();
    }

    public override object GetObjectValue(IUBContext context)
    {
        if (ValueFrom > 1 || ValueFrom < 17)
        {
            if (_referenceVariable == null)
            _referenceVariable = context.GetVariableById(_ValueFromVariableName);
            if (_referenceVariable == null && ValueFrom > 6)
            {
                throw new Exception(string.Format("{0} variable was not found.", _ValueFromVariableName ?? string.Empty));
            }
            else if (ValueFrom > 6)
            {
                if (_referenceVariable.LiteralObjectValue == null)
                {
                    throw new Exception(string.Format("A variable is null."));
                }
            }
            switch (ValueFrom)
            {
                case 2:
                    return context.Transform.position;
                case 3:
                    return context.Transform.localPosition;
                case 4:
                    return context.Transform.rotation.eulerAngles;
                case 5:
                    return context.Transform.localRotation.eulerAngles;
                case 6:
                    return context.Transform.localScale;

                case 7:
                    var c7 = _referenceVariable.LiteralObjectValue as Component;
                    return c7.transform.position;
                case 8:
                    var c8 = _referenceVariable.LiteralObjectValue as Component;
                    return c8.transform.localPosition;
                case 9:
                    var c9 = _referenceVariable.LiteralObjectValue as Component;
                    return c9.transform.rotation.eulerAngles;
                case 10:
                    var c10 = _referenceVariable.LiteralObjectValue as Component;
                    return c10.transform.localRotation.eulerAngles;
                case 11:
                    var c11 = _referenceVariable.LiteralObjectValue as Component;
                    return c11.transform.localScale;

                case 12:
                    var c12 = _referenceVariable.LiteralObjectValue as GameObject;
                   
                    return c12.transform.position;
                case 13:
                    var c13 = _referenceVariable.LiteralObjectValue as GameObject;
                    return c13.transform.localPosition;
                case 14:
                    var c14 = _referenceVariable.LiteralObjectValue as GameObject;
                    return c14.transform.rotation.eulerAngles;
                case 15:
                    var c15 = _referenceVariable.LiteralObjectValue as GameObject;
                    return c15.transform.localRotation.eulerAngles;
                case 16:
                    var c16 = _referenceVariable.LiteralObjectValue as GameObject;
                    return c16.transform.localScale;
            }
        }


        return base.GetObjectValue(context);
    }
    public override void SetObjectValue(IUBContext context,object value)
    {
        if (ValueFrom > 1 || ValueFrom < 17)
        {
            if (_referenceVariable == null)
                _referenceVariable = context.GetVariableById(_ValueFromVariableName);
            if (_referenceVariable == null && ValueFrom > 6)
            {
                throw new Exception(string.Format("{0} variable was not found.", _ValueFromVariableName ?? string.Empty));
            }
            else if (ValueFrom > 6)
            {
                if (_referenceVariable.LiteralObjectValue == null)
                {
                    throw new Exception(string.Format("A variable is null."));
                }
            }
            switch (ValueFrom)
            {
                case 2:
                    context.Transform.position = (Vector3)value;
                    return;
                case 3:
                     context.Transform.localPosition = (Vector3)value;
                     return;
                case 4:
                     context.Transform.rotation = Quaternion.Euler((Vector3)value);
                     return;
                case 5:
                     context.Transform.localRotation = Quaternion.Euler((Vector3)value);
                     return;
                case 6:
                     context.Transform.localScale = (Vector3)value;
                     return;
                case 7:
                    var c7 = _referenceVariable.LiteralObjectValue as Component;
                     c7.transform.position = (Vector3)value;
                     return;
                case 8:
                    var c8 = _referenceVariable.LiteralObjectValue as Component;
                     c8.transform.localPosition = (Vector3)value;
                     return;
                case 9:
                    var c9 = _referenceVariable.LiteralObjectValue as Component;
                    c9.transform.rotation = Quaternion.Euler((Vector3)value);
                     return;
                case 10:
                    var c10 = _referenceVariable.LiteralObjectValue as Component;
                    c10.transform.localRotation = Quaternion.Euler((Vector3)value);
                     return;
                case 11:
                    var c11 = _referenceVariable.LiteralObjectValue as Component;
                     c11.transform.localScale = (Vector3)value;
                     return;

                case 12:
                    var c12 = _referenceVariable.LiteralObjectValue as GameObject;

                     c12.transform.position = (Vector3)value;
                     return;
                case 13:
                    var c13 = _referenceVariable.LiteralObjectValue as GameObject;
                     c13.transform.localPosition = (Vector3)value;
                     return;
                case 14:
                    var c14 = _referenceVariable.LiteralObjectValue as GameObject;
                     c14.transform.rotation = Quaternion.Euler((Vector3)value);
                     return;
                case 15:
                    var c15 = _referenceVariable.LiteralObjectValue as GameObject;
                    c15.transform.localRotation = Quaternion.Euler((Vector3)value);
                     return;
                case 16:
                    var c16 = _referenceVariable.LiteralObjectValue as GameObject;
                     c16.transform.localScale = (Vector3)value;
                     return;
            }
        }


        base.SetObjectValue(context,value);
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
        yield return new ValueFromInfo(2, "Instance" + "-Position");
        yield return new ValueFromInfo(3, "Instance" + "-LocalPosition");
        yield return new ValueFromInfo(4, "Instance" + "-Rotation");
        yield return new ValueFromInfo(5, "Instance" + "-LocalRotation");
        yield return new ValueFromInfo(6, "Instance" + "-LocalScale");

        for (int index = 0; index < contextVariables.Length; index++)
        {
            var declare = contextVariables[index];

            if (typeof(Component).IsAssignableFrom(declare.ValueType))
            {
                yield return new ValueFromInfo(7, declare.Name + "-Position", declare.Guid);
                yield return new ValueFromInfo(8, declare.Name + "-LocalPosition", declare.Guid);
                yield return new ValueFromInfo(9, declare.Name + "-Rotation", declare.Guid);
                yield return new ValueFromInfo(10, declare.Name + "-LocalRotation", declare.Guid);
                yield return new ValueFromInfo(11, declare.Name + "-LocalScale", declare.Guid);
            }
            else if (declare.ValueType == typeof(GameObject))
            {
                yield return new ValueFromInfo(12, declare.Name + "-Position", declare.Guid);
                yield return new ValueFromInfo(13, declare.Name + "-LocalPosition", declare.Guid);
                yield return new ValueFromInfo(14, declare.Name + "-Rotation", declare.Guid);
                yield return new ValueFromInfo(15, declare.Name + "-LocalRotation", declare.Guid);
                yield return new ValueFromInfo(16, declare.Name + "-LocalScale", declare.Guid);
            }

        }
    }
}