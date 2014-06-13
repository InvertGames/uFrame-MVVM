using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Trigger information
/// </summary>
[Serializable]
public class TriggerInfo
{
    [SerializeField]
    private string _data;

    [SerializeField]
    private string _displayName;

    [SerializeField]
    private string _guid;

    [SerializeField]
    private bool _isStatic = false;

    [SerializeField]
    private UBActionSheet _sheet;

    [SerializeField]
    private string _triggerTypeName;

    /// <summary>
    /// Data that can be used if the trigger is predefined.
    /// </summary>
    public string Data
    {
        get { return _data; }
        set { _data = value; }
    }

    /// <summary>
    /// The name that is displayed without any spaces
    /// </summary>
    public string DisplayName
    {
        get { return _displayName; }
        set { _displayName = value; }
    }

    public bool Exists
    {
        get { return Sheet != null; }
    }

    /// <summary>
    /// The identifier for this trigger.
    /// </summary>
    public string Guid
    {
        get { return _guid ?? (_guid = System.Guid.NewGuid().ToString()); }
        set { _guid = value; }
    }

    public bool IsCustom
    {
        get { return typeof(UBCustomTrigger).IsAssignableFrom(TriggerType); }
    }

    public bool IsStatic
    {
        get { return _isStatic; }
        set { _isStatic = value; }
    }

    public string Name
    {
        get
        {
            return this.DisplayName;
        }
    }

    public IUBehaviours Owner
    {
        get
        {
            if (Sheet == null)
                return null;
            return Sheet.RootContainer;
        }
    }

    /// <summary>
    /// The sheet that belongs to this trigger.
    /// </summary>
    public UBActionSheet Sheet
    {
        get { return _sheet; }
        set { _sheet = value; }
    }

    /// <summary>
    /// The type of trigger that will be created upon load.
    /// </summary>
    public Type TriggerType
    {
        get { return UBHelper.GetType(_triggerTypeName); }
        set { _triggerTypeName = value.AssemblyQualifiedName; }
    }

    /// <summary>
    /// The string type name of the triggertype.
    /// </summary>
    public string TriggerTypeName
    {
        get { return _triggerTypeName; }
        set { _triggerTypeName = value; }
    }

    public TriggerInfo()
    {
    }

    public IEnumerable<UBSettingAttribute> GetSettings()
    {
        var attributes = TriggerType.GetCustomAttributes(typeof(UBSettingAttribute), true);

        return attributes.OfType<UBSettingAttribute>().ToArray();
    }
}