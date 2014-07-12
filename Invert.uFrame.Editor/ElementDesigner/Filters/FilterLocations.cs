using System;
using System.Collections.Generic;
using Invert.uFrame.Editor;
using UnityEngine;

[Serializable]
public class FilterLocations
{
    [SerializeField]
    private List<string> _keys = new List<string>();

    [SerializeField]
    private List<Vector2> _values = new List<Vector2>();

    public Vector2 this[IDiagramNode node]
    {
        get
        {
            var indexOf = Keys.IndexOf(node.Identifier);
            if (indexOf > -1)
            {
                return Values[indexOf];
            }

            return node.Location;
        }
        set
        {
            var indexOf = Keys.IndexOf(node.Identifier);
            if (indexOf != -1)
            {
                Values[indexOf] = value;
            }
            else
            {
                Add(node.Identifier, value);
            }
        }
    }

    public List<string> Keys
    {
        get { return _keys; }
        set { _keys = value; }
    }

    public List<Vector2> Values
    {
        get { return _values; }
        set { _values = value; }
    }

    public void Remove(string key)
    {
        if (key == null) return;
        var index = Keys.IndexOf(key);
        Keys.RemoveAt(index);
        Values.RemoveAt(index);
    }

    protected void Add(string key, Vector2 value)
    {
        if (Keys.Contains(key)) return;
        Keys.Add(key);
        Values.Add(value);
    }

    public JSONClass  Serialize()
    {
        JSONClass cls = new JSONClass();
        for (int index = 0; index < _keys.Count; index++)
        {
            var key = _keys[index];
            var value = _values[index];
            cls.Add(key, SerializeValue(value));
        }
        return cls;
    }

    protected JSONNode SerializeValue(Vector2 value)
    {
        return new JSONClass
        {
            AsVector2 = value
        };
    }

    public void Deserialize(JSONClass cls)
    {
        foreach (KeyValuePair<string, JSONNode> cl in cls)
        {

            Add(cl.Key, DeserializeValue(cl.Value));
        }
    }

    protected Vector2 DeserializeValue(JSONNode value)
    {
        return value.AsVector2;
    }
}