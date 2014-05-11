using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class UBVariableDeclare : UBVariableBase, IUBVariableDeclare, IComparable
{
    [HideInInspector]
    public bool _Expanded = false;

    [HideInInspector]
    public bool _Expose = false;

    [HideInInspector]
    public string _GUID = null;

    [HideInInspector]
    public bool _NormalMode = false;

    [SerializeField]
    public string _objectValueType = "UnityEngine.Object, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    [SerializeField]
    private Animation _animationValue;

    [SerializeField]
    private bool _boolValue;

    [SerializeField]
    private Color _colorValue;

    [SerializeField]
    private string _enumType = "UnityEngine.AnimationPlayMode, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    [SerializeField]
    private int _enumValue;

    [SerializeField]
    private float _floatValue;

    [SerializeField]
    private GameObject _gameobjectValue;

    [SerializeField]
    private int _intValue;

    [SerializeField]
    private Material _materialValue;

    [SerializeField]
    private string _name = "New Variable";

    [SerializeField]
    private UnityEngine.Object _objectValue;

    [SerializeField]
    private Quaternion _quaternionValue;

    [SerializeField]
    private Rect _rectValue;

    [SerializeField]
    private string _stringValue;

    [SerializeField]
    private Texture _textureValue;

    [SerializeField]
    private Transform _transformValue;

    [SerializeField]
    private UBVarType _varType;

    [SerializeField]
    private Vector2 _vector2Value = Vector3.zero;

    [SerializeField]
    private Vector3 _vector3Value = Vector3.zero;

    public Animation AnimationValue
    {
        get
        {
            return _animationValue;
        }
        set
        {
            _animationValue = value;
        }
    }

    public bool BoolValue
    {
        get
        {
            return _boolValue;
        }
        set
        {
            _boolValue = value;
        }
    }

    public Color ColorValue
    {
        get
        {
            return _colorValue;
        }
        set
        {
            _colorValue = value;
        }
    }

    public object DefaultValue
    {
        get
        {
            return this.LiteralObjectValue;
        }
    }

    public Type EnumType
    {
        get
        {
            return UBHelper.GetType(_enumType) ?? typeof(AnimationPlayMode);
        }
        set { if (value.AssemblyQualifiedName != null) _enumType = value.AssemblyQualifiedName.ToString(); }
    }

    public int EnumValue
    {
        get { return _enumValue; }
        set { _enumValue = value; }
    }

    public float FloatValue
    {
        get
        {
            return _floatValue;
        }
        set
        {
            _floatValue = value;
        }
    }

    public GameObject GameObjectValue
    {
        get
        {
            return _gameobjectValue;
        }
        set
        {
            _gameobjectValue = value;
        }
    }

    public override string Guid { get { return _GUID; } }

    public int IntValue
    {
        get
        {
            return _intValue;
        }
        set
        {
            _intValue = value;
        }
    }

    public override object LiteralObjectValue
    {
        get
        {
            switch (VarType)
            {
                case UBVarType.Animation:
                    return _animationValue;

                case UBVarType.Bool:
                    return _boolValue;

                case UBVarType.Color:
                    return _colorValue;

                case UBVarType.Float:
                    return _floatValue;

                case UBVarType.GameObject:
                    return _gameobjectValue;

                case UBVarType.Int:
                    return _intValue;

                case UBVarType.Material:
                    return _materialValue;

                case UBVarType.Quaternion:
                    return _quaternionValue;

                case UBVarType.Rect:
                    return _rectValue;

                case UBVarType.String:
                    return _stringValue;

                case UBVarType.Texture:
                    return _textureValue;

                case UBVarType.Vector2:
                    return _vector2Value;

                case UBVarType.Vector3:
                    return _vector3Value;

                case UBVarType.Enum:
                    return _enumValue;
                //case UBVarType.Transform:
                //    return _transfo
                default:
                    return _objectValue;
            }
        }
        set
        {
            switch (VarType)
            {
                case UBVarType.Animation:
                    _animationValue = (Animation)value;
                    break;

                case UBVarType.Bool:
                    _boolValue = (bool)value;
                    break;

                case UBVarType.Color:
                    _colorValue = (Color)value;
                    break;

                case UBVarType.Float:
                    _floatValue = (float)value;
                    break;

                case UBVarType.GameObject:
                    _gameobjectValue = (GameObject)value;
                    break;

                case UBVarType.Int:
                    _intValue = (int)value;
                    break;

                case UBVarType.Material:
                    _materialValue = (Material)value;
                    break;

                case UBVarType.Quaternion:
                    _quaternionValue = (Quaternion)value;
                    break;

                case UBVarType.Rect:
                    _rectValue = (Rect)value;
                    break;

                case UBVarType.String:
                    _stringValue = (string)value;
                    break;

                case UBVarType.Texture:
                    _textureValue = (Texture)value;
                    break;

                case UBVarType.Vector2:
                    _vector2Value = (Vector2)value;
                    break;

                case UBVarType.Vector3:
                    _vector3Value = (Vector3)value;
                    break;

                case UBVarType.Enum:
                    _enumValue = (int)value;
                    break;
                //case UBVarType.Transform:
                //      _transfo
                default:
                    _objectValue = (Object)value;
                    break;
            }
        }
    }

    public Material MaterialValue
    {
        get
        {
            return _materialValue;
        }
        set
        {
            _materialValue = value;
        }
    }

    public override string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Type ObjectType
    {
        get
        {
            return ObjectValueType;
        }
        set
        {
            if (value != null)
                _objectValueType = value.AssemblyQualifiedName;
        }
    }

    public UnityEngine.Object ObjectValue
    {
        get
        {
            return _objectValue;
        }
        set
        {
            _objectValue = value;
        }
    }

    public Type ObjectValueType
    {
        get
        {
            return UBHelper.GetType(_objectValueType) ?? (typeof(Object));
        }
    }

    public Quaternion QuaternionValue
    {
        get
        {
            return _quaternionValue;
        }
        set
        {
            _quaternionValue = value;
        }
    }

    public Rect RectValue
    {
        get
        {
            return _rectValue;
        }
        set
        {
            _rectValue = value;
        }
    }

    public string StringValue
    {
        get
        {
            return _stringValue;
        }
        set
        {
            _stringValue = value;
        }
    }

    public Texture TextureValue
    {
        get
        {
            return _textureValue;
        }
        set
        {
            _textureValue = value;
        }
    }

    public Transform TransformValue
    {
        get { return _transformValue; }
        set { _transformValue = value; }
    }

    public override Type ValueType
    {
        get { return GetTypeOfVarType(VarType); }
    }

    public override UBVarType VarType
    {
        get { return _varType; }
        set { _varType = value; }
    }

    public Vector2 Vector2Value
    {
        get
        {
            return _vector2Value;
        }
        set
        {
            _vector2Value = value;
        }
    }

    public Vector3 Vector3Value
    {
        get
        {
            return _vector3Value;
        }
        set
        {
            _vector3Value = value;
        }
    }

    public UBVariableDeclare()
        : base(false)
    {
    }

    public static bool operator !=(UBVariableDeclare left, UBVariableDeclare right)
    {
        return !Equals(left, right);
    }

    public static bool operator ==(UBVariableDeclare left, UBVariableDeclare right)
    {
        return Equals(left, right);
    }

    public void Accept(IBehaviourVisitor visitor)
    {
        visitor.Visit(this);
    }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

    public UBVariableDeclare Copy()
    {
        return new UBVariableDeclare()
        {
            _name = _name,
            _GUID = _GUID,
            _varType = _varType,

            _animationValue = _animationValue,
            _boolValue = _boolValue,
            _colorValue = _colorValue,
            _stringValue = _stringValue,
            _intValue = _intValue,
            _floatValue = _floatValue,
            _vector2Value = _vector2Value,
            _vector3Value = _vector3Value,
            _quaternionValue = _quaternionValue,
            _objectValue = _objectValue,
            _gameobjectValue = _gameobjectValue,
            _transformValue = _transformValue,
            _rectValue = _rectValue,
            _materialValue = _materialValue,
            _textureValue = _textureValue
        };
    }

    public override UBVariableDeclare CreateAsDeclare()
    {
        var declare = new UBVariableDeclare
        {
            Name = Name,
            _boolValue = _boolValue,
            _animationValue = _animationValue,
            _colorValue = _colorValue,
            _enumValue = _enumValue,
            _enumType = _enumType,
            _floatValue = _floatValue,
            _gameobjectValue = _gameobjectValue,
            _intValue = _intValue,
            _GUID = _GUID,
            _name = _name,
            _materialValue = _materialValue,
            _objectValue = _objectValue,
            _objectValueType = _objectValueType,
            _quaternionValue = _quaternionValue,
            _rectValue = _rectValue,
            _textureValue = _textureValue,
            _stringValue = _stringValue,
            _varType = _varType,
            _transformValue = _transformValue,
            _vector2Value = _vector2Value,
            _vector3Value = _vector3Value
        };

        return declare;
    }

    public UBVariableBase CreateUBVariable()
    {
        switch (VarType)
        {
            case UBVarType.String:
                return new UBString(StringValue) { Name = Name, Guid = Guid };

            case UBVarType.Bool:
                return new UBBool(BoolValue) { Name = Name, Guid = Guid };

            case UBVarType.Int:
                return new UBInt(IntValue) { Name = Name, Guid = Guid };

            case UBVarType.Float:
                return new UBFloat(FloatValue) { Name = Name, Guid = Guid };

            case UBVarType.Color:
                return new UBColor(ColorValue) { Name = Name, Guid = Guid };

            case UBVarType.Quaternion:
                return new UBQuaternion(QuaternionValue) { Name = Name, Guid = Guid };

            case UBVarType.Rect:
                return new UBRect(RectValue) { Name = Name, Guid = Guid };

            case UBVarType.Texture:
                return new UBTexture(TextureValue) { Name = Name, Guid = Guid };

            case UBVarType.Vector2:
                return new UBVector2(Vector2Value) { Name = Name, Guid = Guid };

            case UBVarType.Vector3:
                return new UBVector3(Vector3Value) { Name = Name, Guid = Guid };

            case UBVarType.GameObject:
                return new UBGameObject(GameObjectValue) { Name = Name, Guid = Guid };

            case UBVarType.Object:
                return new UBObject(ObjectValue, ObjectType, false) { Name = Name, Guid = Guid };

            case UBVarType.Material:
                return new UBMaterial(MaterialValue) { Name = Name, Guid = Guid };

            case UBVarType.Animation:
                return new UBAnimation(AnimationValue) { Name = Name, Guid = Guid };

            case UBVarType.Transform:
                return new UBTransform(TransformValue) { Name = Name, Guid = Guid };

            case UBVarType.Enum:
                return new UBEnum(EnumValue, EnumType) { Name = Name, Guid = Guid, };
        }
        return null;
    }

    public override void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        var other = obj as UBVariableDeclare;
        return other != null && Equals(other);
    }

    public override int GetHashCode()
    {
        return (_GUID != null ? _GUID.GetHashCode() : 0);
    }

    public Type GetTypeOfVarType(UBVarType varType)
    {
        switch (varType)
        {
            case UBVarType.Bool:
                return typeof(bool);

            case UBVarType.Color:
                return typeof(Color);

            case UBVarType.Float:
                return typeof(float);

            case UBVarType.GameObject:
                return typeof(GameObject);

            case UBVarType.Int:
                return typeof(int);

            case UBVarType.Quaternion:
                return typeof(Quaternion);

            case UBVarType.Rect:
                return typeof(Rect);

            case UBVarType.String:
                return typeof(string);

            case UBVarType.Texture:
                return typeof(Texture);

            case UBVarType.Vector2:
                return typeof(Vector2);

            case UBVarType.Vector3:
                return typeof(Vector3);

            case UBVarType.Animation:
                return typeof(Animation);

            case UBVarType.Material:
                return typeof(Material);

            case UBVarType.Transform:
                return typeof(Transform);

            case UBVarType.Enum:
                return this.EnumType;

            case UBVarType.Object:
                return this.ObjectType;
        }
        return typeof(object);
    }

    public override string GetVariableReferenceString()
    {
        throw new NotImplementedException();
    }

    public void Push(IUBContext context)
    {
        context.Variables.Add(CreateUBVariable());
    }

    public override void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
    }

    protected bool Equals(UBVariableDeclare other)
    {
        return string.Equals(_GUID, other._GUID);
    }

    protected override IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables)
    {
        yield break;
    }
}