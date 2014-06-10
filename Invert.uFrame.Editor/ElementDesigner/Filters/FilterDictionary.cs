using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FilterDictionary<TValue>
{
    [SerializeField]
    private List<string> _keys = new List<string>();

    [SerializeField]
    private List<TValue> _values = new List<TValue>();

    public TValue this[IDiagramNode node]
    {
        get
        {
            var indexOf = Keys.IndexOf(node.Identifier);
            if (indexOf > -1)
            {
                return Values[indexOf];
            }

            return default(TValue);
        }
        set
        {
            var indexOf = Keys.IndexOf(node.Identifier);
            if (indexOf > -1)
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

    public List<TValue> Values
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

    protected void Add(string key, TValue value)
    {
        Keys.Add(key);
        Values.Add(value);
    }
}