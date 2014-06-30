using System;
using System.Runtime.Serialization;
using Invert.uFrame.Editor;
using UnityEngine;

[Serializable]
public class DefaultFilter : IDiagramFilter,IJsonObject
{
    private Type[] _allowedTypes;

    [SerializeField]
    private FilterLocations _locations = new FilterLocations();
    [SerializeField]
    private FilterCollapsedDictionary _collapsedValues = new FilterCollapsedDictionary();

    public virtual bool ImportedOnly
    {
        get { return false; }
    }

    public FilterLocations Locations
    {
        get { return _locations; }
        set { _locations = value; }
    }

    public virtual string Name
    {
        get { return "All"; }
    }

    public string InfoLabel { get; private set; }

    public FilterCollapsedDictionary CollapsedValues
    {
        get { return _collapsedValues; }
        set { _collapsedValues = value; }
    }

    public virtual bool IsItemAllowed(object item, Type t)
    {
        return IsAllowed(item, t);
    }

    public virtual bool IsAllowed(object item, Type t)
    {
        return true;
    }

    public void Serialize(JSONClass cls)
    {
        cls.Add("Locations", _locations.Serialize());
        cls.Add("CollapsedValues", _collapsedValues.Serialize());
    }

    public void Deserialize(JSONClass cls)
    {
        Locations.Deserialize(cls["Locations"].AsObject);
        CollapsedValues.Deserialize(cls["CollapsedValues"].AsObject);

    }
}