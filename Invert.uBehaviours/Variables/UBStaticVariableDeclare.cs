using System;

public class UBStaticVariableDeclare : IUBVariableDeclare
{
    private string _enumType;
    private string _name;
    private string _objectType;
    private Type _valueType;
    private string _guid;
    private object _defaultValue;

    public object DefaultValue
    {
        get { return _defaultValue; }
        set
        {
            _defaultValue = value;
            if (ValueType == null && value != null) 
                ValueType = value.GetType();
        }
    }

    public Type EnumType
    {
        get
        {
            if (_enumType == null)
                return null;
            return UBHelper.GetType(_enumType);
        }
        set
        {
            _enumType = value.AssemblyQualifiedName;
        }
    }

    public string Guid
    {
        get { return string.IsNullOrEmpty(_guid) ? _name : _guid; }
        set { _guid = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Type ObjectType
    {
        get
        {
            return UBHelper.GetType(_objectType);
        }
        set
        {
            _objectType = value.AssemblyQualifiedName;
        }
    }

    public Type ValueType
    {
        get { return _valueType; }
        set
        {
            _valueType = value;
            if (_valueType != null)
            {
                if (typeof(Enum).IsAssignableFrom(_valueType))
                {
                    _enumType = _valueType.AssemblyQualifiedName;
                }
                if (typeof(UnityEngine.Object).IsAssignableFrom(_valueType))
                {
                    _objectType = _valueType.AssemblyQualifiedName;
                }
            }
        }
    }

    public UBStaticVariableDeclare()
    {

    }

    public void Accept(IBehaviourVisitor visitor)
    {
        visitor.Visit(this);
    }
}