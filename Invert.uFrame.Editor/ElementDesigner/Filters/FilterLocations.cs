using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FilterLocations
{
    [SerializeField]
    private List<string> _keys = new List<string>();

    [SerializeField]
    private List<Vector2> _values = new List<Vector2>();

    public Vector2 this[IDiagramItem item]
    {
        get
        {
            var indexOf = Keys.IndexOf(item.Identifier);
            if (indexOf > -1)
            {
                return Values[indexOf];
            }

            return item.Location;
        }
        set
        {
            var indexOf = Keys.IndexOf(item.Identifier);
            if (indexOf > -1)
            {
                Values[indexOf] = value;
            }
            else
            {
                Add(item.Identifier, value);
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
        var index = Keys.IndexOf(key);
        Keys.RemoveAt(index);
        Values.RemoveAt(index);
    }

    protected void Add(string key, Vector2 value)
    {
        Keys.Add(key);
        Values.Add(value);
    }
}