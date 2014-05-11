using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using Object = UnityEngine.Object;

public class ByteObject<TItemType> : IReferenceHolder where TItemType : class, IUBSerializable
{
    public byte[] _ByteData;

    [SerializeField]
    private List<int> _ActionIndexes = new List<int>();

    private int _currentFieldIndex = 0;

    [SerializeField]
    private List<int> _FieldStartIndexes = new List<int>();

    [SerializeField]
    private List<string> _FieldTypes = new List<string>();

    [NonSerialized]
    private List<TItemType> _items;

    [SerializeField]
    private List<string> _ItemTypes = new List<string>();

    [SerializeField]
    private List<UnityEngine.Object> _ObjectReferences = new List<Object>();

    private IUBehaviours _uBehaviours;

    List<UnityEngine.Object> IReferenceHolder.ObjectReferences
    {
        get { return _ObjectReferences; }
    }

    public List<TItemType> Items
    {
        get
        {
            if (_items == null)
                Load(_uBehaviours);

            return _items;
        }
        set { _items = value; }
    }

    public ByteObject()
    {
    }

    public ByteObject(ByteObject<TItemType> copy)
    {
        _ByteData = new byte[copy._ByteData.Length];
        Buffer.BlockCopy(copy._ByteData, 0, _ByteData, 0, _ByteData.Length);

        _ActionIndexes.AddRange(copy._ActionIndexes);
        _FieldStartIndexes.AddRange(copy._FieldStartIndexes);
        _FieldTypes.AddRange(copy._FieldTypes);
        _ItemTypes.AddRange(copy._ItemTypes);
        _ObjectReferences.AddRange(copy._ObjectReferences);
    }

    public virtual void AddItem(TItemType item)
    {
        Items.Add(item);
        Save(_uBehaviours);
    }

    public virtual void DeserializeItem(IUBSerializable item, UBBinarySerializer serializer)
    {
        var fields = GetSerializableFields(item).ToArray();
        var numberOfFields = serializer.DeserializeInt();
        //        Debug.Log("number of fields " + item.ToString() + " " + numberOfFields);
        item.Deserialize(this, serializer);
        var end = _currentFieldIndex + numberOfFields;
        for (var i = _currentFieldIndex; i < end; i++, _currentFieldIndex++)
        {
            serializer.MoveToIndex(_FieldStartIndexes[i]);
            var fieldName = "";
            try
            {
                //                Debug.Log("Field index " + i);
                fieldName = serializer.DeserializeString();
                var fieldType = serializer.DeserializeType();
                // Find the field of the type and name
                var field = fields.FirstOrDefault(p => p.Name == fieldName && p.FieldType == fieldType);

                if (field == null)
                {
                    // Move the serializer on to the next one
                    //if (i < _FieldStartIndexes.Count)
                    //    serializer.MoveToIndex(_FieldStartIndexes[i + 1]);
                    continue;
                }
                // Type and Name have matches we have the field we are looking for lets deserialize it
                DeserializeField(item, field, serializer);
            }
            catch (Exception ex)
            {
                // Debug.Log("Number of Fields: " + numberOfFields);
                if (fieldName == null)
                    Debug.LogWarning("There was a problem loading a field on " + item.ToString());

                // try and correct the index to the next field
                //if (i < _FieldStartIndexes.Count)
                //serializer.MoveToIndex(_FieldStartIndexes[i + 1]);
                if (fieldName == null)
                    Debug.LogException(ex);
            }
        }
        _currentFieldIndex++; // Skip past the last one?
    }

    public virtual IEnumerable<IUBFieldInfo> GetSerializableFields(IUBSerializable item)
    {
        IUBFieldInfo[] fields = null;

        //var action = item as UBAction;
        //if (action == null)
        //    yield break;

        //fields = action.GetVariableFields().ToArray();
        ////}
        ////else
        ////{
        fields = item.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new ReflectionFieldInfo(p)).Cast<IUBFieldInfo>().ToArray();

        var allowed = new Type[] { typeof(string), typeof(float), typeof(bool), typeof(int), typeof(double), typeof(Vector2), typeof(Vector3), typeof(Rect), typeof(Quaternion), typeof(Color) };

        foreach (var field in fields)
        {
            if (field.FieldType.IsArray)
                yield return field;
            else if (typeof(UBVariableBase).IsAssignableFrom(field.FieldType))
                yield return field;
            else if (allowed.Contains(field.FieldType))
                yield return field;
            else if (field.FieldType == typeof(UBActionSheet))
                yield return field;
            else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                yield return field;
            else if (typeof(Enum).IsAssignableFrom(field.FieldType))
                yield return field;
        }
    }

    public virtual void Load(IUBehaviours behaviour)
    {
        if (_items != null) return;
        _uBehaviours = behaviour;
        _currentFieldIndex = 0;
        _items = new List<TItemType>();
        if (_ByteData == null)
        {
            return;
        }
        var serializer = new UBBinarySerializer(_ByteData);
        for (int index = 0; index < _ItemTypes.Count; index++)
        {
            serializer.MoveToIndex(_ActionIndexes[index]);
            var itemTypeName = _ItemTypes[index];
            var t = UBHelper.GetType(itemTypeName);
            if (t == null) continue;
            var item = Activator.CreateInstance(t) as TItemType;
            if (item == null)
            {
                // TODO Type missing? Log error
                continue;
            }
            _items.Add(item);
            try
            {
                DeserializeItem(item, serializer);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("Unable to load {0} because it had corrupted data.", itemTypeName));
                Debug.LogException(ex);
            }
        }
    }

    public void Save()
    {
        Save(_uBehaviours);
    }

    public void Save(IUBehaviours uBehaviours)
    {
        _uBehaviours = uBehaviours;
        Clear();
        var serializer = new UBBinarySerializer();

        foreach (var item in Items)
        {
            // Keep up with the item types
            _ItemTypes.Add(item.GetType().Name);
            // Keep up with the number of indexes
            _ActionIndexes.Add(serializer.ByteArraySize);
            // Serialize the item
            SerializeItem(item, serializer);
        }

        _ActionIndexes.Add(serializer.ByteArraySize);
        _ByteData = serializer.ByteArray;
    }

    public void SerializeItem(TItemType item, UBBinarySerializer serializer)
    {
        var fields = GetSerializableFields(item).ToArray();
        //        Debug.Log(item.ToString() + fields.Length);
        // Serialize the number of fields
        serializer.Serialize(fields.Length);
        item.Serialize(this, serializer);
        foreach (var field in fields)
        {
            // Store each field start index.  It uses length of the bytestream so it will work correctly when trying to skip over a field
            _FieldStartIndexes.Add(serializer.ByteArraySize);
            // Store the type so it can be resolved
            //_FieldTypes.Add(field.FieldType.AssemblyQualifiedName);
            // Serialize the field name for versioning
            serializer.Serialize(field.Name);
            //            Debug.Log(item.ToString() + " - " + field.Name);
            serializer.Serialize(field.FieldType);
            // Serialize the field
            SerializeField(item, field, serializer);
        }
        _FieldStartIndexes.Add(serializer.ByteArraySize);
    }

    protected virtual object DeserializeFieldValue(Type t, UBBinarySerializer serializer)
    {
        if (typeof(Object).IsAssignableFrom(t))
            return _ObjectReferences[serializer.DeserializeInt()];
        if (typeof(Enum).IsAssignableFrom(t))
            return serializer.DeserializeInt();
        if (typeof(UBVariableBase).IsAssignableFrom(t))
            return DeserializeUBVariable(t, serializer);
        if (t == typeof(UBActionSheet))
        {
            var index = serializer.DeserializeInt();
            //var forwardToId = serializer.DeserializeString();
            if (index > -1 && index < _uBehaviours.Sheets.Count)
            {
                //_uBehaviours.Sheets[index].Parent = this as UBActionSheet;
                _uBehaviours.Sheets[index].RootContainer = _uBehaviours;
                _uBehaviours.Sheets[index].Load(_uBehaviours);
                return _uBehaviours.Sheets[index];
            }
            if (index > -1)
            Debug.LogError("COULDN'T FIND ACTIONSHEET AT INDEX " + index + " : " + _uBehaviours.Sheets.Count,_uBehaviours as UnityEngine.Object);
            return null;
        }
        if (t == typeof(string))
            return serializer.DeserializeString();
        if (t == typeof(float))
            return serializer.DeserializeFloat();
        if (t == typeof(float))
            return serializer.DeserializeDouble();
        if (t == typeof(int))
            return serializer.DeserializeInt();
        if (t == typeof(bool))
            return serializer.DeserializeBool();
        if (t == typeof(Color))
            return serializer.DeserializeColor();
        if (t == typeof(Vector3))
            return serializer.DeserializeVector3();
        if (t == typeof(Vector2))
            return serializer.DeserializeVector2();
        if (t == typeof(Quaternion))
            return serializer.DeserializeQuaternion();
        if (t == typeof(Rect))
            return serializer.DeserializeRect();

        return null;
    }

    protected virtual void SerializeObjectValue(UBBinarySerializer serializer, Type t, object v)
    {
        if (t.IsArray)
        {
            var arr = (Array)v;
            var elementType = t.GetElementType();
            if (arr != null)
            {
                serializer.Serialize(arr.Length);
                foreach (var item in arr)
                {
                    SerializeObjectValue(serializer, elementType, item);
                }
            }
            else
            {
                serializer.Serialize(0);
            }
        }
        else if (typeof(Object).IsAssignableFrom(t))
        {
            var index = _ObjectReferences.Count;
            _ObjectReferences.Add((Object)v);
            serializer.Serialize(index);
        }
        else if (t == typeof(UBActionSheet))
        {
            var sheet = (UBActionSheet)v;
            if (sheet == null)
            {
                serializer.Serialize(-1);
            }
            else
            {
                serializer.Serialize(_uBehaviours.Sheets.IndexOf(sheet));
            }
            //serializer.Serialize(sheet == null ? string.Empty : sheet.ForwardToId);
        }
        else if (typeof(Enum).IsAssignableFrom(t))
            serializer.Serialize((int)v);
        else if (typeof(UBVariableBase).IsAssignableFrom(t) && v != null)
            SerializeUBVariable((UBVariableBase)v, serializer);
        else if (t == typeof(string))
            serializer.Serialize((string)v);
        else if (t == typeof(float))
            serializer.Serialize((float)v);
        else if (t == typeof(int))
            serializer.Serialize((int)v);
        else if (t == typeof(double))
            serializer.Serialize((double)v);
        else if (t == typeof(bool))
            serializer.Serialize((bool)v);
        else if (t == typeof(Color))
            serializer.Serialize((Color)v);
        else if (t == typeof(Vector3))
            serializer.Serialize((Vector3)v);
        else if (t == typeof(Vector2))
            serializer.Serialize((Vector2)v);
        else if (t == typeof(Quaternion))
            serializer.Serialize((Quaternion)v);
        else if (t == typeof(Rect))
            serializer.Serialize((Rect)v);
    }

    private void Clear()
    {
        _ObjectReferences.Clear();
        _ItemTypes.Clear();
        _ActionIndexes.Clear();
        _FieldStartIndexes.Clear();
        _FieldTypes.Clear();
    }

    private void DeserializeField(IUBSerializable item, IUBFieldInfo field, UBBinarySerializer serializer)
    {
        var t = field.FieldType;
        if (t.IsArray)
        {
            var elementType = t.GetElementType();
            if (elementType == null) return;
            var length = serializer.DeserializeInt();
            Array arr = Array.CreateInstance(elementType, length);
            for (var i = 0; i < length; i++)
            {
                var arrayItem = DeserializeFieldValue(elementType, serializer);
                arr.SetValue(arrayItem, i);
            }
            field.SetValue(item, arr);
        }
        else
        {
            field.SetValue(item, DeserializeFieldValue(t, serializer));
        }
    }

    private object DeserializeUBVariable(Type t, UBBinarySerializer serializer)
    {
        var v = Activator.CreateInstance(t) as UBVariableBase;
        v.Deserialize(this, serializer);
        return v;
    }

    private void SerializeField(object item, IUBFieldInfo field, UBBinarySerializer serializer)
    {
        var t = field.FieldType;
        var v = field.GetValue(item);
        SerializeObjectValue(serializer, t, v);
    }

    private void SerializeUBVariable(UBVariableBase ubVariableBase, UBBinarySerializer serializer)
    {
        ubVariableBase.Serialize(this, serializer);
    }
}